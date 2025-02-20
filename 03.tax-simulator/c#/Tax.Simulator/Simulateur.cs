namespace Tax.Simulator;

/// <summary>
/// Simulator of taxes
/// </summary>
public static class Simulateur
{
    private static readonly decimal[] tranchesImposition = {10225m, 26070m, 74545m, 160336m}; // Slices ceiling
    private static readonly decimal[] tauxImposition = {0.0m, 0.11m, 0.30m, 0.41m, 0.45m}; // Corresponding rate
    private static readonly string[] situationsFamiliales = { "Célibataire", "Marié/Pacsé" }; // Family situations
    private static readonly decimal nombreMois = 12; // Number of months in a year (to handle 13th month if needed)

    /// <summary>
    /// Calculate the annual tax
    /// </summary>
    /// <param name="situationFamiliale">family situation</param>
    /// <param name="salaireMensuel">mensual salary</param>
    /// <param name="salaireMensuelConjoint">partner's mensual salary</param>
    /// <param name="nombreEnfants">number of children</param>
    /// <returns></returns>
    public static decimal CalculerImpotsAnnuel(
        string situationFamiliale,
        decimal salaireMensuel,
        decimal salaireMensuelConjoint,
        int nombreEnfants)
    {
        VerifierParametres(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);
        decimal revenuAnnuel = CalculerRevenuAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint);
        decimal partsFiscales = CalculerPartsFiscales(situationFamiliale, nombreEnfants);
        decimal revenuImposableParTranche = revenuAnnuel / partsFiscales;
        decimal impotsParTranche = CalculerImpotsParTranche(revenuImposableParTranche);
        decimal impotsReel = Math.Round(impotsParTranche * partsFiscales, 2);
        return impotsReel;
    }

    /// <summary>
    /// Check the parameters and throws exceptions if they are invalid
    /// </summary>
    /// <param name="situationFamiliale">family situation</param>
    /// <param name="salaireMensuel">mensual salary</param>
    /// <param name="salaireMensuelConjoint">partner's mensual salary</param>
    /// <param name="nombreEnfants">number of children</param>
    private static void VerifierParametres(
        string situationFamiliale,
        decimal salaireMensuel,
        decimal salaireMensuelConjoint,
        int nombreEnfants)
    {
        if (!situationsFamiliales.Contains(situationFamiliale))
        {
            throw new ArgumentException("Situation familiale invalide.");
        }

        if (salaireMensuel <= 0)
        {
            throw new ArgumentException("Les salaires doivent être positifs.");
        }

        if (situationFamiliale == situationsFamiliales[1] && salaireMensuelConjoint < 0)
        {
            throw new ArgumentException("Les salaires doivent être positifs.");
        }

        if (nombreEnfants < 0)
        {
            throw new ArgumentException("Le nombre d'enfants ne peut pas être négatif.");
        }
    }

    /// <summary>
    /// Calculate the annual income
    /// </summary>
    /// <param name="situationFamiliale">family situation</param>
    /// <param name="salaireMensuel">mensual salary</param>
    /// <param name="salaireMensuelConjoint">partner's mensual salary</param>
    /// <returns>annual income</returns>
    private static decimal CalculerRevenuAnnuel(string situationFamiliale, decimal salaireMensuel, decimal salaireMensuelConjoint)
    {
        decimal revenuAnnuel;
        if (situationFamiliale == situationsFamiliales[1])
        {
            revenuAnnuel = (salaireMensuel + salaireMensuelConjoint) * nombreMois;
        }
        else
        {
            revenuAnnuel = salaireMensuel * nombreMois;
        }
        return revenuAnnuel;
    }

    /// <summary>
    /// Calculate the number of fiscal parts
    /// </summary>
    /// <param name="situationFamiliale">family situation</param>
    /// <param name="nombreEnfants">number of children</param>
    /// <returns>fiscal parts</returns>
    private static decimal CalculerPartsFiscales(string situationFamiliale, int nombreEnfants)
    {
        var baseQuotient = situationFamiliale == situationsFamiliales[1] ? 2 : 1;
        decimal quotientEnfants;

        if (nombreEnfants == 0)
        {
            quotientEnfants = 0;
        }
        else if (nombreEnfants == 1)
        {
            quotientEnfants = 0.5m;
        }
        else if (nombreEnfants == 2)
        {
            quotientEnfants = 1.0m;
        }
        else
        {
            quotientEnfants = 1.0m + (nombreEnfants - 2) * 0.5m;
        }

        decimal partsFiscales = baseQuotient + quotientEnfants;
        return partsFiscales;
    }

    /// <summary>
    /// Calculate the tax per slice
    /// </summary>
    /// <param name="revenuImposableParTranche">taxable income per slice</param>
    /// <returns>tax per slice</returns>
    private static decimal CalculerImpotsParTranche(decimal revenuImposableParTranche)
    {
        decimal impots = 0;
        for (int i = 0; i < tranchesImposition.Length; i++)
        {
            if (revenuImposableParTranche <= tranchesImposition[i])
            {
                impots += (revenuImposableParTranche - (i > 0 ? tranchesImposition[i - 1] : 0)) * tauxImposition[i];
                break;
            }
            else
            {
                impots += (tranchesImposition[i] - (i > 0 ? tranchesImposition[i - 1] : 0)) * tauxImposition[i];
            }
        }

        if (revenuImposableParTranche > tranchesImposition[^1])
        {
            impots += (revenuImposableParTranche - tranchesImposition[^1]) * tauxImposition[^1];
        }

        return impots;
    }
}