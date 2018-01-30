using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Models.Enum
{
    public enum DataTypeEnum
    {
        Texto = 1,

        [Display(Name = "Número")]
        Numero = 2,

        Data = 3,

        Booleano = 4
    }
}