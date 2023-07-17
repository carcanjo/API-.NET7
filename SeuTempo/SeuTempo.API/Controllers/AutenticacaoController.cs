using Microsoft.AspNetCore.Mvc;
using SeuTempo.API.Utils;
using SeuTempo.Application.InputModel;
using SeuTempo.Application.Interfaces;
using SeuTempo.Application.ViewModel;
using SeuTempo.Core.Exceptions;

namespace SeuTempo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly ILogger<AutenticacaoController> logger;
        private readonly ITokenService tokenService;

        public AutenticacaoController(ILogger<AutenticacaoController> logger, ITokenService tokenService)
        {
            this.logger = logger;
            this.tokenService = tokenService;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseViewModel<>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseViewModel<>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseViewModel<>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseViewModel<>))]
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenInputModel inputModel)
        {
            try
            {
                return Ok(Responses.ApplicationSucessMessage(await tokenService.GetTokenAsync(inputModel)));
            }
            catch (DomainException)
            {
                return Unauthorized(Responses.UnauthorizeErrorMessage());
            }
            catch (Exception ex)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage(ex.Message));
            }
        }
    }
}
