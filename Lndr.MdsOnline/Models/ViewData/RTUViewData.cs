using System.Collections.Generic;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class RTUViewData
    {
        public int Chamado { get; set; }

        public IEnumerable<SolicitacaoRoteiroTesteUnitarioViewData> Testes { get; set; }
    }
}