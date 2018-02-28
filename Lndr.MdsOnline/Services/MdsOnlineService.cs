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

        private readonly IServiceContext _context;

        public MdsOnlineService(IServiceContext serviceContext, IMdsOnlineRepository repository)
        {
            this._repository = repository;
            this._context = serviceContext;
        }

        public void UploadArquivo(ArquivoDTO arquivo)
        {
            this._repository.UploadArquivo(arquivo);
        }

        public void ApagarArquivo(string guid)
        {
            this._repository.RemoverArquivo(guid);
        }

        public ArquivoDTO ObterArquivo(string guid)
        {
            return this._repository.ObterArquivo(guid);
        }

        public IEnumerable<SolicitacaoRTUDomain> ObterRTU(int solicitacaoID)
        {
            return this._repository.ObterRTU(solicitacaoID);
        }

        public void SalvarRTU(IEnumerable<SolicitacaoRTUDomain> RTU, int solicitacaoID)
        {
            this._repository.SalvarRTU(RTU, solicitacaoID);
        }

        public IEnumerable<SolicitacaoRTFDTO> ObterRTF(int solicitacaoID)
        {
            var testesDomain = this._repository.ObterTestesRTF(solicitacaoID);
            var evidencias = this._repository.ObterEvidenciasRTF(solicitacaoID);
            var testes = Mapper.Map<List<SolicitacaoRTFDTO>>(testesDomain);

            if (!testes.IsNullOrEmpty() && !evidencias.IsNullOrEmpty())
            {
                testes.ForEach(t => {
                    t.Evidencias = evidencias.Where(e => e.SolicitacaoRTFID == t.SolicitacaoRTFID);
                });
            }
            return testes;
        }

        public void SalvarRTF(IEnumerable<SolicitacaoRTFDTO> RTF, int solicitacaoID)
        {
            this._repository.SalvarRTF(RTF, solicitacaoID);
        }        
    }
}