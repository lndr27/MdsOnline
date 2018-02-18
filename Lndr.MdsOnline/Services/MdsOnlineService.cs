using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Repositories;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Services
{
    public class MdsOnlineService : IMdsOnlineService
    {
        private readonly IMdsOnlineRepository _repository;

        public MdsOnlineService(IMdsOnlineRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> ObterRtu(int solicitacaoID)
        {
            return this._repository.ObterRTU(solicitacaoID);
        }

        public void SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> RTU, int solicitacaoID)
        {
            this._repository.SalvarRTU(RTU, solicitacaoID);
        }
    }
}