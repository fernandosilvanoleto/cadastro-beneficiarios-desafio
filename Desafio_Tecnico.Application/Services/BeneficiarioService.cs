using AutoMapper;
using Desafio_Tecnico.Application.Dto.Beneficiario;
using Desafio_Tecnico.Application.Dto.Plano;
using Desafio_Tecnico.Application.Services.Interface;
using Desafio_Tecnico.Core.Enum;
using Desafio_Tecnico.Core.Models;
using Desafio_Tecnico.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Text.RegularExpressions;

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

        public async Task<ResponseModel<BeneficiarioModel>> AtivarBeneficiario(int id)
        {
            ResponseModel<BeneficiarioModel> response = new ResponseModel<BeneficiarioModel>();

            try
            {
                var beneficiario = await _context.Beneficiarios.FindAsync(id);

                if (beneficiario == null)
                {
                    response = MensagemErroPadrao(response, "ValidationError", "Beneficiario não localizado");

                    return response;

                }

                // Ativação Lógica - Preservar histórico interno
                beneficiario.Is_deleted = false;
                beneficiario.DataAtualizacao = DateTime.Now;

                _context.Beneficiarios.Update(beneficiario);
                await _context.SaveChangesAsync();

                response.Dados = beneficiario;
                response.Mensagem = "Beneficiario ativado com sucesso";

                return response;
            }
            catch (Exception ex)
            {
                response = MensagemErroPadrao(response, "ValidationError", ex.Message);

                return response;
            }
        }

        public async Task<ResponseModel<BeneficiarioModel>> BuscarBeneficiariosPorId(int id)
        {
            ResponseModel<BeneficiarioModel> response = new ResponseModel<BeneficiarioModel>();

            try
            {
                var beneficiario = await _context.Beneficiarios.FindAsync(id);

                if(beneficiario == null)
                {
                    response = MensagemErroPadrao(response, "ValidationError","Beneficiario não localizado");

                    return response;
                }
                response.Dados = beneficiario;
                response.Mensagem = "Beneficiário localizado com sucesso";
                return response;

            }
            catch(Exception ex)
            {
                response = MensagemErroPadrao(response, "ValidationError", ex.Message);

                return response;
            }
        }

        public async Task<ResponseModel<BeneficiarioModel>> CriarBeneficiario(BeneficiarioCriacaoDto beneficiarioCriacaoDto)
        {
            ResponseModel<BeneficiarioModel> response = new ResponseModel<BeneficiarioModel>();

            try
            {
                beneficiarioCriacaoDto.Cpf = LimparCPF(beneficiarioCriacaoDto.Cpf);

                if (!VerificarValidarCPF(beneficiarioCriacaoDto.Cpf))
                {
                    response = MensagemErroPadrao(response, "ValidationError", "CPF de Novo Beneficiário Inválido.");

                    return response;
                }

                if (BeneficiarioExiste(beneficiarioCriacaoDto))
                {
                    response = MensagemErroPadrao(response, "ValidationError", "Beneficiário com mesmo CPF já criado anteriormente");

                    return response;
                }

                if (!PlanoExisteBanco(beneficiarioCriacaoDto.PlanoId))
                {
                    response = MensagemErroPadrao(response, "ValidationPlanoError", "Status 404 - Plano inexistente. Por favor, revise os dados novamente.");

                    return response;
                }

                BeneficiarioModel beneficiario = _mapper.Map<BeneficiarioModel>(beneficiarioCriacaoDto);

                _context.Add(beneficiario);
                await _context.SaveChangesAsync();

                var beneficiarios = await _context.Beneficiarios.FirstOrDefaultAsync(p => p.Id == beneficiario.Id);
                response.Dados = beneficiarios;
                response.Mensagem = "Beneficiário criado com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response = MensagemErroPadrao(response, "ValidationError", ex.Message);
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
                    response = MensagemErroPadrao(response, "ValidationError", "Beneficiario não localizado");

                    return response;
                }

                // Exclusão Lógica - Preservar histórico interno
                beneficiario.Is_deleted = true;
                beneficiario.DataAtualizacao = DateTime.Now;

                _context.Beneficiarios.Update(beneficiario);
                await _context.SaveChangesAsync();

                response.Dados = beneficiario;
                response.Mensagem = "Beneficiário removido com sucesso";

                return response;

            }
            catch (Exception ex)
            {
                response = MensagemErroPadrao(response, "ValidationError", ex.Message);
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
                    response = MensagemErroPadrao(response, "ValidationError", "CPF inválido.");

                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "cpf",
                        Rule = "invalid"
                    });

                    return response;
                }

                if (beneficiarioEdicaoDto.Cpf.Trim().ToLower() != beneficiarioBanco.Cpf.Trim().ToLower() && BeneficiarioExiste(beneficiarioEdicaoDto))
                {
                    response = MensagemErroPadrao(response, "ValidationError", "Beneficiário já criado anteriormente com esse mesmo CPF. Por favor, revise o seu CPF.");

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
                response = MensagemErroPadrao(response, "ValidationError", ex.Message);
                return response;
            }
        }

        public async Task<ResponseModel<List<BeneficiarioModel>>> ListarBeneficiarios()
        {
            ResponseModel<List<BeneficiarioModel>> response = new ResponseModel<List<BeneficiarioModel>>();

            try
            {
                var beneficiarios = await _context.Beneficiarios.ToListAsync();

                if (beneficiarios == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Beneficiarios não localizado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "not_found"
                    });

                    return response;
                }

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

        public bool BeneficiarioExiste(IBeneficiarioBase beneficiarioBase)
        {
            string cpf = beneficiarioBase.Cpf.Trim().ToLower();

            // converter valores para minúsculos e remover os espaços da direita e esquerda -- 15/11/2025
            return _context.Beneficiarios.Any(item => item.Cpf.ToLower().Trim() == cpf);
        }

        public bool PlanoExisteBanco(int planoId)
        {
            if (planoId <= 0)
                return false;

            return _context.Planos.Any(item => item.Id == planoId);
        }

        public string LimparCPF(string cpfOriginal)
        {
            return Regex.Replace(cpfOriginal, "[^0-9]", "");
        }

        public bool VerificarValidarCPF(string cpfLimpo)
        {
            if (string.IsNullOrWhiteSpace(cpfLimpo))
                return false;

            var regex = new Regex(@"(^\d{3}\.\d{3}\.\d{3}-\d{2}$)|(^\d{11}$)");  // só aceita CPF formatado (000.000.000-00) ou só números (00000000000)

            return regex.IsMatch(cpfLimpo); // retorna true se o CPF combinar com o padrão
        } 

        public ResponseModel<BeneficiarioModel> MensagemErroPadrao(ResponseModel<BeneficiarioModel> response, string mensagemError, string mensagem)
        {
            response.Status = false;
            response.Error = mensagemError;
            response.Mensagem = mensagem;
            response.Details.Add(new ValidacaoModel
            {
                Field = "id",
                Rule = "não encontrado"
            });

            return response;
        }

        public async Task<ResponseModel<List<BeneficiarioModel>>> ListarBeneficiariosAtivos()
        {
            ResponseModel<List<BeneficiarioModel>> response = new ResponseModel<List<BeneficiarioModel>>();

            try
            {
                var beneficiarios = await _context.Beneficiarios.Where(b => b.Status == Status.ATIVO).ToListAsync();

                if (beneficiarios == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Beneficiarios não localizados";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "not_found"
                    });

                    return response;
                }

                response.Dados = beneficiarios;
                response.Mensagem = "Beneficiários ativos listados com sucesso";
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
    }
}
