using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Models.ViewData
{
    public class RTUViewData
    {       
        public int Chamado { get; set; }

        public IEnumerable<SolicitacaoRTUViewData> Testes { get; set; }
    }
}