using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_Tecnico.Application.Dto.Beneficiario
{
    public class BeneficiarioCriacaoDto : IBeneficiarioBase
    {
        [Required(ErrorMessage = " O nome do beneficiário é obrigatório")]
        public string NomeCompleto { get; set; }


        [Required(ErrorMessage = "A data de nascimento é obrigatório")]
        public DateTime DataNascimento { get; set; }


        [Required(ErrorMessage = "O CPF do beneficiário é obrigatório")]
        [RegularExpression(@"(\d{3}\.\d{3}\.\d{3}-\d{2}|\d{11})", ErrorMessage = "CPF deve estar no formato 000.000.000-00 ou 00000000000")]
        public string Cpf { get; set; }


        [Required(ErrorMessage = "O Plano deve ser escolhido.")]
        public int PlanoId { get; set; }
    }
}
