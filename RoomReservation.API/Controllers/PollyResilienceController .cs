using Microsoft.AspNetCore.Mvc;
using RoomReservation.Application.Interfaces.Services;

namespace RoomReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollyResilienceController : ControllerBase
    {
        private readonly IExternalSimulatorService _simulatorService;
        private readonly ILogger<PollyResilienceController> _logger;

        public PollyResilienceController(
            IExternalSimulatorService simulatorService,
            ILogger<PollyResilienceController> logger)
        {
            _simulatorService = simulatorService;
            _logger = logger;
        }

        /// <summary>
        /// Simula uma chamada externa com falha para testar as políticas de resiliência com Polly.
        /// </summary>
        /// <returns>Mensagem de sucesso ou erro simulado.</returns>
        [HttpGet("simulate-failure")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> SimulateExternalCall()
        {
            try
            {
                var result = await _simulatorService.GetSimulatedResponseAsync();
                return Ok(new { success = true, message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro simulado ao chamar serviço externo com Polly");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new
                {
                    success = false,
                    error = "Falha ao chamar o serviço externo simulado.",
                    details = ex.Message
                });
            }
        }
    }
}
