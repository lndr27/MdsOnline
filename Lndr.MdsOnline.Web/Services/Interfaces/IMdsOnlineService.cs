using Lndr.MdsOnline.Web.Helpers.DataAccess;
using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Web.Models.DTO.CheckList;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using Lndr.MdsOnline.Web.Models.DTO.Rtu;

namespace Lndr.MdsOnline.Services
{
    public interface IMdsOnlineService
    {
        #region Upload Arquivos +
        void UploadArquivo(ArquivoDTO arquivo);

        void ApagarArquivo(string guid);

        ArquivoDTO ObterArquivo(string guid);
        #endregion


        #region RTU +
        RtuDTO ObterRtu(int solicitacaoID);

        void SalvarRtu(RtuDTO rtu);
        #endregion

        #region Checklist
        RtfDTO ObterRTF(int solicitacaoID);

        void SalvarRTF(RtfDTO rtf);
        #endregion


        #region Checklist
        CheckListDTO ObterCheckList(int checklistID);

        CheckListDTO ObterCheckListSolicitacao(int solicitacaoID, int checklistID);

        int SalvarCheckList(CheckListDTO checklist);

        IPagination<CheckListDTO> ObterListaCheckLists(int pagina, int tamanhoPagina);
        #endregion
    }
}
