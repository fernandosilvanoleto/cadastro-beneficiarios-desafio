using Desafio_Tecnico.Application.Dto.Plano;
using Desafio_Tecnico.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

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
        /// Lista todos os beneficiários
        /// </summary>
        [HttpGet("Todos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarPlanos()
        {
            var planos = await _planoInterface.ListarPlanos();

            if (!planos.Status)
                return StatusCode(StatusCodes.Status500InternalServerError, planos);

            return Ok(planos);
        }


        /// <summary>
        /// Retorna um beneficiário pelo ID
        /// </summary>
        [HttpGet("Detalhes/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Detalhe(int id)
        {
            var plano = await _planoInterface.ListarPlanoIndividual(id);

            if (!plano.Status)
            {
                if (plano.Error == "ValidationError")
                    return NotFound(plano);

                return StatusCode(StatusCodes.Status500InternalServerError, plano);
            }

            return Ok(plano);
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
        [HttpDelete("Deletar/{id}")]
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

        /// <summary>
        /// Deleta um plano pelo ID
        /// </summary>
        [HttpPut("Ativar/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtivarPlano(int id)
        {
            var plano = await _planoInterface.AtivarPlano(id);

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
