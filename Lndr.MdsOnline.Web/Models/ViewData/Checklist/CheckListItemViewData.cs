namespace Lndr.MdsOnline.Web.Models.ViewData.CheckList
{
    public class CheckListItemViewData
    {
        public int CheckListItemID { get; set; }

        public int CheckListGrupoItemID { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }



        public bool Sim { get; set; }

        public bool Nao { get; set; }

        public bool NaoAplicavel { get; set; }

        public string Observacao { get; set; }

        public int UsuarioID { get; set; }
    }
}