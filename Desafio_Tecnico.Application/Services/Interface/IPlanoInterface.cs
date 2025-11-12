using Desafio_Tecnico.Core.Models;
using Desafio_Tecnico.Application.Dto.Plano;

namespace Desafio_Tecnico.Application.Services.Interface
{
    public interface IPlanoInterface
    {
        Task<ResponseModel<PlanoModel>> CriarPlano(PlanoCriacaoDto planoCriacaoDto);
        Task<ResponseModel<PlanoModel>> EditarPlano(PlanoEdicaoDto planoEdicaoDto);
        Task<ResponseModel<PlanoModel>> DeletarPlano(int id);
    }
}
