using Lndr.MdsOnline.Web.Models.Domain.Rtf;
using Lndr.MdsOnline.Web.Models.Domain.Rtu;
using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Web.Models.DTO.CheckList;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using Lndr.MdsOnline.Web.Models.DTO.Rtu;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Repositories
{
    public interface IMdsOnlineRepository
    {
        #region UPLOAD ARQUIVOS +
        void UploadArquivo(ArquivoDTO arquivo);

        void RemoverArquivo(string guid);

        ArquivoDTO ObterArquivo(string guid);
        #endregion

        #region RTU +
        RtuDomain ObterRtu(int solicitacaoID);

        IEnumerable<RtuTesteDomain> ObterTestesRTU(int solicitacaoID);

        void SalvarRtu(RtuDTO rtu);
        #endregion

        #region RTF +
        RtfDomain ObterRtf(int solicitacaoID);

        IEnumerable<RtfTesteDomain> ObterRtfTestes(int solicitacaoID);

        IEnumerable<RtfTesteEvidenciaDTO> ObterRtfTesteEvidencias(int solicitacaoID);

        void SalvarRTF(RtfDTO rtf);
        #endregion

        #region CheckList +
        CheckListDTO ObterCheckList(int solicitacaoID, int checklistID);

        IEnumerable<CheckListGrupoItemDTO> ObterCheckListGrupoItem(int checklistID);

        IEnumerable<CheckListItemDTO> ObterCheckListItens(int solicitacaoID, int checklistID);

        void SalvarCheckList(CheckListDTO checklist);

        List<PaginacaoCheckListDTO> ObterListaCheckLists();

        int ObterQuantidadeTotalCheckLists();
        #endregion
    }
}