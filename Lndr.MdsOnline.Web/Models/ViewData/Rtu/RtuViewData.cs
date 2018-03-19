using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lndr.MdsOnline.Web.Models.ViewData.Rtu
{
    public class RtuViewData
    {
        public int RtuID { get; set; }

        public int SolicitacaoID { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }

        public int UsuarioVerificacaoID { get; set; }

        public int UsuarioAtualizacaoID { get; set; }

        public List<RtuTesteViewData> Testes { get; set; }
    }
}