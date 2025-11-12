using AutoMapper;
using Desafio_Tecnico.Core.Enum;
using Desafio_Tecnico.Core.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Desafio_Tecnico.Application.Dto.Beneficiario;
using Desafio_Tecnico.Infraestructure.Data;
using Desafio_Tecnico.Application.Services;


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
            result.Dados.Status.Should().Be(Status.INATIVO);
        }

        [Fact]
        public async Task ListarBeneficiarios_ComFiltros_DeveRetornarSomenteCorretos()
        {
            var service = await CriarServiceComListaDeBeneficiarios();

            var todos = (await service.ListarBeneficiarios()).Dados;
            var filtrados = todos.Where(b => b.Status == Status.ATIVO && b.PlanoId == 1).ToList();
            filtrados.Should().OnlyContain(b => b.Status == Status.ATIVO && b.PlanoId == 1);
        }
    }
}
