using System.Web;
using System.Web.Optimization;

namespace VivedyWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Bundle with jquery scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.4.1.slim.min.js"));

            //Bundle with jquery scripts for field validations
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //Bundle with bootstrap and custom scripts
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap-4.0.0.min.js",
                      "~/Scripts/navbarred.js"));

            //Bundle with popper scripts
            bundles.Add(new ScriptBundle("~/bundles/popper").Include(
                      "~/Scripts/popper.min.js"));

            //Bundle with zxing scripts
            bundles.Add(new ScriptBundle("~/bundles/scanner").Include(
                      "~/Scripts/bookingVerification.js"));

            //Bundle with search and filters scripts
            bundles.Add(new ScriptBundle("~/bundles/searchAndFilters").Include(
                      "~/Scripts/searchAndfilter.js"));

            //Bundle with bootstrap and custom css files
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/vivedy-bootstrap.css",
                      "~/Content/all.min.css"));

            //Admin Sidebar
            bundles.Add(new ScriptBundle("~/Scripts/AdminSideBar").Include(
                      "~/Scripts/AdminSideBar.js"));


        }
    }
}
