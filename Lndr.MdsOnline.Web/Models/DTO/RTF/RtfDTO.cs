using System;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Models.DTO.RTF
{
    public class RtfDTO
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

        public int UsuarioAtualizacaoID { get; set; }

        public List<RtfTesteDTO> Testes { get; set; }
    }
}