using Microsoft.AspNetCore.Mvc;
using SeuTempo.API.Utils;
using SeuTempo.Application.InputModel;
using SeuTempo.Application.ViewModel;
using SeuTempo.Core.Exceptions;

namespace SeuTempo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseViewModel<>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseViewModel<>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseViewModel<>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseViewModel<>))]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] TokenInputModel inputModel)
        {
            try
            {
                return Ok(Responses.ApplicationSucessMessage(inputModel));
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
