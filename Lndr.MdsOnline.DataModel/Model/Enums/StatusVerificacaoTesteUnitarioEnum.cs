using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.DataModel
{
    public enum StatusVerificacaoTesteUnitarioEnum
    {
        [Display(Name = "N�o Testado")]
        NaoTestado = 1,

        [Display(Name = "Sim, OK")]
        OK = 2,

        [Display(Name = "Sim, NOK")]
        NOK = 3
    }
}
