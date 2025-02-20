using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tax.Simulator.Api
{
    /// <summary>
    /// Controller for the simulator
    /// </summary>
    [ApiController]
    public class SimulateurControleur : ControllerBase
    {
        /// <summary>
        /// Calculate the taxes
        /// </summary>
        /// <param name="situationFamiliale">family situation</param>
        /// <param name="salaireMensuel">mensual income</param>
        /// <param name="salaireMensuelConjoint">partner's mensual income</param>
        /// <param name="nombreEnfants">number of childrens</param>
        /// <returns>taxes</returns> 
        [HttpGet]
        [Route("api/tax/calculate")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult CalculerImpots(
            [BindRequired] string situationFamiliale, 
            [BindRequired] decimal salaireMensuel, 
            [BindRequired] decimal salaireMensuelConjoint, 
            [BindRequired] int nombreEnfants)
        {
            try
            {
                return Ok(
                    Simulateur.CalculerImpotsAnnuel(
                        situationFamiliale,
                        salaireMensuel,
                        salaireMensuelConjoint,
                        nombreEnfants)
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
