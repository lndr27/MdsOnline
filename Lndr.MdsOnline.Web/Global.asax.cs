using AutoMapper;
using Lndr.MdsOnline.Services;
using Lndr.MdsOnline.Web.Helpers;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Lndr.MdsOnline
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IocConfig.RegisterIoc(new Autofac.ContainerBuilder());
            Mapper.Initialize(MapperConfig.ConfigMapper);
        }


        protected void Application_BeginRequest()
        {            
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_EndRequest()
        {

        }

        protected void Application_Error()
        {

        }
    }
}
