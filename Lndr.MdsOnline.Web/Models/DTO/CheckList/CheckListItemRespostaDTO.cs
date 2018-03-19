namespace Lndr.MdsOnline.Web.Models.DTO.CheckList
{
    public class CheckListItemRespostaDTO
    {
        public bool Sim { get; set; }

        public bool Nao { get; set; }

        public bool NaoAplicavel { get; set; }

        public string Observacao { get; set; }

        public int UsuarioID { get; set; }
    }
}