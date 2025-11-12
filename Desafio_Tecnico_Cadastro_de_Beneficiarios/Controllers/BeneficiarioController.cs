using Desafio_Tecnico.Application.Dto.Beneficiario;
using Desafio_Tecnico.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiarioController : ControllerBase
    {
        private readonly IBeneficiarioInterface _beneficiarioInterface;

        public BeneficiarioController(IBeneficiarioInterface beneficiarioInterface)
        {
            _beneficiarioInterface = beneficiarioInterface;
        }

        /// <summary>
        /// Lista todos os beneficiários
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarBeneficiarios()
        {
            var beneficiario = await _beneficiarioInterface.ListarBeneficiarios();

            if (!beneficiario.Status)
                return StatusCode(StatusCodes.Status500InternalServerError, beneficiario);

            return Ok(beneficiario);
        }

        /// <summary>
        /// Retorna um beneficiário pelo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Detalhe(int id)
        {
            var beneficiario = await _beneficiarioInterface.BuscarBeneficiariosPorId(id);

            if (!beneficiario.Status)
            {
                if (beneficiario.Error == "ValidationError")
                    return NotFound(beneficiario);

                return StatusCode(StatusCodes.Status500InternalServerError, beneficiario);
            }

            return Ok(beneficiario);
        }

        /// <summary>
        /// Edita um beneficiário existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarBeneficiario([FromBody] BeneficiarioEdicaoDto dto)
        {
            var beneficiario = await _beneficiarioInterface.EditarBeneficiarios(dto);

            if (!beneficiario.Status)
            {
                if (beneficiario.Error == "ValidationError")
                    return NotFound(beneficiario);

                return StatusCode(StatusCodes.Status500InternalServerError, beneficiario);
            }

            return Ok(beneficiario);
        }

        /// <summary>
        /// Deleta um beneficiário pelo ID
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletarBeneficiario(int id)
        {
            var beneficiario = await _beneficiarioInterface.DeletarBeneficiario(id);

            if (!beneficiario.Status)
            {
                if (beneficiario.Error == "ValidationError")
                    return NotFound(beneficiario);

                return StatusCode(StatusCodes.Status500InternalServerError, beneficiario);
            }

            return Ok(beneficiario);
        }
    }
}
