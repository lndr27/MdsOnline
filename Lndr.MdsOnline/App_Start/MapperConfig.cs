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
            cfg.CreateMap<SolicitacaoRTUDomain, SolicitacaoRTUViewData>();
            cfg.CreateMap<SolicitacaoRTUViewData, SolicitacaoRTUDomain>();
            #endregion

            #region RTF +
            cfg.CreateMap<SolicitacaoRTFDomain, SolicitacaoRTFDTO>();
            cfg.CreateMap<SolicitacaoRTFDTO, SolicitacaoRTFViewData>();
            cfg.CreateMap<SolicitacaoRTFViewData, SolicitacaoRTFDTO>();
            cfg.CreateMap<SolicitacaoRTFEvidenciaViewData, SolicitacaoRTFEvidenciaDTO>();
            cfg.CreateMap<SolicitacaoRTFEvidenciaDTO, SolicitacaoRTFEvidenciaViewData>();
            #endregion
        }
    }
}