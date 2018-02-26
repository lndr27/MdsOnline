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


        IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> ObterRTU(int solicitacaoID);

        void SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> rtu, int solicitcaoID);


        IEnumerable<SolicitacaoRoteiroTesteFuncionalDomain> ObterTestesRTF(int solictiacaoID);

        IEnumerable<SolicitacaoRoteiroTesteFuncionalEvidenciaDTO> ObterEvidenciasRTF(int solicitacaoID);

        void SalvarRTF(IEnumerable<SolicitacaoRoteiroTesteFuncionalDTO> rtf, int solicitacaoID);        
    }
}