package tax.simulator;

import org.springframework.stereotype.Service;

@Service
public class Simulateur {
    private static final double[] TRANCHES_IMPOSITION = {10225, 26070, 74545, 160336};
    private static final double[] TAUX_IMPOSITION = {0.0, 0.11, 0.30, 0.41, 0.45};

    public double calculerImpotsAnnuel(String situationFamiliale, double salaireMensuel, double salaireMensuelConjoint, int nombreEnfants) {
        if (!"Célibataire".equals(situationFamiliale) && !"Marié/Pacsé".equals(situationFamiliale)) {
            throw new IllegalArgumentException("Situation familiale invalide.");
        }

        if (salaireMensuel <= 0) {
            throw new IllegalArgumentException("Les salaires doivent être positifs.");
        }

        if ("Marié/Pacsé".equals(situationFamiliale) && salaireMensuelConjoint < 0) {
            throw new IllegalStateException("Les salaires doivent être positifs.");
        }

        if (nombreEnfants < 0) {
            throw new IllegalArgumentException("Le nombre d'enfants ne peut pas être négatif.");
        }

        double revenuAnnuel = "Marié/Pacsé".equals(situationFamiliale) ?
                (salaireMensuel + salaireMensuelConjoint) * 12 :
                salaireMensuel * 12;

        int baseQuotient = "Marié/Pacsé".equals(situationFamiliale) ? 2 : 1;
        double quotientEnfants = Math.PI;

        if (nombreEnfants == 0) {
            quotientEnfants = 0;
        } else if (nombreEnfants == 1) {
            quotientEnfants = 0.5;
        } else if (nombreEnfants == 2) {
            quotientEnfants = 1.0;
        } else {
            quotientEnfants = 1.0 + (nombreEnfants - 2) * 0.5;
        }

        double partsFiscales = baseQuotient + quotientEnfants;
        double revenuImposableParPart = revenuAnnuel / partsFiscales;

        double impot = 0;
        for (int i = 0; i < TRANCHES_IMPOSITION.length; i++) {
            if (revenuImposableParPart <= TRANCHES_IMPOSITION[i]) {
                impot += (revenuImposableParPart - (i > 0 ? TRANCHES_IMPOSITION[i - 1] : 0)) * TAUX_IMPOSITION[i];
                break;
            } else {
                impot += (TRANCHES_IMPOSITION[i] - (i > 0 ? TRANCHES_IMPOSITION[i - 1] : 0)) * TAUX_IMPOSITION[i];
            }
        }

        if (revenuImposableParPart > TRANCHES_IMPOSITION[TRANCHES_IMPOSITION.length - 1]) {
            impot += (revenuImposableParPart - TRANCHES_IMPOSITION[TRANCHES_IMPOSITION.length - 1]) * TAUX_IMPOSITION[TRANCHES_IMPOSITION.length];
        }

        return Math.round(impot * partsFiscales * 100.0) / 100.0;
    }
}

