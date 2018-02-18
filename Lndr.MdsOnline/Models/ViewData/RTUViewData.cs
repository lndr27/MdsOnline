﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class RTUViewData
    {
        [Required]
        public int Chamado { get; set; }

        public IEnumerable<SolicitacaoRoteiroTesteUnitarioViewData> Testes { get; set; }
    }
}