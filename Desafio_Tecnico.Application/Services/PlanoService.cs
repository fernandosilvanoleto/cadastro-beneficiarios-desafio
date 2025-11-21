using AutoMapper;
using Desafio_Tecnico.Core.Models;
using Microsoft.EntityFrameworkCore;
using Desafio_Tecnico.Application.Dto.Plano;
using Desafio_Tecnico.Application.Services.Interface;
using Desafio_Tecnico.Infraestructure.Data;

namespace Desafio_Tecnico.Application.Services
{
    public class PlanoService : IPlanoInterface
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PlanoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseModel<PlanoModel>> CriarPlano(PlanoCriacaoDto planoCriacaoDto)
        {
            ResponseModel<PlanoModel> response = new ResponseModel<PlanoModel>();

            try
            {
                if (PlanoExiste(planoCriacaoDto))
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Plano já criado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "não encontrado"
                    });

                    return response;
                }

                PlanoModel plano = _mapper.Map<PlanoModel>(planoCriacaoDto);

                _context.Add(plano);
                await _context.SaveChangesAsync();

                var planoComBeneficiarios = await _context.Planos.Include(p => p.Beneficiarios).FirstOrDefaultAsync(p => p.Id == plano.Id);
                response.Dados = planoComBeneficiarios;
                response.Mensagem = "Plano criado com sucesso";
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

        public async Task<ResponseModel<PlanoModel>> DeletarPlano(int id)
        {
            ResponseModel<PlanoModel> response = new ResponseModel<PlanoModel>();

            try
            {
                var plano = await _context.Planos.FindAsync(id);

                if (plano == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Plano não localizado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "não encontrado"
                    });
                    return response;

                }

                response.Dados = plano;
                response.Mensagem = "Plano removido com sucesso";


                // Exclusão Lógica - Preservar histórico interno
                plano.Is_deleted = true;
                plano.DataAtualizacao = DateTime.Now;

                _context.Planos.Update(plano);
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

        public async Task<ResponseModel<PlanoModel>> AtivarPlano(int id)
        {
            ResponseModel<PlanoModel> response = new ResponseModel<PlanoModel>();

            try
            {
                var plano = await _context.Planos.FindAsync(id);

                if (plano == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Plano não localizado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "não encontrado"
                    });
                    return response;

                }

                response.Dados = plano;
                response.Mensagem = "Plano ativado com sucesso";


                // Ativação Lógica - Preservar histórico interno
                plano.Is_deleted = false;
                plano.DataAtualizacao = DateTime.Now;

                _context.Planos.Update(plano);
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

        public async Task<ResponseModel<PlanoModel>> EditarPlano(PlanoEdicaoDto planoEdicaoDto)
        {
            ResponseModel<PlanoModel> response = new ResponseModel<PlanoModel>();

            try
            {
                var PlanoBanco = _context.Planos.Find(planoEdicaoDto.Id);

                if (PlanoBanco == null)
                {
                    response.Status = false;
                    response.Mensagem = "Plano não localizado";
                    response.Error = "ValidationError";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "não encontrado"
                    });
                    return response;
                }

                if (planoEdicaoDto.Nome.Trim().ToLower() != PlanoBanco.Nome.Trim().ToLower() && PlanoExiste(planoEdicaoDto))
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Plano já criado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "não encontrado"
                    });

                    return response;
                }

                PlanoBanco.Nome = planoEdicaoDto.Nome;
                PlanoBanco.Codigo_registro_ans = planoEdicaoDto.Codigo_registro_ans;

                _context.Planos.Update(PlanoBanco);
                await _context.SaveChangesAsync();

                var planoAtualizado = await _context.Planos.Include(p => p.Beneficiarios).FirstOrDefaultAsync(p => p.Id == PlanoBanco.Id);
                response.Dados = planoAtualizado;
                response.Mensagem = "Plano editado com sucesso";
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

        public async Task<ResponseModel<PlanoModel>> ListarPlanoIndividual(int planoId)
        {
            ResponseModel<PlanoModel> response = new ResponseModel<PlanoModel>();

            try
            {
                var plano = await _context.Planos.FindAsync(planoId);

                if (plano == null)
                {
                    response.Status = false;
                    response.Error = "ValidationError";
                    response.Mensagem = "Plano não localizado";
                    response.Details.Add(new ValidacaoModel
                    {
                        Field = "id",
                        Rule = "not_found"
                    });

                    return response;
                }

                response.Dados = plano;
                response.Mensagem = "Plano localizado com sucesso";
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

        public async Task<ResponseModel<List<PlanoModel>>> ListarPlanos()
        {
            ResponseModel<List<PlanoModel>> response = new ResponseModel<List<PlanoModel>>();

            try
            {
                var planos = await _context.Planos.Where(p => p.Is_deleted == false).ToListAsync();

                response.Dados = planos;
                response.Mensagem = "Beneficiários listados com sucesso";
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

        public bool PlanoExiste(IPlanoBase planoBase)
        {
            string nome = planoBase.Nome.Trim().ToLower();
            string codigo_Registro = planoBase.Codigo_registro_ans.Trim().ToLower();

            // converter valores para minúsculos e remover os espaços da direita e esquerda -- 15/11/2025
            return _context.Planos.Any(item => (item.Nome.ToLower().Trim() == nome
                    || item.Codigo_registro_ans.ToLower().Trim() == codigo_Registro));
        }
    }
}
