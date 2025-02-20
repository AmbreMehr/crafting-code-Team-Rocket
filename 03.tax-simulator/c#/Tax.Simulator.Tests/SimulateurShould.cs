using System.Data;
using Xunit;

namespace Tax.Simulator.Tests;

/// <summary>
/// Check the simulator class
/// </summary>
public class SimulateurShould
{

    #region Test User story 1
    /// <summary>
    /// Check the calcul of tax for single person with normal scenario
    /// </summary>
    [Fact]
    public void CalculTaxSingleOK()
    {
        string situationFamiliale = "Célibataire";
        decimal salaireMensuel = 2000;
        decimal salaireMensuelConjoint = 0;
        int nombreEnfants = 0;
        decimal expectedResult = 1515.25m;

        decimal result = Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        Assert.Equal(expectedResult, result);
    }

    /// <summary>
    /// Check the calcul of tax for single person with error scenario (negative salary)
    /// </summary>
    [Fact]
    public void CalculTaxSingleError()
    {
        string situationFamiliale = "Célibataire";
        decimal salaireMensuel = -2000;
        decimal salaireMensuelConjoint = 0;
        int nombreEnfants = 0;

        Assert.Throws<ArgumentException>(() => Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants));
    }
    #endregion

    #region Test User story 2

    /// <summary>
    /// Check the calcul of tax for married person with normal scenario
    /// </summary>
    [Fact]
    public void CalculTaxMarriedOK()
    {
        string situationFamiliale = "Marié/Pacsé";
        decimal salaireMensuel = 2000;
        decimal salaireMensuelConjoint = 2500;
        int nombreEnfants = 0;
        decimal expectedResult = 4043.90m;

        decimal result = Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        Assert.Equal(expectedResult, result);
    }

    /// <summary>
    /// Check the calcul of tax for married person with error scenario (negative salary)
    /// </summary>
    [Fact]
    public void CalculTaxMarriedError()
    {
        string situationFamiliale = "Marié/Pacsé";
        decimal salaireMensuel = 2000;
        decimal salaireMensuelConjoint = -2500;
        int nombreEnfants = 0;


        Assert.Throws<ArgumentException>(() => Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants));
    }


    #endregion

    #region User story 3
    /// <summary>
    /// Check the calcul of tax for married person with children
    /// </summary>
    [Fact]
    public void CalculTaxChildrenOK()
    {
        string situationFamiliale = "Marié/Pacsé";
        decimal salaireMensuel = 3000;
        decimal salaireMensuelConjoint = 3000;
        int nombreEnfants = 3;
        decimal expectedResult = 3983.37m;

        decimal result = Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants);

        Assert.Equal(expectedResult, result);
    }

    /// <summary>
    /// Check the calcul of tax for married person with children with error scenario (negative number of children)
    /// </summary>
    [Fact]
    public void CalculTaxChildrenError()
    {
        string situationFamiliale = "Marié/Pacsé";
        decimal salaireMensuel = 3000;
        decimal salaireMensuelConjoint = 3000;
        int nombreEnfants = -1;

        Assert.Throws<ArgumentException>(() => Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants));
    }
    #endregion

    #region User story 4
    /// <summary>
    /// Check the error for invalid family situation
    /// </summary>
    [Fact]
    public void CheckFamilySituation()
    {
        string situationFamiliale = "Divorcé";
        decimal salaireMensuel = 3000;
        decimal salaireMensuelConjoint = 3000;
        int nombreEnfants = 3;

        Assert.Throws<ArgumentException>(() => Simulateur.CalculerImpotsAnnuel(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants));
    }


    #endregion





}