namespace Lndr.MdsOnline.Models.Domain
{
    public class DocumentoGrupoItemChecklistOpcaoDomain
    {
        public int DocumentoGrupoItemChecklistOpcaoID { get; set; }

        public int DocumentoGrupoItemChecklistID { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public int Ordem { get; set; }

        public bool Obrigatorio { get; set; }

        public string DataType { get; set; }

        public string Funcao { get; set; }
    }
}