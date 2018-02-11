using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Models.Enum
{
    public enum StatusTesteUnitarioEnum
    {       
        [Display(Name = "Sim, OK")]
        OK = 1,
        
        [Display(Name = "Não Testado")]
        NaoTestado = 2,

        [Display(Name = "Com Erro")]
        ComErro = 3
    }
}