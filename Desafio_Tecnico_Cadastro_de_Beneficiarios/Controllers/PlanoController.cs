using Desafio_Tecnico.Application.Dto.Plano;
using Desafio_Tecnico.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanoController : ControllerBase
    {
        private readonly IPlanoInterface _planoInterface;

        public PlanoController(IPlanoInterface planoInterface)
        {
            _planoInterface = planoInterface;
        }

        /// <summary>
        /// Criar Plano
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarPlano([FromBody] PlanoCriacaoDto planoCriacaoDto)
        {
            var plano = await _planoInterface.CriarPlano(planoCriacaoDto);

            if (!plano.Status)
            {
                if (plano.Error == "ValidationError")
                    return Conflict(plano);

                return StatusCode(StatusCodes.Status500InternalServerError, plano);
            }

            return CreatedAtAction(nameof(EditarPlano), new { id = plano.Dados.Id }, plano);
        }

        /// <summary>
        /// Edita um plano existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarPlano([FromBody] PlanoEdicaoDto planoEdicaoDto)
        {
            var plano = await _planoInterface.EditarPlano(planoEdicaoDto);

            if (!plano.Status)
            {
                if (plano.Error == "ValidationError")
                    return NotFound(plano);

                return StatusCode(StatusCodes.Status500InternalServerError, plano);
            }

            return Ok(plano);
        }

        /// <summary>
        /// Deleta um plano pelo ID
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarPlano(int id)
        {
            var plano = await _planoInterface.DeletarPlano(id);

            if (!plano.Status)
            {
                if (plano.Error == "ValidationError")
                    return NotFound(plano);

                return StatusCode(StatusCodes.Status500InternalServerError, plano);
            }

            return Ok(plano);
        }
    }
}
