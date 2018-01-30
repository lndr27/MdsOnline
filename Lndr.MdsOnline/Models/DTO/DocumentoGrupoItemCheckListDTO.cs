using Lndr.MdsOnline.Models.Domain;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Models.DTO
{
    public class DocumentoGrupoItemChecklistDTO : DocumentoGrupoItemChecklistDomain
    {
        public IEnumerable<DocumentoGrupoItemChecklistOpcaoDomain> Opcoes { get; set; }

        public IEnumerable<DocumentoItemChecklistDomain> ItensChecklist { get; set; }
    }
}