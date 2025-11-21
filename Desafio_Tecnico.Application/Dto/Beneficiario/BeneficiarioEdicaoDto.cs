using Desafio_Tecnico.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace Desafio_Tecnico.Application.Dto.Beneficiario
{
    public class BeneficiarioEdicaoDto : IBeneficiarioBase
    {
        public int Id { get; set; }
        [Required]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [RegularExpression(@"(\d{3}\.\d{3}\.\d{3}-\d{2}|\d{11})", ErrorMessage = "CPF deve estar no formato 000.000.000-00 ou 00000000000")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }
        public Status Status { get; set; }

        [Required(ErrorMessage = "O Plano deve ser escolhido.")]
        public int PlanoId { get; set; }
    }
}
