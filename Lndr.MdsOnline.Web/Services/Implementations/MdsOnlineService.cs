using AutoMapper;
using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Web.Models.DTO.CheckList;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using Lndr.MdsOnline.Web.Models.DTO.Rtu;
using Lndr.MdsOnline.Web.Models.Enum;
using Lndr.MdsOnline.Web.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Lndr.MdsOnline.Services
{
    public class MdsOnlineService : IMdsOnlineService
    {
        private readonly IMdsOnlineRepository _repository;

        private readonly IServiceContext _userContext;

        public MdsOnlineService(IServiceContext serviceContext, IMdsOnlineRepository repository)
        {
            this._repository = repository;
            this._userContext = serviceContext;
        }

        #region Upload Arquivos +
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
        #endregion

        #region RTU +
        public RtuDTO ObterRtu(int solicitacaoID)
        {
            var rtuDomain = this._repository.ObterRtu(solicitacaoID);
            var rtuTestesDomain = this._repository.ObterTestesRTU(solicitacaoID).ToList();

            var rtuDto = Mapper.Map<RtuDTO>(rtuDomain) ?? new RtuDTO { SolicitacaoID = solicitacaoID };
            var testesDto = Mapper.Map<List<RtuTesteDTO>>(rtuTestesDomain);
            rtuDto.Testes = testesDto;

            return rtuDto;
        }

        public void SalvarRtu(RtuDTO rtu)
        {
            this._repository.SalvarRtu(rtu);
        }
        #endregion

        #region RTF +
        public RtfDTO ObterRTF(int solicitacaoID)
        {
            var rtf = Mapper.Map<RtfDTO>(this._repository.ObterRtf(solicitacaoID)) ?? new RtfDTO();
            var testes = Mapper.Map<List<RtfTesteDTO>>(this._repository.ObterRtfTestes(solicitacaoID));
            var evidencias = this._repository.ObterRtfTesteEvidencias(solicitacaoID);
            testes.ForEach(t => {
                t.Evidencias = evidencias.Where(e => e.RtfTesteID == t.RtfTesteID && e.TipoEvidenciaID == (int)TipoEvidenciaEnum.Sucesso).ToList();
                t.Erros = evidencias.Where(e => e.RtfTesteID == t.RtfTesteID && e.TipoEvidenciaID == (int)TipoEvidenciaEnum.Erro).ToList();
            });
            rtf.Testes = testes;
            return rtf;
        }

        public void SalvarRTF(RtfDTO rtf)
        {
            this._repository.SalvarRTF(rtf);
        }
        #endregion

        #region CheckList +
        public CheckListDTO ObterCheckList(int checklistID)
        {
            return this.ObterCheckListSolicitacao(0, checklistID);
        }

        public CheckListDTO ObterCheckListSolicitacao(int solicitacaoID, int checklistID)
        {
            var checklist         = this._repository.ObterCheckList(solicitacaoID, checklistID);
            if (checklist == null)
            {
                return new CheckListDTO();
            }

            checklist.GruposItens = this._repository.ObterCheckListGrupoItem(checklistID).ToList();
            var itens             = this._repository.ObterCheckListItens(solicitacaoID, checklistID);
            checklist.GruposItens.ForEach(g => {
                g.Itens.AddRange(itens.Where(i => i.CheckListGrupoItemID == g.CheckListGrupoItemID));
            });
            return checklist;
        }

        public void SalvarCheckList(CheckListDTO checklist)
        {
            this._repository.SalvarCheckList(checklist);
        }
        #endregion
    }
}