using System;

namespace Lndr.MdsOnline.Models.DTO
{
    public class ArquivoDTO
    {
        public Guid Guid { get; set; }

        public string Nome { get; set; }

        public string Extensao { get; set; }

        public string ContentType { get; set; }

        public float TamanhoKb { get; set; }

        public byte[] Arquivo { get; set; }

        public bool IsRascunho { get; set; }

        public DateTime DataUpload { get; set; }

        public int UsuarioID { get; set; }
    }
}