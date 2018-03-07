using System;

namespace Lndr.MdsOnline.Web.Helpers.Extensions
{
    public static class ObjectExtensions
    {
        public static T GetValue<T>(this object obj, string propertyName)
        {
            try
            {
                return (T)obj.GetType().GetProperty(propertyName).GetValue(obj);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static object GetValue(this object obj, string propertyName)
        {
            try
            {
                return obj.GetType().GetProperty(propertyName).GetValue(obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}