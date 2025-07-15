using Microsoft.AspNetCore.Mvc;
using RoomReservation.Application.Interfaces.Services;

namespace RoomReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IExternalSimulatorService _simulatorService;

        public TestController(IExternalSimulatorService simulatorService)
        {
            _simulatorService = simulatorService;
        }

        /// <summary>
        /// Testa uma chamada externa com políticas de resiliência (simulação).
        /// </summary>
        [HttpGet("failure")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SimulateExternalCall()
        {
            try
            {
                var result = await _simulatorService.GetSimulatedResponseAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro simulado: {ex.Message}");
            }
        }
    }
}
