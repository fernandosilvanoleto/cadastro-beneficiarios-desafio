using Desafio_Tecnico.Core.Enum;

namespace Desafio_Tecnico.Core.Models
{
    public class BeneficiarioModel
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public Status Status { get; set; } = Status.ATIVO;

        public int PlanoId { get; set; }

        public PlanoModel Plano { get; set; }
        public int MyProperty { get; set; }
    }

}
