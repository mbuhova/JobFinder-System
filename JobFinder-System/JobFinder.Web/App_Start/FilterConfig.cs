﻿using System.Web;
using System.Web.Mvc;

namespace JobFinder.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute { View = "Error" });
            //filters.Add(new HandleErrorAttribute ());
        }
    }
}
