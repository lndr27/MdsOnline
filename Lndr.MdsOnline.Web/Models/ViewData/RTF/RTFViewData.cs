using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Web.Models.ViewData.RTF
{
    public class RtfViewData
    {       
        public int Chamado { get; set; }

        public int SolicitacaoID { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }

        [Required]
        public string NomeUsuario { get; set; }

        public int UsuarioVerificacaoID { get; set; }

        public string NomeUsuarioVerificacao { get; set; }

        public List<RtfTesteViewData> Testes { get; set; }
    }    
}