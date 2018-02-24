using Lndr.MdsOnline.Helpers.Extensions;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class CampoViewData
    {
        public string Campo { get; set; }

        public IEnumerable<string> Erros { get; set; }

        public bool IsValido { get { return this.Erros.IsNullOrEmpty(); } }
    }
}