using System;

namespace Lndr.MdsOnline.Web.Models.Domain.Rtu
{
    public class RtuDomain
    {
        public int RtuID { get; set; }

        public int SolicitacaoID { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }

        public int UsuarioVerificacaoID { get; set; }

        public int UsuarioAtualizacaoID { get; set; }

    }
}