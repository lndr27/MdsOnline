using Autofac;

namespace Lndr.MdsOnline.Helpers
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