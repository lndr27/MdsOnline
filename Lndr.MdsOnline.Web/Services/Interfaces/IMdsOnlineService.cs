using Lndr.MdsOnline.Web.Models.Domain;
using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Web.Models.DTO.CheckList;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using Lndr.MdsOnline.Web.Models.DTO.Rtu;
using System.Collections.Generic;

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

        void SalvarCheckList(CheckListDTO checklist);
        #endregion
    }
}
