using Lndr.MdsOnline.Models.Domain;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Services
{
    public interface IMdsOnlineService
    {
        IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> ObterRtu(int solicitacaoID);

        void SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> rtu, int solicitacaoID);
    }
}
