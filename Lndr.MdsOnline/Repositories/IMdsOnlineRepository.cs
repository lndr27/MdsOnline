using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Models.DTO;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Repositories
{
    public interface IMdsOnlineRepository
    {
        void UploadArquivo(ArquivoDTO arquivo);

        void RemoverArquivo(string guid);

        ArquivoDTO ObterArquivo(string guid);


        IEnumerable<SolicitacaoRTUDomain> ObterRTU(int solicitacaoID);

        void SalvarRTU(IEnumerable<SolicitacaoRTUDomain> rtu, int solicitcaoID);


        IEnumerable<SolicitacaoRTFDomain> ObterTestesRTF(int solictiacaoID);

        IEnumerable<SolicitacaoRTFEvidenciaDTO> ObterEvidenciasRTF(int solicitacaoID);

        void SalvarRTF(IEnumerable<SolicitacaoRTFDTO> rtf, int solicitacaoID);        
    }
}