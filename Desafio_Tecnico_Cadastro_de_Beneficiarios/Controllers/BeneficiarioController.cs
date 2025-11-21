using Desafio_Tecnico.Application.Dto.Beneficiario;
using Desafio_Tecnico.Application.Dto.Plano;
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
            bool retornarSomenteBeneficiariosAtivos = false;
            var beneficiario = await _beneficiarioInterface.ListarBeneficiarios(retornarSomenteBeneficiariosAtivos);

            if (!beneficiario.Status)
                return StatusCode(StatusCodes.Status500InternalServerError, beneficiario);

            return Ok(beneficiario);
        }

        /// <summary>
        /// Lista todos os beneficiários ativos
        /// </summary>
        [HttpGet("ativos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarBeneficiariosAtivos()
        {
            bool retornarSomenteBeneficiariosAtivos = true;
            var beneficiario = await _beneficiarioInterface.ListarBeneficiarios(retornarSomenteBeneficiariosAtivos);

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
        /// Criar Plano
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarBeneficiario([FromBody] BeneficiarioCriacaoDto beneficiarioCriacaoDto)
        {
            var beneficiario = await _beneficiarioInterface.CriarBeneficiario(beneficiarioCriacaoDto);

            if (!beneficiario.Status)
            {
                if (beneficiario.Error == "ValidationError")
                {
                    return Conflict(beneficiario);
                    
                }

                if (beneficiario.Error == "ValidationPlanoError")
                {
                    return StatusCode(StatusCodes.Status404NotFound, beneficiario);

                }

                return StatusCode(StatusCodes.Status500InternalServerError, beneficiario);
            }

            return CreatedAtAction(nameof(EditarBeneficiario), new { id = beneficiario.Dados.Id }, beneficiario);
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
        [HttpDelete("RemoverBeneficiario/{id}")]
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

        /// <summary>
        /// Ativar um beneficiário pelo ID
        /// </summary>
        [HttpPut("AtivarBeneficiario/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtivarBeneficiario(int id)
        {
            var beneficiario = await _beneficiarioInterface.AtivarBeneficiario(id);

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
