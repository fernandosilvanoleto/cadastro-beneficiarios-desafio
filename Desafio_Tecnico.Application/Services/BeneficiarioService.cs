using AutoMapper;
using Desafio_Tecnico.Core.Models;
using Microsoft.EntityFrameworkCore;
using Desafio_Tecnico.Application.Dto.Beneficiario;
using Desafio_Tecnico.Application.Services.Interface;
using Desafio_Tecnico.Infraestructure.Data;

namespace Desafio_Tecnico.Application.Services
{
    public class BeneficiarioService : IBeneficiarioInterface
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BeneficiarioService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseModel<BeneficiarioModel>> BuscarBeneficiariosPorId(int id)
        {
            ResponseModel<BeneficiarioModel> response = new ResponseModel<BeneficiarioModel>();

            try
            {
                var beneficiario = await _context.Beneficiarios.FindAsync(id);

                if(beneficiario == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Beneficiario não localizado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "not_found"
                    });

                    return response;
                }
                response.Dados = beneficiario;
                response.Mensagem = "Beneficiário localizado com sucesso";
                return response;

            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Error = "ServerError";
                response.Mensagem = ex.Message;
                return response;
            }
        }

        public async Task<ResponseModel<BeneficiarioModel>> DeletarBeneficiario(int id)
        {
            ResponseModel<BeneficiarioModel> response = new ResponseModel<BeneficiarioModel>();

            try
            {
                var beneficiario = await _context.Beneficiarios.FindAsync(id);

                if(beneficiario == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Beneficiario não localizado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "não encontrado"
                    });
                    return response;
                }

                response.Dados = beneficiario;
                response.Mensagem = "Beneficiário removido com sucesso";

                _context.Beneficiarios.Remove(beneficiario);
                await _context.SaveChangesAsync();

                return response;

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Error = "ServerError";
                response.Mensagem = ex.Message;
                return response;
            }
        }

        public async Task<ResponseModel<BeneficiarioModel>> EditarBeneficiarios(BeneficiarioEdicaoDto beneficiarioEdicaoDto)
        {
            ResponseModel<BeneficiarioModel> response = new ResponseModel<BeneficiarioModel>();

            try
            {
                var beneficiarioBanco = await _context.Beneficiarios.FindAsync(beneficiarioEdicaoDto.Id);

                if (beneficiarioBanco == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "CPF inválido";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "cpf",
                        Rule = "invalid"
                    });

                    return response;
                }
                beneficiarioBanco.NomeCompleto = beneficiarioEdicaoDto.NomeCompleto;
                beneficiarioBanco.Cpf = beneficiarioEdicaoDto.Cpf;
                beneficiarioBanco.DataNascimento = beneficiarioEdicaoDto.DataNascimento;
                beneficiarioBanco.Status = beneficiarioEdicaoDto.Status;

                _context.Update(beneficiarioBanco);
                await _context.SaveChangesAsync();
                
                response.Dados = beneficiarioBanco;
                response.Mensagem = "Beneficiário editado com sucesso";
                return response;
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Error = "NotFound";
                response.Mensagem = ex.Message;
                return response;
            }
        }

        public async Task<ResponseModel<List<BeneficiarioModel>>> ListarBeneficiarios()
        {
            ResponseModel<List<BeneficiarioModel>> response = new ResponseModel<List<BeneficiarioModel>>();

            try
            {
                var beneficiarios = await _context.Beneficiarios.ToListAsync();

                response.Dados = beneficiarios;
                response.Mensagem = "Beneficiários listados com sucesso";
                return response;


            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Error = "ServerError";
                response.Mensagem = ex.Message;
                return response;
            }
        }
    }
}
