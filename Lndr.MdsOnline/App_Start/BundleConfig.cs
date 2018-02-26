using System.Web.Optimization;

namespace Lndr.MdsOnline
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            EnableOptimizations();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                    "~/Scripts/popper-browser.js",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js",
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-medium-editor.js",
                    "~/Scripts/angular-ui-sortable.js",
                    "~/Scripts/medium-editor.js",
                    "~/Scripts/lodash.js",
                    "~/Scripts/alertify.js",
                    "~/Scripts/mousetrap.js",
                    "~/Scripts/ng-file-upload-shim.js",
                    "~/Scripts/ng-file-upload.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/angular-infra").IncludeDirectory("~/JS/infra", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/angular-services").IncludeDirectory("~/JS/controllers", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/angular-controllers").IncludeDirectory("~/JS/services", "*.js", true));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/font-awesome.css",
                    "~/Content/bootstrap.css",
                    "~/Content/medium-editor.css",
                    "~/Content/medium-beagle.css",
                    "~/Content/alertifyjs/alertify.css",
                    "~/Content/alertifyjs/themes/bootstrap.css",
                    "~/Content/site.css"));
            
        }

        private static void EnableOptimizations()
        {
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
 