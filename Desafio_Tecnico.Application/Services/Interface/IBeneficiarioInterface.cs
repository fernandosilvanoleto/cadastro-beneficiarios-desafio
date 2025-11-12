using Desafio_Tecnico.Core.Models;
using Desafio_Tecnico.Application.Dto.Beneficiario;

namespace Desafio_Tecnico.Application.Services.Interface
{
    public interface IBeneficiarioInterface
    {
        Task<ResponseModel<List<BeneficiarioModel>>> ListarBeneficiarios();
        Task<ResponseModel<BeneficiarioModel>> BuscarBeneficiariosPorId(int id);
        Task<ResponseModel<BeneficiarioModel>> EditarBeneficiarios(BeneficiarioEdicaoDto beneficiarioEdicaoDto);
        Task<ResponseModel<BeneficiarioModel>> DeletarBeneficiario(int id);
    }
}
