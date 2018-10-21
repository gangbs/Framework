using System.Web;
using System.Web.Optimization;

namespace WebAppTest
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region rewritrCssUrl

            bundles.Add(new StyleBundle("~/bundles/easyui/css/themes").Include(
                "~/Content/jquery-easyui-1.5.5.6/themes/default/easyui.css"),new CssRewriteUrlTransform());//

            bundles.Add(new StyleBundle("~/bundles/easyui/css/icon").Include(
                "~/Content/jquery-easyui-1.5.5.6/themes/icon.css"), new CssRewriteUrlTransform());//


            //"~/Content/jquery-easyui-1.5.5.6/demo/demo.css""~/Content/jquery-easyui-1.5.5.6/themes/icon.css",


            bundles.Add(new ScriptBundle("~/bundles/easyui/js").Include(
                "~/Content/jquery-easyui-1.5.5.6/jquery.min.js",
                "~/Content/jquery-easyui-1.5.5.6/jquery.easyui.min.js"
                ));

            #endregion




            #region

            bundles.UseCdn = true;
            var cdnJq = new ScriptBundle("~/bundlesTest/jquery", "https://cdn.bootcss.com/jquery/3.3.1/jquery.js").Include(
                "~/Scripts/jquery-1.10.2.js"
                );

            cdnJq.CdnFallbackExpression = "window.jQuery";

            bundles.Add(cdnJq);

            #endregion



            #region

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            #endregion
           
        }
    }
}
