using AutoMapper;
using Desafio_Tecnico.Application.Dto.Beneficiario;
using Desafio_Tecnico.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio_Tecnico.Application.Profiles
{
    public class BeneficiarioProfile : Profile
    {
        public BeneficiarioProfile()
        {
            CreateMap<BeneficiarioCriacaoDto, BeneficiarioModel>();
        }        
    }
}
