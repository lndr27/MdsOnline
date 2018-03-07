using AutoMapper;
using Lndr.MdsOnline.DataModel.Model;
using Lndr.MdsOnline.Web.Models.Domain;
using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Web.Models.ViewData;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using Lndr.MdsOnline.Web.Models.ViewData.RTF;

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

            cfg.CreateMap<RtfDTO, RTFViewData>();
            cfg.CreateMap<RTFViewData, RtfDTO>();
            cfg.CreateMap<RtfTesteDTO, RtfTesteViewData>();
            cfg.CreateMap<RtfTesteViewData, RtfTesteDTO>();
            cfg.CreateMap<RtfTesteEvidenciaDTO, RtfTesteEvidenciaViewData>();
            cfg.CreateMap<RtfTesteEvidenciaViewData, RtfTesteEvidenciaDTO>();

            cfg.CreateMap<RTF, RtfHistorico>();
            cfg.CreateMap<RtfTeste, RtfTesteHistorico>();
            cfg.CreateMap<RtfTesteEvidencia, RtfTesteEvidenciaHistorico>();
            #endregion
        }
    }
}