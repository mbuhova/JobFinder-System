using JobFinder.Data;
using JobFinder.Models;
using JobFinder.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using JobFinder.Web.Models;
using PagedList;

namespace JobFinder.Web.Areas.Person.Controllers
{
    [Authorize(Roles="Person")]

    public class FollowedOffersController : BaseController
    {
        private const int OffersPerPage = 5;

        public FollowedOffersController(IJobFinderData data) : base(data)
        {

        }

        // GET: Person/FollowedOffers
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Follow(int? id)
        {
            JobOffer offer = this.data.JobOffers.Find((int)id);
            string personId = this.User.Identity.GetUserId();
            JobFinder.Models.Person currentUser = this.data.People.Find(personId);

            if (offer.PeopleFollowing.Contains(currentUser))
            {
                offer.PeopleFollowing.Remove(currentUser);
            }
            else
            {
                offer.PeopleFollowing.Add(currentUser);
            }

            this.data.SaveChanges();

            return new EmptyResult();
        }

        public ActionResult GetFollowedOffers(int? page)
        {
            string currentUser = this.User.Identity.GetUserId();
            IEnumerable<SearchResultOfferViewModel> model = this.data.People.Find(currentUser)
                .FollowedOffers.AsQueryable().Select(SearchResultOfferViewModel.FromJobOffer)
                .OrderByDescending(o => o.DateCreated);

            int pageNumber = page ?? 1;
            model = model.ToPagedList(pageNumber, OffersPerPage);
            return View(model);
        }
    }
}