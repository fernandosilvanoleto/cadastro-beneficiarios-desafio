using Desafio_Tecnico.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace Desafio_Tecnico.Core.Models
{
    public class BeneficiarioModel
    {
        public int Id { get; set; }

        [Required]
        public int PlanoId { get; set; }
        public PlanoModel Plano { get; set; }

        [Required]
        public string NomeCompleto { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve conter exatamente 11 caracteres.")]
        public string Cpf { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }
        
        public Status Status { get; set; } = Status.ATIVO;        

        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime DataAtualizacao { get; set; }
        public bool Is_deleted { get; set; } = false;

    }

}
