using AutoMapper;
using Lndr.MdsOnline.DataModel.Model;
using Lndr.MdsOnline.Web.Helpers;
using Lndr.MdsOnline.Services;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Lndr.MdsOnline
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new NullDatabaseInitializer<MdsOnlineDbContext>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IocConfig.RegisterIoc(new Autofac.ContainerBuilder());
            Mapper.Initialize(MapperConfig.ConfigMapper);
        }

        protected void Application_BeginRequest()
        {
            var authService = ServiceLocator.Resolve<IAuthenticationService>();
        }

        protected void Application_EndRequest()
        {

        }

        protected void Application_Error()
        {

        }
    }
}
