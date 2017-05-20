using System.Web.Http;

namespace ChilindoBankLtd
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "AccountBalance",
            //    routeTemplate: "api/account/balance",
            //    defaults: new { controller = "account"}
            //);

            //config.Routes.MapHttpRoute(
            //     name: "Deposit",
            //     routeTemplate: "api/account/deposit",
            //     defaults: new { controller = "account" }
            // );

            //config.Routes.MapHttpRoute(
            //     name: "Widthraw",
            //     routeTemplate: "api/account/widthraw",
            //     defaults: new { controller = "account" }
            // );

            //config.Routes.MapHttpRoute(
            //    name: "Deposit",
            //    routeTemplate: "api/account/deposit/{accountNumber}/{amount}/{currency}",
            //    defaults: new { controller = "account" }
            //);

        }
    }
}
