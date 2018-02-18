using AutoMapper;
using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Models.ViewData;

namespace Lndr.MdsOnline
{
    public static class MapperConfig
    {
        public static void ConfigMapper (IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SolicitacaoRoteiroTesteUnitarioDomain, SolicitacaoRoteiroTesteUnitarioViewData>();
            cfg.CreateMap<SolicitacaoRoteiroTesteUnitarioViewData, SolicitacaoRoteiroTesteUnitarioDomain>();
        }
    }
}