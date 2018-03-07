using System;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Models.ViewData.RTF
{
    public class RTFViewData
    {       
        public int Chamado { get; set; }

        public int SolicitacaoID { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }

        public string NomeUsuario { get; set; }

        public int UsuarioVerificacaoID { get; set; }

        public string NomeUsuarioVerificacao { get; set; }

        public IEnumerable<RtfTesteViewData> Testes { get; set; }
    }    
}