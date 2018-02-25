using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Models.DTO;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Services
{
    public interface IMdsOnlineService
    {
        IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> ObterRTU(int solicitacaoID);

        void SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> rtu, int solicitacaoID);


        IEnumerable<SolicitacaoRoteiroTesteFuncionalDTO> ObterRTF(int solicitacaoID);

        void SalvarRTF(IEnumerable<SolicitacaoRoteiroTesteFuncionalDomain> RTF, int solicitacaoID);
    }
}
