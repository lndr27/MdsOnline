using System;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Models.DTO.Rtu
{
    public class RtuDTO
    {
        public int RtuID { get; set; }

        public int SolicitacaoID { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }

        public int UsuarioVerificacaoID { get; set; }

        public int UsuarioAtualizacaoID { get; set; }

        public List<RtuTesteDTO> Testes { get; set; }
    }
}