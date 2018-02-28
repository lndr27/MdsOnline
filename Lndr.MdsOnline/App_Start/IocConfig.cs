using Autofac;
using Autofac.Integration.Mvc;
using Lndr.MdsOnline.Services;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Lndr.MdsOnline
{
    public static class IocConfig
    {
        public static void RegisterIoc(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            builder.RegisterFilterProvider();

            builder.RegisterType<ServiceContext>().As<IServiceContext>();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(r => r.Name.EndsWith("Repository")).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(r => r.Name.EndsWith("Service")).AsImplementedInterfaces();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}