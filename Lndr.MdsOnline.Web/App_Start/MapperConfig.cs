using AutoMapper;
using Lndr.MdsOnline.Web.Models.Domain.Rtf;
using Lndr.MdsOnline.Web.Models.Domain.Rtu;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using Lndr.MdsOnline.Web.Models.DTO.Rtu;
using Lndr.MdsOnline.Web.Models.ViewData.RTF;
using Lndr.MdsOnline.Web.Models.ViewData.Rtu;
using Lndr.MdsOnline.Web.Models.DTO.CheckList;
using Lndr.MdsOnline.Web.Models.ViewData.CheckList;

namespace Lndr.MdsOnline
{
    public static class MapperConfig
    {
        public static void ConfigMapper (IMapperConfigurationExpression cfg)
        {
            ConfigRtfMaps(cfg);
            ConfigRtfMaps(cfg);
            ConfigChecklistMaps(cfg);
        }

        private static void ConfigRtfMaps(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RtfDomain, RtfDTO>();
            cfg.CreateMap<RtfTesteDomain, RtfTesteDTO>();
            cfg.CreateMap<RtfDTO, RtfViewData>();
            cfg.CreateMap<RtfTesteDTO, RtfTesteViewData>();
            cfg.CreateMap<RtfTesteEvidenciaDTO, RtfTesteEvidenciaViewData>();
            cfg.CreateMap<RtfViewData, RtfDTO>();
            cfg.CreateMap<RtfTesteViewData, RtfTesteDTO>();
            cfg.CreateMap<RtfTesteEvidenciaViewData, RtfTesteEvidenciaDTO>();
        }

        private static void ConfigRtuMaps(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RtuDomain, RtuDTO>();
            cfg.CreateMap<RtuDTO, RtuViewData>();
            cfg.CreateMap<RtuViewData, RtuDTO>();
            cfg.CreateMap<RtuTesteDomain, RtuTesteDTO>();
            cfg.CreateMap<RtuTesteDTO, RtuTesteViewData>();
            cfg.CreateMap<RtuTesteViewData, RtuTesteDTO>();
        }

        private static void ConfigChecklistMaps(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CheckListDTO, CheckListViewData>();
            cfg.CreateMap<CheckListGrupoItemDTO, CheckListGrupoItemViewData>();
            cfg.CreateMap<CheckListItemDTO, CheckListItemViewData>();

            cfg.CreateMap<CheckListViewData, CheckListDTO>();
            cfg.CreateMap<CheckListGrupoItemViewData, CheckListGrupoItemDTO>();
            cfg.CreateMap<CheckListItemViewData, CheckListItemDTO>();
        }
    }
}