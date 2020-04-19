using System.Web;
using System.Web.Optimization;

namespace FMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/animate.js",
                      "~/Scripts/viewportchecker.js",
                      "~/Scripts/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/exportpdf").Include(
                      "~/Scripts/tableExport.js",
                      "~/Scripts/jquery.base64.js",
                      "~/Scripts/html2canvas.js",
                      "~/Scripts/sprintf.js",
                      "~/Scripts/jspdf.js",
                      "~/Scripts/base64.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstraptable").Include(
                      "~/Scripts/bootstrap-table.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/listjs").Include(
                      "~/Scripts/list.js"));

            bundles.Add(new ScriptBundle("~/bundles/bags").Include("~/Scripts/bags.js"));

            bundles.Add(new ScriptBundle("~/bundles/highcharts").Include("~/Scripts/Highcharts-4.0.1/js/highcharts.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/custom.css",
                      "~/Content/animate.css",
                      "~/Content/datepicker3.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
