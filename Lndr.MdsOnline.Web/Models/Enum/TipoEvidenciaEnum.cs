using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Web.Models.Enum
{
    public enum TipoEvidenciaEnum
    {
        [Display(Name = "Teste bem-sucedido")]
        Sucesso = 1,

        [Display(Name = "Teste com erros")]
        Erro = 2
    }
}