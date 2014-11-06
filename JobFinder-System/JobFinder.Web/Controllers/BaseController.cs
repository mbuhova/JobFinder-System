using JobFinder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobFinder.Web.Controllers
{
    public class BaseController : Controller
    {
       protected IJobFinderData data;

       public BaseController()
        {
            this.data = new JobFinderData(new JobFinderDbContext());
        }
    }
}