using System.Collections.Generic;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class RTFViewData
    {       
        public int Chamado { get; set; }

        public IEnumerable<SolicitacaoRTFViewData> Testes { get; set; }
    }
}