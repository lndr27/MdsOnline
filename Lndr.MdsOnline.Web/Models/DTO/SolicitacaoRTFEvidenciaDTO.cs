using System;

namespace Lndr.MdsOnline.Web.Models.DTO
{
    public class SolicitacaoRTFEvidenciaDTO
    {
        public int SolicitacaoRTFEvidenciaID { get; set; }

        public int SolicitacaoRTFID { get; set; }

        public int TipoEvidenciaID { get; set; }

        public string GuidImagem { get; set; }

        public string Descricao { get; set; }

        public int Ordem { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }
    }
}