using AutoMapper;
using Desafio_Tecnico.Application.Dto.Beneficiario;
using Desafio_Tecnico.Application.Services;
using Desafio_Tecnico.Core.Enum;
using Desafio_Tecnico.Core.Models;
using Desafio_Tecnico.Infraestructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sprache;
using System;


namespace Desafio.Tests
{
    public class BeneficiarioTests : IDisposable 
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BeneficiarioTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BeneficiarioEdicaoDto, BeneficiarioModel>();
            });
            _mapper = config.CreateMapper();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private BeneficiarioService CriarService() => new BeneficiarioService(_context, _mapper);

        private async Task<BeneficiarioService> CriarServiceComBeneficiarioExistente(string cpf)
        {
            _context.Beneficiarios.Add(new BeneficiarioModel
            {
                NomeCompleto = "Beneficiario Existente",
                Cpf = cpf,
                PlanoId = 1,
                DataNascimento = new DateTime(01/01/2000),
                Status = Status.ATIVO
            });
            await _context.SaveChangesAsync();

            return CriarService();
        }

        private async Task<BeneficiarioService> CriarServiceComListaDeBeneficiarios()
        {
            _context.Beneficiarios.AddRange(new List<BeneficiarioModel>
    {
            new BeneficiarioModel { NomeCompleto = "Lucas de Jesus Marinho", Cpf = "11111111111", PlanoId = 1, Status = Status.ATIVO, DataNascimento= new DateTime(01/01/2000)},
            new BeneficiarioModel { NomeCompleto = "Ana Alice de Jesus da Silva", Cpf = "22222222222", PlanoId = 2, Status = Status.INATIVO, DataNascimento= new DateTime(02/02/2000) },
            new BeneficiarioModel { NomeCompleto = "Maria de Jesus da Silva", Cpf = "33333333333", PlanoId = 1, Status = Status.ATIVO, DataNascimento= new DateTime (03/03/2000) }
            });
            await _context.SaveChangesAsync();

            return CriarService();
        }

        private async Task<BeneficiarioService> CriarServiceComBeneficiarioAtivo()
        {
            _context.Beneficiarios.Add(new BeneficiarioModel
            {
                NomeCompleto = "Lucas Ativo",
                Cpf = "55555555555",
                PlanoId = 1,
                DataNascimento = new DateTime(04/04/2000),
                Status = Status.ATIVO
            });
            await _context.SaveChangesAsync();

            return CriarService();
        }

        private async Task<BeneficiarioService> NovoBeneficiarioComCPFInvalido()
        {
            _context.Beneficiarios.Add(new BeneficiarioModel
            {
                NomeCompleto = "Fernando Silva Noleto",
                Cpf = "055556721",
                PlanoId = 1,
                DataNascimento = new DateTime(09 / 06 / 1995),
                Status = Status.ATIVO
            });
            await _context.SaveChangesAsync();

            return CriarService();
        }

        [Fact]
        public async Task AtualizarStatus_Beneficiario_DeveAlterarParaInativo()
        {
            var service = await CriarServiceComBeneficiarioAtivo();
            var beneficiario = _context.Beneficiarios.First();

            var dtoEdicao = new BeneficiarioEdicaoDto
            {
                Id = beneficiario.Id,
                NomeCompleto = beneficiario.NomeCompleto,
                Cpf = beneficiario.Cpf,
                DataNascimento = beneficiario.DataNascimento,
                Status = Status.INATIVO
            };

            var result = await service.EditarBeneficiarios(dtoEdicao);
            result.Status.Should().BeTrue();
            result.Mensagem.Should().Be("Beneficiário editado com sucesso");
            result.Dados.Status.Should().Be(Status.INATIVO);
        }


        [Fact]
        public async Task ListarBeneficiarios_ComFiltros_DeveRetornarSomenteCorretos()
        {
            var service = await CriarServiceComListaDeBeneficiarios();
            bool retornarSomenteBeneficiariosAtivos = true;

            var todos = (await service.ListarBeneficiarios(retornarSomenteBeneficiariosAtivos)).Dados;
            var filtrados = todos.Where(b => b.Status == Status.ATIVO && b.PlanoId == 1).ToList();


            filtrados.Should().OnlyContain(b => b.Status == Status.ATIVO && b.PlanoId == 1);

            var result = await service.ListarBeneficiarios(retornarSomenteBeneficiariosAtivos);

            result.Status.Should().BeTrue();
            result.Dados.Should().NotBeNull();                  
            result.Dados.Should().HaveCount(2);                
            result.Dados.Should().OnlyContain(b => b.Status == Status.ATIVO); // só ATIVO
        }

        [Fact]
        public async Task CriarBeneficiario_ComCPFExistente_BancoDados()
        {

           // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "BeneficiarioTesteCPFExiste")
                .Options;

            string cpfExistente = "05555672144";

            using (var context = new AppDbContext(options))
            {
                // cria um beneficiário existente no banco
                context.Beneficiarios.Add(new BeneficiarioModel
                {
                    NomeCompleto = "João da Silva",
                    Cpf = cpfExistente,
                    DataNascimento = new DateTime(1990, 01, 10),
                    PlanoId = 1
                });

                context.SaveChanges();
            }

            ResponseModel<BeneficiarioModel> resultado;

            // Act
            using (var context = new AppDbContext(options))
            {
                var service = new BeneficiarioService(context, _mapper);

                var dto = new BeneficiarioCriacaoDto
                {
                    NomeCompleto = "Fernando Silva Noleto",
                    Cpf = cpfExistente, // CPF duplicado
                    DataNascimento = new DateTime(1995, 06, 09),
                    PlanoId = 1
                };

                resultado = await service.CriarBeneficiario(dto);
            }

            // Assert
            Assert.Equal("Beneficiário com mesmo CPF já criado anteriormente", resultado.Mensagem);
            Assert.Null(resultado.Dados);

        }

        [Fact]
        public async Task ValidarCPFAoCriarBeneficiario_BancoDados()
        {
            // ARRANGE
            var service = CriarService();

            var dtoCriacao = new BeneficiarioCriacaoDto
            {
                NomeCompleto = "Novo Usuário",
                Cpf = "055556721",                     // CPF INVÁLIDO
                DataNascimento = new DateTime(1990, 01, 01),
                PlanoId = 1
            };

            // ACT
            var result = await service.CriarBeneficiario(dtoCriacao);

            // ASSERT
            result.Status.Should().BeFalse();
            result.Mensagem.Should().Be("CPF de Novo Beneficiário Inválido.");
            result.Dados.Should().BeNull();
        }

        [Fact]
        public async Task VinculacaoPlanoInexistente_Cadastro()
        {
            // ARRANGE

            _context.Planos.AddRange(new List<PlanoModel>
            {
                new PlanoModel { Id = 1, Nome = "Plano Bronze", Codigo_registro_ans = "ANS001", Status = Status.ATIVO },
                new PlanoModel { Id = 2, Nome = "Plano Prata",  Codigo_registro_ans = "ANS002", Status = Status.ATIVO },
                new PlanoModel { Id = 3, Nome = "Plano Ouro",   Codigo_registro_ans = "ANS003", Status = Status.ATIVO }
            });

            await _context.SaveChangesAsync();

            var service = new BeneficiarioService(_context, _mapper);

            var dtoCriacao = new BeneficiarioCriacaoDto
            {
                NomeCompleto = "Novo Usuário",
                Cpf = "05555672144",                     // CPF INVÁLIDO
                DataNascimento = new DateTime(1990, 01, 01),
                PlanoId = 10
            };

            // ACT
            var result = await service.CriarBeneficiario(dtoCriacao);

            // ASSERT
            result.Status.Should().BeFalse();
            result.Mensagem.Should().Be("Status 404 - Plano inexistente. Por favor, revise os dados novamente.");
            result.Error.Should().Be("ValidationPlanoError");
            result.Dados.Should().BeNull();
        }

        [Fact]
        public async Task BuscarBeneficiario_Inexistente()
        {
            // ARRANGE
            var service = await CriarServiceComListaDeBeneficiarios();
            int idBeneficiario = 5;

            // ACT
            var result = await service.BuscarBeneficiariosPorId(idBeneficiario);

            // ASSERT
            result.Status.Should().BeFalse();
            result.Mensagem.Should().Be("Beneficiario não localizado");
            result.Error.Should().Be("ValidationError");
        }
    }
}
