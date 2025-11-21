using Desafio_Tecnico.Core.Models;
using Desafio_Tecnico.Application.Dto.Plano;

namespace Desafio_Tecnico.Application.Services.Interface
{
    public interface IPlanoInterface
    {
        Task<ResponseModel<List<PlanoModel>>> ListarPlanos(bool planosAtivos);
        Task<ResponseModel<PlanoModel>> ListarPlanoIndividual(int planoId);
        Task<ResponseModel<PlanoModel>> CriarPlano(PlanoCriacaoDto planoCriacaoDto);
        Task<ResponseModel<PlanoModel>> EditarPlano(PlanoEdicaoDto planoEdicaoDto);
        Task<ResponseModel<PlanoModel>> DeletarPlano(int id);
        Task<ResponseModel<PlanoModel>> AtivarPlano(int id);
    }
}
