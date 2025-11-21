using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_Tecnico.Application.Dto.Plano
{
    public interface IPlanoBase
    {
        // Classe criada para usar a mesma função de "PlanoExiste" em PlanoService.cs - Isso permite não criar duas vezes o método "PlanoExiste"
        // As classes implementadores "PlanoCriacaoDto" e "PlanoEdicaoDto" só bastam adicionar o : IPlanoBase. Isso não compromete a interface com o usuário/cliente
        string Nome { get; set; }
        string Codigo_registro_ans { get; set; }
    }
}
