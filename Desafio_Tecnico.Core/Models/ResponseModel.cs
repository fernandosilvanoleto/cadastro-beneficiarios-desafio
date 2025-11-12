namespace Desafio_Tecnico.Core.Models
{
    public class ResponseModel<T>
    {
        public T Dados { get; set; }
        public string Error { get; set; }
        public string Mensagem { get; set; }
        public bool Status { get; set; } = true;
        public List<ValidacaoModel> Details { get; set; } = new List<ValidacaoModel>();
    }
}
