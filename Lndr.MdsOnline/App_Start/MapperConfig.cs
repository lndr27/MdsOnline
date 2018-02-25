using AutoMapper;
using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Models.DTO;
using Lndr.MdsOnline.Models.ViewData;

namespace Lndr.MdsOnline
{
    public static class MapperConfig
    {
        public static void ConfigMapper (IMapperConfigurationExpression cfg)
        {
            #region RTU +
            cfg.CreateMap<SolicitacaoRoteiroTesteUnitarioDomain, SolicitacaoRoteiroTesteUnitarioViewData>();
            cfg.CreateMap<SolicitacaoRoteiroTesteUnitarioViewData, SolicitacaoRoteiroTesteUnitarioDomain>();
            #endregion

            #region RTF +
            cfg.CreateMap<SolicitacaoRoteiroTesteFuncionalDomain, SolicitacaoRoteiroTesteFuncionalDTO>();
            cfg.CreateMap<SolicitacaoRoteiroTesteFuncionalDTO, SolicitacaoRoteiroTesteFuncionalDomain>();
            cfg.CreateMap<SolicitacaoRoteiroTesteFuncionalDomain, SolicitacaoRoteiroTesteFuncionalViewData>();
            cfg.CreateMap<SolicitacaoRoteiroTesteFuncionalViewData, SolicitacaoRoteiroTesteFuncionalDomain>();
            cfg.CreateMap<SolicitacaoRoteiroTesteFuncionalEvidenciaViewData, SolicitacaoRoteiroTesteFuncionalEvidenciaDTO>();
            cfg.CreateMap<SolicitacaoRoteiroTesteFuncionalEvidenciaDTO, SolicitacaoRoteiroTesteFuncionalEvidenciaViewData>();
            #endregion
        }
    }
}