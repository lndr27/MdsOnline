﻿using System;

namespace Lndr.MdsOnline.Models.Domain
{
    public class SolicitacaoRTUDomain
    {
        public int SolicitacaoRTUID { get; set; }

        public int SolicitacaoID { get; set; }

        public string Sequencia { get; set; }

        public string Condicao { get; set; }

        public string DadosEntrada { get; set; }

        public string ResultadoEsperado { get; set; }

        public int StatusVerificacaoTesteUnitarioID { get; set; }

        public string ComoTestar { get; set; }

        public string Observacoes { get; set; }

        public int Ordem { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }
    }
}