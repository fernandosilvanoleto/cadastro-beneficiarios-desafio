using AutoMapper;
using Desafio_Tecnico.Core.Models;
using Desafio_Tecnico.Application.Dto.Plano;
using Desafio_Tecnico.Application.Dto.Beneficiario;

namespace Desafio_Tecnico.Application.Profiles
{
    public class PlanoProfile : Profile
    {
        public PlanoProfile()
        {
            CreateMap<PlanoCriacaoDto, PlanoModel>();
        }
    }
}
