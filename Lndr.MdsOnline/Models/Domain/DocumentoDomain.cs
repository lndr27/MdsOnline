using System;

namespace Lndr.MdsOnline.Models.Domain
{
    public class DocumentoDomain
    {
        public int DocumentoID { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public int TipoDocumentoID { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioAtualizacaoID { get; set; }
    }
}