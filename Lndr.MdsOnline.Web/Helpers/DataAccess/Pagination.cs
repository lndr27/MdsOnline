using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Helpers.DataAccess
{
    public class Pagination<T> : IPagination<T>
    {
        public Pagination()
        {
            this.Lista = new List<T>();
        }

        public int Pagina { get; set; }

        public int TotalPaginas { get; set; }

        public bool PossuiProximaPagina
        {
            get { return this.Pagina < this.TotalPaginas; }
        }

        public bool PossuiPaginaAnterior
        {
            get { return this.Pagina > 1; }
        }

        public int ProximaPagina
        {
            get { return this.PossuiProximaPagina ? this.Pagina + 1 : this.Pagina; }
        }

        public int PaginaAnterior
        {
            get { return this.PossuiPaginaAnterior ? this.Pagina - 1 : this.Pagina; }
        }

        public IEnumerable<T> Lista { get; set; }
    }
}