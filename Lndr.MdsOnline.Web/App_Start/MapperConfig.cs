using AutoMapper;
using Lndr.MdsOnline.Web.Models.Domain;
using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Web.Models.ViewData;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using Lndr.MdsOnline.Web.Models.ViewData.RTF;
using Lndr.MdsOnline.Web.Models.Domain.Rtu;
using Lndr.MdsOnline.Web.Models.DTO.Rtu;
using Lndr.MdsOnline.Web.Models.ViewData.Rtu;
using Lndr.MdsOnline.Web.Models.Domain.Rtf;

namespace Lndr.MdsOnline
{
    public static class MapperConfig
    {
        public static void ConfigMapper (IMapperConfigurationExpression cfg)
        {
            #region RTU +
            cfg.CreateMap<RtuDomain, RtuDTO>();
            cfg.CreateMap<RtuDTO, RtuViewData>();
            cfg.CreateMap<RtuViewData, RtuDTO>();
            cfg.CreateMap<RtuTesteDomain, RtuTesteDTO>();
            cfg.CreateMap<RtuTesteDTO, RtuTesteViewData>();
            cfg.CreateMap<RtuTesteViewData, RtuTesteDTO>();
            #endregion

            #region RTF +
            cfg.CreateMap<RtfDomain, RtfDTO>();
            cfg.CreateMap<RtfTesteDomain, RtfTesteDTO>();

            cfg.CreateMap<RtfDTO, RtfViewData>();
            cfg.CreateMap<RtfTesteDTO, RtfTesteViewData>();
            cfg.CreateMap<RtfTesteEvidenciaDTO, RtfTesteEvidenciaViewData>();
            cfg.CreateMap<RtfViewData, RtfDTO>();
            cfg.CreateMap<RtfTesteViewData, RtfTesteDTO>();
            cfg.CreateMap<RtfTesteEvidenciaViewData, RtfTesteEvidenciaDTO>();
            #endregion
        }
    }
}