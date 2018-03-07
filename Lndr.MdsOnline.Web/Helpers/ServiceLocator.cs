using Autofac;

namespace Lndr.MdsOnline.Web.Helpers
{
    public static class ServiceLocator
    {
        public static IContainer Container { get; set; }

        public static T Resolve<T>()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                return scope.Resolve<T>();
            }
        }
    }
}