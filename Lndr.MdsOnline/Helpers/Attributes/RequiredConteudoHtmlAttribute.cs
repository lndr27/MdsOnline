using Lndr.MdsOnline.Helpers.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredConteudoHtmlAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            var textoSemHtml = value.ToString().RemoverTagsHtml().RemoverEspacosEmBranco();
            return !string.IsNullOrWhiteSpace(textoSemHtml);
        }
    }
}