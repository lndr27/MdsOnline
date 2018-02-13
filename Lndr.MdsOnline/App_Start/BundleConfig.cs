using System.Web;
using System.Web.Optimization;

namespace Lndr.MdsOnline
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                    "~/Scripts/angular.js"));

            bundles.Add(new ScriptBundle("~/bundles/medium").Include(
                    "~/Scripts/angular-medium-editor.js",
                    "~/Scripts/medium-editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-infra").IncludeDirectory("~/JS/infra", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/angular-services").IncludeDirectory("~/JS/controllers", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/angular-controllers").IncludeDirectory("~/JS/services", "*.js", true));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/medium-editor.css",
                    "~/Content/medium-beagle.css",
                    "~/Content/site.css"));
            
        }
    }
}
 