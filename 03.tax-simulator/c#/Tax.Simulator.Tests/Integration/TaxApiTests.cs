using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Tax.Simulator.Tests.Integration;

/// <summary>
/// Integration tests for the API
/// </summary>
/// <param name="factory">Mock API</param>
public class TaxApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    /// <summary>
    /// Test the /api/tax/calculate endpoint with good values
    /// </summary>
    /// <param name="situationFamiliale">family situation</param>
    /// <param name="salaireMensuel">mensual salary</param>
    /// <param name="salaireMensuelConjoint">partner's mensual salary</param>
    /// <param name="nombreEnfants">number of children</param>
    /// <param name="expectedResult">result the API should respond</param>
    [Theory]
    [InlineData("Célibataire", 2000, 0, 0, 1515.25)]
    [InlineData("Marié/Pacsé", 3000, 3000, 3, 3983.37)]
    public async Task CalculateTax_ReturnsOkStatusCode(
        string situationFamiliale,
        decimal salaireMensuel,
        decimal salaireMensuelConjoint,
        int nombreEnfants,
        decimal expectedResult)
    {
        var response =
            await _client.GetAsync(FormatUrl(situationFamiliale, salaireMensuel, salaireMensuelConjoint,
                nombreEnfants));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<decimal>();
        result.Should().Be(expectedResult);
    }

    /// <summary>
    /// Test the /api/tax/calculate endpoint with wrong values
    /// </summary>
    /// <param name="situationFamiliale">family situation</param>
    /// <param name="salaireMensuel">mensual salary</param>
    /// <param name="salaireMensuelConjoint">partner's mensual salary</param>
    /// <param name="nombreEnfants">number of children</param>
    /// <param name="expectedResult">result the API should respond</param>
    [Theory]
    [InlineData("Célibataire", -1000, 0, 0)]
    [InlineData("Célibataire", 3000, 0, -1)]
    public async Task CalculateTax_ReturnsBadRequest_For_Invalid_Inputs(
        string situationFamiliale,
        decimal salaireMensuel,
        decimal salaireMensuelConjoint,
        int nombreEnfants)
        => (await _client.GetAsync(
                FormatUrl(situationFamiliale, salaireMensuel, salaireMensuelConjoint, nombreEnfants))
            ).StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);

    private static string FormatUrl(
        string situationFamiliale,
        decimal salaireMensuel,
        decimal salaireMensuelConjoint,
        int nombreEnfants) =>
        $"/api/tax/calculate?situationFamiliale={situationFamiliale}&salaireMensuel={salaireMensuel}&salaireMensuelConjoint={salaireMensuelConjoint}&nombreEnfants={nombreEnfants}";
}