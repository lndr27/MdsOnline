using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Helpers.DataAccess
{
    public interface IPagination<T>
    {
        int Pagina { get; set; }

        int TotalPaginas { get; set; }

        bool PossuiProximaPagina { get; }

        bool PossuiPaginaAnterior { get; }

        int ProximaPagina { get; }

        int PaginaAnterior { get; }

        IEnumerable<T> Lista { get; set; }
    }
}
