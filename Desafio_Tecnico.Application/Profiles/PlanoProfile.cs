using AutoMapper;
using Desafio_Tecnico.Core.Models;
using Desafio_Tecnico.Application.Dto.Plano;

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
