using Lndr.MdsOnline.Models.Domain;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Models.DTO
{
    public class DocumentoGrupoItemCheckListDTO : DocumentoGrupoItemCheckListDomain
    {
        public IEnumerable<DocumentoItemCheckListDomain> ItensChecklist;
    }
}