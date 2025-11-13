using Desafio_Tecnico.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace Desafio_Tecnico.Core.Models
{
    public class PlanoModel
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Codigo_registro_ans { get; set; }

        [Required]
        public Status Status { get; set; } = Status.ATIVO;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime DataAtualizacao { get; set; }


        public ICollection<BeneficiarioModel> Beneficiarios { get; set; }
        
    }
}
