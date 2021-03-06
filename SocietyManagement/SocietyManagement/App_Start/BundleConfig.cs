﻿using System.Web;
using System.Web.Optimization;

namespace SocietyManagement
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js", "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js", "~/Scripts/bootstrap-datepicker.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/AdminLTE.css"));

            bundles.Add(new StyleBundle("~/Content/editor").Include(
                      "~/Scripts/froala-editor/css/froala_style.min.css",
                      "~/Scripts/froala-editor/css/froala_editor.pkgd.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/editor").Include(                      
                      "~/Scripts/froala-editor/js/froala_editor.pkgd.min.js"));
        }
    }
}
