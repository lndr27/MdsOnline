using System;

namespace Lndr.MdsOnline.Web.Models.DTO.CheckList
{
    public class PaginacaoCheckListDTO
    {
        public int CheckListID { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public string NomeUsuarioAtualizacao { get; set; }

        public string NomeUsuarioCriacao { get; set; }
    }
}