using AutoMapper;
using Lndr.MdsOnline.Helpers.Extensions;
using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Models.DTO;
using Lndr.MdsOnline.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Lndr.MdsOnline.Services
{
    public class MdsOnlineService : IMdsOnlineService
    {
        private readonly IMdsOnlineRepository _repository;

        public MdsOnlineService(IMdsOnlineRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> ObterRTU(int solicitacaoID)
        {
            return this._repository.ObterRTU(solicitacaoID);
        }

        public void SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> RTU, int solicitacaoID)
        {
            this._repository.SalvarRTU(RTU, solicitacaoID);
        }

        public IEnumerable<SolicitacaoRoteiroTesteFuncionalDTO> ObterRTF(int solicitacaoID)
        {
            var testesDomain = this._repository.ObterTestesRTF(solicitacaoID);
            var evidencias = this._repository.ObterEvidenciasRTF(solicitacaoID);
            var testes = Mapper.Map<List<SolicitacaoRoteiroTesteFuncionalDTO>>(testesDomain);

            if (!testes.IsNullOrEmpty() && !evidencias.IsNullOrEmpty())
            {
                testes.ForEach(t => {
                    t.Evidencias = evidencias.Where(e => e.SolicitacaoRoteiroTesteFuncionalID == t.SolicitacaoRoteiroTesteFuncionalID);
                });
            }
            return testes;
        }

        public void SalvarRTF(IEnumerable<SolicitacaoRoteiroTesteFuncionalDomain> RTF, int solicitacaoID)
        {
            this._repository.SalvarRTF(RTF, solicitacaoID);
        }
    }
}