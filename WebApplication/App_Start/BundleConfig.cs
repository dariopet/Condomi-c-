using System.Web.Optimization;

namespace WebApplication
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterLayout(bundles);

            RegisterShared(bundles);

            RegisterAccount(bundles);

         //   RegisterHome(bundles);

         //   RegisterCharts(bundles);

         //RegisterWidgets(bundles);

         //   RegisterElements(bundles);

         //   RegisterForms(bundles);

          //  RegisterTables(bundles);

          //  RegisterCalendar(bundles);

          //  RegisterMailbox(bundles);

          //  RegisterExamples(bundles);

         //   RegisterDocumentation(bundles);
        }

        //private static void RegisterDocumentation(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/Scripts/Documentation/menu").Include(
        //        "~/Scripts/Documentation/Documentation-menu.js"));
        //}

        //private static void RegisterExamples(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/Blank/menu").Include(
        //        "~/Scripts/Examples/Blank-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/Invoice/menu").Include(
        //        "~/Scripts/Examples/Invoice-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/Lockscreen/menu").Include(
        //        "~/Scripts/Examples/Lockscreen-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/Login").Include(
        //        "~/Scripts/Examples/Login.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/Login/menu").Include(
        //        "~/Scripts/Examples/Login-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/P404/menu").Include(
        //        "~/Scripts/Examples/P404-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/P500/menu").Include(
        //        "~/Scripts/Examples/P500-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/Pace").Include(
        //        "~/Scripts/Examples/Pace.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/Pace/menu").Include(
        //        "~/Scripts/Examples/Pace-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/ProfileEx/menu").Include(
        //        "~/Scripts/Examples/ProfileEx-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/Register").Include(
        //        "~/Scripts/Examples/Register.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Examples/Register/menu").Include(
        //        "~/Scripts/Examples/Register-menu.js"));
        //}

        //private static void RegisterMailbox(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Inbox").Include(
        //        "~/Scripts/Mailbox/Inbox.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Inbox/menu").Include(
        //        "~/Scripts/Mailbox/Inbox-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Compose").Include(
        //        "~/Scripts/Mailbox/Compose.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Compose/menu").Include(
        //       "~/Scripts/Mailbox/Compose-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Mailbox/Read/menu").Include(
        //        "~/Scripts/Mailbox/Read-menu.js"));
        //}

        //private static void RegisterCalendar(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/Scripts/Calendar").Include(
        //        "~/Scripts/Calendar/Calendar.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Calendar/menu").Include(
        //        "~/Scripts/Calendar/Calendar-menu.js"));
        //}

        //private static void RegisterTables(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/Scripts/TablaExportar").Include(
        //      "~/Scripts/tablaexportar.js"));
        //}

        //private static void RegisterForms(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/Scripts/Forms/Advanced").Include(
        //        "~/Scripts/Forms/Advanced.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Forms/Advanced/menu").Include(
        //        "~/Scripts/Forms/Advanced-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Forms/Editors").Include(
        //        "~/Scripts/Forms/Editors.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Forms/Editors/menu").Include(
        //        "~/Scripts/Forms/Editors-menu.js"));


        //    bundles.Add(new ScriptBundle("~/Scripts/Forms/General/menu").Include(
        //        "~/Scripts/Forms/General-menu.js"));
        //}

        //private static void RegisterElements(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/Scripts/Elements/Buttons/menu").Include(
        //        "~/Scripts/Elements/Buttons-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Elements/General/menu").Include(
        //        "~/Scripts/Elements/General-menu.js"));

        //    bundles.Add(new StyleBundle("~/Styles/Elements/General").Include(
        //        "~/Styles/Elements/General.css"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Elements/Icons/menu").Include(
        //        "~/Scripts/Elements/Icons-menu.js"));

        //    bundles.Add(new StyleBundle("~/Styles/Elements/Icons").Include(
        //        "~/Styles/Elements/Icons.css"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Elements/Modals/menu").Include(
        //        "~/Scripts/Elements/Modals-menu.js"));

        //    bundles.Add(new StyleBundle("~/Styles/Elements/Modals").Include(
        //        "~/Styles/Elements/Modals.css"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Elements/Sliders").Include(
        //        "~/Scripts/Elements/Sliders.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Elements/Sliders/menu").Include(
        //        "~/Scripts/Elements/Sliders-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Elements/Timeline/menu").Include(
        //        "~/Scripts/Elements/Timeline-menu.js"));
        //}

        //private static void RegisterWidgets(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/Scripts/Widgets/menu").Include(
        //        "~/Scripts/Widgets/Widgets-menu.js"));
        //}

        //private static void RegisterCharts(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/Scripts/Charts/ChartsJs").Include(
        //        "~/Scripts/Charts/ChartsJs.js"));
        //    bundles.Add(new ScriptBundle("~/Scripts/Charts/ChartsJs/menu").Include(
        //                    "~/Scripts/Charts/ChartsJs-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Charts/Morris").Include(
        //        "~/Scripts/Charts/Morris.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Charts/Morris/menu").Include(
        //        "~/Scripts/Charts/Morris-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Charts/Flot").Include(
        //        "~/Scripts/Charts/Flot.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Charts/Flot/menu").Include(
        //        "~/Scripts/Charts/Flot-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Charts/Inline").Include(
        //        "~/Scripts/Charts/Inline.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Charts/Inline/menu").Include(
        //        "~/Scripts/Charts/Inline-menu.js"));
        //}

        //private static void RegisterHome(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/Scripts/Home/DashboardV1").Include(
        //        "~/Scripts/Home/DashboardV1.js"));
        //    bundles.Add(new ScriptBundle("~/Scripts/Home/DashboardV1/menu").Include(
        //        "~/Scripts/Home/DashboardV1-menu.js"));

        //    bundles.Add(new ScriptBundle("~/Scripts/Home/DashboardV2").Include(
        //        "~/Scripts/Home/DashboardV2.js"));
        //    bundles.Add(new ScriptBundle("~/Scripts/Home/DashboardV2/menu").Include(
        //        "~/Scripts/Home/DashboardV2-menu.js"));
        //}

        private static void RegisterAccount(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Account/Login").Include(
                "~/Scripts/Account/Login.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Account/Register").Include(
                "~/Scripts/Account/Register.js"));
        }

        private static void RegisterShared(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Shared/_Layout").Include(
                "~/Scripts/Shared/_Layout.js"));
        }

        private static void RegisterLayout(BundleCollection bundles)
        {
            // bootstrap
            bundles.Add(new ScriptBundle("~/AdminLTE/bootstrap/jsB").Include(
                "~/AdminLTE/bootstrap/js/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/bootstrap/cssB").Include(
                "~/AdminLTE/bootstrap/css/bootstrap.min.css"));

            // dist
            bundles.Add(new ScriptBundle("~/AdminLTE/dist/jsB").Include(
                "~/AdminLTE/dist/js/app.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/dist/cssB").Include(
                "~/AdminLTE/dist/css/admin-lte.min.css"));

            bundles.Add(new StyleBundle("~/AdminLTE/dist/css/skinsB").Include(
                "~/AdminLTE/dist/css/skins/_all-skins.min.css"));

            //// documentation
            //bundles.Add(new ScriptBundle("~/AdminLTE/documentation/jsB").Include(
            //    "~/AdminLTE/documentation/js/docs.js"));

            //bundles.Add(new StyleBundle("~/AdminLTE/documentation/cssB").Include(
            //    "~/AdminLTE/documentation/css/style.css"));

            // plugins | bootstrap-slider
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/bootstrap-slider/jsB").Include(
                                        "~/AdminLTE/plugins/bootstrap-slider/js/bootstrap-slider.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/bootstrap-slider/cssB").Include(
                                        "~/AdminLTE/plugins/bootstrap-slider/css/slider.css"));

            // plugins | bootstrap-wysihtml5
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/bootstrap-wysihtml5/jsB").Include(
                                         "~/AdminLTE/plugins/bootstrap-wysihtml5/js/bootstrap3-wysihtml5.all.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/bootstrap-wysihtml5/cssB").Include(
                                        "~/AdminLTE/plugins/bootstrap-wysihtml5/css/bootstrap3-wysihtml5.min.css"));

            //// plugins | chartjs
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/chartjs/jsB").Include(
            //                             "~/AdminLTE/plugins/chartjs/js/chart.min.js"));

            //// plugins | ckeditor
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/ckeditor/jsB").Include(
            //                             "~/AdminLTE/plugins/ckeditor/js/ckeditor.js"));

            //// plugins | colorpicker
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/colorpicker/jsB").Include(
            //                             "~/AdminLTE/plugins/colorpicker/js/bootstrap-colorpicker.min.js"));

            //bundles.Add(new StyleBundle("~/AdminLTE/plugins/colorpicker/cssB").Include(
            //                            "~/AdminLTE/plugins/colorpicker/css/bootstrap-colorpicker.css"));

            // plugins | datetables
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/datatables/jsB").Include(
                                         "~/AdminLTE/plugins/datatables/js/jquery.dataTables.min.js",
                                         "~/AdminLTE/plugins/datatables/js/dataTables.bootstrap.min.js",
                                         "~/AdminLTE/plugins/datatables/js/datatables.buttons.min.js",
                                         "~/AdminLTE/plugins/datatables/js/jszip.min.js",
                                         "~/AdminLTE/plugins/datatables/js/pdfmake.min.js",
                                         "~/AdminLTE/plugins/datatables/js/vfs_fonts.js",
                                         "~/AdminLTE/plugins/datatables/js/buttons.html5.min.js",
                                         "~/AdminLTE/plugins/datatables/js/buttons.print.min.js",
                                         "~/AdminLTE/plugins/datatables/js/datatables.tabletools.js",
                                         "~/AdminLTE/plugins/datatables/js/dataTables.responsive.min.js",
                                         "~/AdminLTE/plugins/datatables/js/responsive.bootstrap.min.js",
                                         "~/AdminLTE/plugins/datatables/js/jquery.dataTables.yadcf.js",
                                         "~/AdminLTE/plugins/datatables/js/buttons.colVis.min.js",
                                         "~/AdminLTE/plugins/datatables/js/dataTables.select.min.js",
                                         "~/Scripts/tabladetalles.js" ));
            
            bundles.Add(new StyleBundle("~/AdminLTE/plugins/datatables/cssB").Include(
                                        "~/AdminLTE/plugins/datatables/css/buttons.dataTables.min.css",
                                         "~/AdminLTE/plugins/datatables/css/jquery.dataTables_themeroller.css",
                                         "~/AdminLTE/plugins/datatables/css/responsive.bootstrap.min.css",
                                       "~/AdminLTE/plugins/datatables/css/jquery.dataTables.yadcf.css",
                                        "~/AdminLTE/plugins/datatables/css/dataTables.bootstrap.css",
                                         "~/AdminLTE/plugins/jquery-ui/css/datepicker.css"));

            // plugins | datepicker
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/datepicker/jsB").Include(
                                         "~/AdminLTE/plugins/datepicker/js/bootstrap-datepicker.js",
                                         "~/AdminLTE/plugins/datetimepicker/js/bootstrap-datetimepicker.js",
                                        "~/AdminLTE/plugins/datetimepicker/js/locales/bootstrap-datetimepicker.es.js" ));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/datepicker/cssB").Include(                                        
                                        "~/AdminLTE/plugins/datetimepicker/css/bootstrap-datetimepicker.css"));

            // plugins | daterangepicker
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/daterangepicker/jsB").Include(
                                         "~/AdminLTE/plugins/daterangepicker/js/moment.min.js",
                                         "~/AdminLTE/plugins/daterangepicker/js/daterangepicker.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/daterangepicker/cssB").Include(
                                        "~/AdminLTE/plugins/daterangepicker/css/daterangepicker-bs3.css"));

            //// plugins | fastclick
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/fastclick/jsB").Include(
            //                             "~/AdminLTE/plugins/fastclick/js/fastclick.min.js"));

            //// plugins | flot
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/flot/jsB").Include(
            //                             "~/AdminLTE/plugins/flot/js/jquery.flot.min.js",
            //                             "~/AdminLTE/plugins/flot/js/jquery.flot.resize.min.js",
            //                             "~/AdminLTE/plugins/flot/js/jquery.flot.pie.min.js",
            //                             "~/AdminLTE/plugins/flot/js/jquery.flot.categories.min.js"));

            // plugins | font-awesome
            bundles.Add(new StyleBundle("~/AdminLTE/plugins/font-awesome/cssB").Include(
                                        "~/AdminLTE/plugins/font-awesome/css/font-awesome.min.css"));

            // plugins | fullcalendar
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/fullcalendar/jsB").Include(
                                         "~/AdminLTE/plugins/fullcalendar/js/fullcalendar.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/fullcalendar/cssB").Include(
                                        "~/AdminLTE/plugins/fullcalendar/css/fullcalendar.css"));
                         
               bundles.Add(new StyleBundle("~/AdminLTE/plugins/fullcalendar/css/printB").Include(
                                        "~/AdminLTE/plugins/fullcalendar/css/print/fullcalendar.print.css"));

            //// plugins | icheck
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/icheck/jsB").Include(
                                         "~/AdminLTE/plugins/icheck/js/icheck.min.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/icheck/cssB").Include(
                                        "~/AdminLTE/plugins/icheck/css/all.css"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/icheck/css/flatB").Include(
                                        "~/AdminLTE/plugins/icheck/css/flat/flat.css"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/icheck/css/sqare/blueB").Include(
                                        "~/AdminLTE/plugins/icheck/css/sqare/blue.css"));

            // plugins | input-mask
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/input-mask/jsB").Include(
                                         "~/AdminLTE/plugins/input-mask/js/jquery.inputmask.js",
                                         "~/AdminLTE/plugins/input-mask/js/jquery.inputmask.date.extensions.js",
                                         "~/AdminLTE/plugins/input-mask/js/jquery.inputmask.extensions.js"));

            //// plugins | ionicons
            bundles.Add(new StyleBundle("~/AdminLTE/plugins/ionicons/cssB").Include(
                                        "~/AdminLTE/plugins/ionicons/css/ionicons.min.css"));

            //// plugins | ionslider
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/ionslider/jsB").Include(
            //                             "~/AdminLTE/plugins/ionslider/js/ion.rangeSlider.min.js"));

            //bundles.Add(new StyleBundle("~/AdminLTE/plugins/ionslider/cssB").Include(
            //                            "~/AdminLTE/plugins/ionslider/css/ion.rangeSlider.css",
            //                            "~/AdminLTE/plugins/ionslider/css/ion.rangeSlider.skinNice.css"));

            // plugins | jquery
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/jquery/jsB").Include(
                                         "~/AdminLTE/plugins/jquery/js/jQuery-2.1.4.min.js"));

            // plugins | jquery-validate
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/jquery-validate/jsB").Include(
                                         "~/AdminLTE/plugins/jquery-validate/js/jquery.validate*"));

            // plugins | jquery-ui
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/jquery-ui/jsB").Include(
                                         "~/AdminLTE/plugins/jquery-ui/js/jquery-ui.min.js",
                                         "~/AdminLTE/plugins/jquery-ui/js/datepicker-es.js"));

            bundles.Add(new StyleBundle("~/AdminLTE/plugins/jquery-ui/cssB").Include(
                                        "~/AdminLTE/plugins/jquery/css/jquery-ui.css",                                        
                                        "~/AdminLTE/plugins/jquery/css/jquery-ui.min.css"));

            //// plugins | jvectormap
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/jvectormap/jsB").Include(
            //                             "~/AdminLTE/plugins/jvectormap/js/jquery-jvectormap-1.2.2.min.js",
            //                             "~/AdminLTE/plugins/jvectormap/js/jquery-jvectormap-world-mill-en.js"));

            //bundles.Add(new StyleBundle("~/AdminLTE/plugins/jvectormap/cssB").Include(
            //                            "~/AdminLTE/plugins/jvectormap/css/jquery-jvectormap-1.2.2.css"));

            //// plugins | knob
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/knob/jsB").Include(
            //                             "~/AdminLTE/plugins/knob/js/jquery.knob.js"));

            //// plugins | morris
            //bundles.Add(new StyleBundle("~/AdminLTE/plugins/morris/cssB").Include(
            //                            "~/AdminLTE/plugins/morris/css/morris.css"));

            // plugins | momentjs
            bundles.Add(new ScriptBundle("~/AdminLTE/plugins/momentjs/jsB").Include(
                                         "~/AdminLTE/plugins/momentjs/js/moment.min.js"));

            //// plugins | pace
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/pace/jsB").Include(
            //                             "~/AdminLTE/plugins/pace/js/pace.min.js"));

            //bundles.Add(new StyleBundle("~/AdminLTE/plugins/pace/cssB").Include(
            //                            "~/AdminLTE/plugins/pace/css/pace.min.css"));

            //// plugins | slimscroll
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/slimscroll/jsB").Include(
            //                             "~/AdminLTE/plugins/slimscroll/js/jquery.slimscroll.min.js"));

            //// plugins | sparkline
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/sparkline/jsB").Include(
            //                             "~/AdminLTE/plugins/sparkline/js/jquery.sparkline.min.js"));

            //// plugins | timepicker
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/timepicker/jsB").Include(
            //                             "~/AdminLTE/plugins/timepicker/js/bootstrap-timepicker.min.js"));

            //bundles.Add(new StyleBundle("~/AdminLTE/plugins/timepicker/cssB").Include(
            //                            "~/AdminLTE/plugins/timepicker/css/bootstrap-timepicker.min.css"));

            //// plugins | raphael
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/raphael/jsB").Include(
            //                             "~/AdminLTE/plugins/raphael/js/raphael-min.js"));

            //// plugins | select2
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/select2/jsB").Include(
            //                             "~/AdminLTE/plugins/select2/js/select2.full.min.js"));

            //bundles.Add(new StyleBundle("~/AdminLTE/plugins/select2/cssB").Include(
            //                            "~/AdminLTE/plugins/select2/css/select2.min.css"));

            //// plugins | morris
            //bundles.Add(new ScriptBundle("~/AdminLTE/plugins/morris/jsB").Include(
            //                             "~/AdminLTE/plugins/morris/js/morris.min.js"));

        }
    }
}
