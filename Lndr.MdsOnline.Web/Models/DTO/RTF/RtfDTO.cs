using System;
using System.Linq;
using System.Collections.Generic;
using Lndr.MdsOnline.Web.Helpers.Extensions;

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

        public IEnumerable<RtfTesteDTO> Testes { get; set; }

        public IEnumerable<int> TestesParaManter
        {
            get
            {
                return this.Testes.IsNullOrEmpty() ? new List<int>() : this.Testes.Where(t => t.RtfTesteID > 0).Select(t => t.RtfTesteID);
            }
        }
    }
}