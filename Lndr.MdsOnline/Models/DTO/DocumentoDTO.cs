using Lndr.MdsOnline.Models.Domain;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Models.DTO
{
    public class DocumentoDTO: DocumentoDomain
    {
        public IEnumerable<DocumentoGrupoItemCheckListDTO> GruposItensChecklist { get; set; }

        public IEnumerable<DocumentoTopicoDomain> Topicos { get; set; }
    }
}