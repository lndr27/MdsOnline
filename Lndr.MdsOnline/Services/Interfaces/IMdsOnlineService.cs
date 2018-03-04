using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Models.DTO;
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
    }
}
