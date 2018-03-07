using Lndr.MdsOnline.DataModel.Model;
using Lndr.MdsOnline.Web.Models.Domain;
using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Services
{
    public interface IMdsOnlineService
    {
        void UploadArquivo(ArquivoDTO arquivo);

        void ApagarArquivo(string guid);

        ArquivoDTO ObterArquivo(string guid);


        IEnumerable<SolicitacaoRTUDomain> ObterRTU(int solicitacaoID);

        void SalvarRTU(IEnumerable<SolicitacaoRTUDomain> rtu, int solicitacaoID);


        IEnumerable<SolicitacaoRTFDTO> ObterRTF(int solicitacaoID);

        void SalvarRTF(IEnumerable<SolicitacaoRTFDTO> RTF, int solicitacaoID);

        RtfDTO GetRTF(int solicitacaoID);

        void SaveRTF(RtfDTO dto);
    }
}
