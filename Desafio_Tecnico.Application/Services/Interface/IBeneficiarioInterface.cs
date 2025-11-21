using Desafio_Tecnico.Application.Dto.Beneficiario;
using Desafio_Tecnico.Application.Dto.Plano;
using Desafio_Tecnico.Core.Models;

namespace Desafio_Tecnico.Application.Services.Interface
{
    public interface IBeneficiarioInterface
    {
        Task<ResponseModel<List<BeneficiarioModel>>> ListarBeneficiarios();
        Task<ResponseModel<List<BeneficiarioModel>>> ListarBeneficiariosAtivos();
        Task<ResponseModel<BeneficiarioModel>> BuscarBeneficiariosPorId(int id);
        Task<ResponseModel<BeneficiarioModel>> CriarBeneficiario(BeneficiarioCriacaoDto planoCriacaoDto);
        Task<ResponseModel<BeneficiarioModel>> EditarBeneficiarios(BeneficiarioEdicaoDto beneficiarioEdicaoDto);
        Task<ResponseModel<BeneficiarioModel>> DeletarBeneficiario(int id);
        Task<ResponseModel<BeneficiarioModel>> AtivarBeneficiario(int id);
    }
}
