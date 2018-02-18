using Lndr.MdsOnline.Models.Domain;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Repositories
{
    public interface IMdsOnlineRepository
    {
        IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> ObterRTU(int solicitacaoID);

        void SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> rtu, int solicitcaoID);
    }
}