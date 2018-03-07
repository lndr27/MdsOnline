using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Web.Models.Enum
{
    public enum StatusVerificacaoTesteUnitarioEnum
    {
        [Display(Name = "Não Testado")]
        NaoTestado = 1,

        [Display(Name = "Sim, OK")]
        OK = 2,        

        [Display(Name = "Sim, NOK")]
        NOK = 3
    }
}