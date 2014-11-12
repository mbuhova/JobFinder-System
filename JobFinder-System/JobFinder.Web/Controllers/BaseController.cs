using JobFinder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using JobFinder.Models;

namespace JobFinder.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IJobFinderData data;

        public BaseController(IJobFinderData data)
        {
            this.data = data;                     
        }
    }
}