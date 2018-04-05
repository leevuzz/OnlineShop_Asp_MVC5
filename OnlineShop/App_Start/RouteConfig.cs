using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Product Category
            routes.MapRoute(
                name: "Product Category",
                url: "san-pham/{metaTitle}-{cateId}",
                defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "OnlineShop.Controllers" }
            );
          
            //Product Detail
            routes.MapRoute(
                name: "Product Detail",
                url: "chi-tiet/{metaTitle}-{id}",
                defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "OnlineShop.Controllers" }
            );
            //Search
            routes.MapRoute(
                name: "Search",
                url: "tim-kiem",
                defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                namespaces: new[] { "OnlineShop.Controllers" }
            );
            //About
            routes.MapRoute(
                name: "About",
                url: "gioi-thieu",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OnlineShop.Controllers" }
            );
            //Contact
            routes.MapRoute(
               name: "Contact",
               url: "lien-he",
               defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "OnlineShop.Controllers" }
           );
            //Cart
            routes.MapRoute(
              name: "Cart",
              url: "gio-hang",
              defaults: new { controller = "CartItem", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "OnlineShop.Controllers" }
          );
            //Payment
            routes.MapRoute(
             name: "Payment",
             url: "thanh-toan",
             defaults: new { controller = "CartItem", action = "Payment", id = UrlParameter.Optional },
             namespaces: new[] { "OnlineShop.Controllers" }
         );
            //Add Cart
            routes.MapRoute(
               name: "Add Cart",
               url: "them-gio-hang",
               defaults: new { controller = "CartItem", action = "AddCart", id = UrlParameter.Optional },
               namespaces: new[] { "OnlineShop.Controllers" }
           );
            //Payment Success
            routes.MapRoute(
               name: "Payment Success",
               url: "hoan-thanh",
               defaults: new { controller = "CartItem", action = "Success", id = UrlParameter.Optional },
               namespaces: new[] { "OnlineShop.Controllers" }
           );
            //Register
            routes.MapRoute(
               name: "Register",
               url: "dang-ky",
               defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
               namespaces: new[] { "OnlineShop.Controllers" }
           );
            //Login
            routes.MapRoute(
              name: "Login",
              url: "dang-nhap",
              defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
              namespaces: new[] { "OnlineShop.Controllers" }
          );
            //defautl
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OnlineShop.Controllers" }
            );
        }
    }
}
