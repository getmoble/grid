using System.Web.Optimization;

namespace Grid
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/Content/styleBundle")
                   .Include("~/Content/bootstrap.theme.min.css",
                            "~/Content/style.css",
                            "~/Content/bootstrap-datetimepicker.min.css",
                            "~/Content/c3.min.css",
                            "~/Content/jquery-te-1.4.0.css",
                            "~/Content/fullcalendar.min.css",
                            "~/Content/font-awesome.min.css",
                            "~/Content/timeline.min.css",
                            "~/Content/selectize.css",
                            "~/Content/selectize.bootstrap3.css",
                            "~/Content/ladda-themeless.min.css",
                            "~/Content/animate.css"));

            bundles.Add(new Bundle("~/Scripts/scriptBundle")
                   .Include("~/Scripts/jquery-2.2.1.min.js",
                            "~/Scripts/jquery-te-1.4.0.min.js",
                            "~/Scripts/moment.min.js",
                            "~/Scripts/bootstrap.min.js",
                            "~/Scripts/bootbox.min.js",
                            "~/Scripts/fullcalendar.min.js",
                            "~/Scripts/selectize.min.js",
                            "~/Scripts/bootstrap-datetimepicker.min.js",
                            "~/Scripts/d3.v3.min.js",
                            "~/Scripts/c3.min.js",
                            "~/Scripts/knockout-3.4.0.js",
                            "~/Scripts/spin.min.js",
                            "~/Scripts/ladda.min.js",
                            "~/Scripts/ko.custom.js",
                            "~/App/Framework.js",
                            "~/Scripts/html2canvas.min.js",
                            "~/Scripts/userFeedback.js"));

        }
    }
}