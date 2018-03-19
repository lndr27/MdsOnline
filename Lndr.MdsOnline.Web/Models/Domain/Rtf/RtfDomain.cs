using System;

namespace Lndr.MdsOnline.Web.Models.Domain.Rtf
{
    public class RtfDomain
    {
        public int RtfID { get; set; }

        public int Chamado { get; set; }

        public int SolicitacaoID { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }

        public string NomeUsuario { get; set; }

        public int UsuarioVerificacaoID { get; set; }

        public string NomeUsuarioVerificacao { get; set; }
    }
}