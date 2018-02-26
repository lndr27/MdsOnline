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


        IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> ObterRTU(int solicitacaoID);

        void SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> rtu, int solicitacaoID);


        IEnumerable<SolicitacaoRoteiroTesteFuncionalDTO> ObterRTF(int solicitacaoID);

        void SalvarRTF(IEnumerable<SolicitacaoRoteiroTesteFuncionalDTO> RTF, int solicitacaoID);        
    }
}
