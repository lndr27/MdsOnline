using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Models.DTO;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Repositories
{
    public interface IMdsOnlineRepository
    {
        IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> ObterRTU(int solicitacaoID);

        void SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> rtu, int solicitcaoID);

        IEnumerable<SolicitacaoRoteiroTesteFuncionalDomain> ObterTestesRTF(int solictiacaoID);

        IEnumerable<SolicitacaoRoteiroTesteFuncionalEvidenciaDTO> ObterEvidenciasRTF(int solicitacaoID);

        void SalvarRTF(IEnumerable<SolicitacaoRoteiroTesteFuncionalDomain> rtf, int solicitacaoID);
    }
}