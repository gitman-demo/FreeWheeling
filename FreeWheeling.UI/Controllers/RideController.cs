﻿using FreeWheeling.Domain.Abstract;
using FreeWheeling.Domain.Entities;
using FreeWheeling.UI.DataContexts;
using FreeWheeling.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace FreeWheeling.UI.Controllers
{
    public class RideController : Controller
    {
        private IdentityDb idb = new IdentityDb(); 

        private ICycleRepository repository;

        public RideController(ICycleRepository repoParam)
        {

            repository = repoParam;

        }

        public ActionResult Index(int groupid, int rideid = -1)
        {
            //var TimeZone = TimeZoneInfo.Local.Id;
            RideModelIndex RideModel = new RideModelIndex();

            Group _Group = repository.GetGroupByID(groupid);

            if (rideid == -1)
            {
                RideModel.Ride = _Group.Rides.Where(u => u.RideDate >= DateTime.Now).OrderBy(i => i.RideDate).FirstOrDefault();
                RideModel.NextRide = _Group.Rides.Where(u => u.RideDate > RideModel.Ride.RideDate).OrderBy(i => i.RideDate).FirstOrDefault();
                RideModel.Comments = repository.GetCommentsForRide(RideModel.Ride.id);
                RideModel.RideDate = RideModel.Ride.RideDate;
            }
            else
            {
                int _rideid = rideid;
                RideModel.Ride = repository.GetRideByID(rideid);
                RideModel.NextRide = _Group.Rides.Where(u => u.RideDate > RideModel.Ride.RideDate).OrderBy(i => i.RideDate).FirstOrDefault();
                RideModel.Comments = repository.GetCommentsForRide(RideModel.Ride.id);
                RideModel.RideDate = RideModel.Ride.RideDate;
            }

            if (RideModel.Ride != null)
            {
                RideModel.Group = _Group;
                RideModel.Riders = repository.GetRidersForRide(RideModel.Ride.id);
                RideModel.Comments = repository.GetCommentsForRide(RideModel.Ride.id);

                TimeZoneInfo EST = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");

                foreach (Rider r in RideModel.Riders)
                {

                    r.LeaveTime = TimeZoneInfo.ConvertTimeFromUtc(r.LeaveTime, EST);

                }

            }
            else
            {         
                GroupModel GroupModel = new GroupModel();
                GroupModel._Groups = repository.GetGroups().ToList();  
                var currentUser = idb.Users.Find(User.Identity.GetUserId());
                GroupModel.CurrentGroupMembership = repository.CurrentGroupsForUser(currentUser.Id);
                return RedirectToAction("index", "group", GroupModel);
            }

            return View(RideModel);
        }

        public ActionResult AddHocList()
        {
            var currentUser = idb.Users.Find(User.Identity.GetUserId());
            Member _Member = repository.GetMemberByUserID(currentUser.Id);
            AdHocRidesModel _AdHocRidesModel = new AdHocRidesModel();
            _AdHocRidesModel._Ad_HocRide = repository.GetAdHocRides(repository.GetLocations().Where(o => o.id == currentUser.LocationID).FirstOrDefault());

            return View(_AdHocRidesModel);
        }

        public ActionResult ViewAdHocRide(int adhocrideid)
        {
            Ad_HocRide Ah = repository.GetAdHocRideByID(adhocrideid);
            AdHocViewModel adHocViewModel = new AdHocViewModel { Ride = Ah, RideDate = Ah.RideDate, RideTime = Ah.RideTime,  };
            adHocViewModel.Riders = repository.GetRidersForAdHocRide(adhocrideid);
            adHocViewModel.Comments = repository.GetCommentsForAdHocRide(adhocrideid);

            return View(adHocViewModel);
        }

        public ActionResult NextRide(int RideId, int Groupid, int PreviousRideID)
        {

            RideModelIndex RideModel = new RideModelIndex();

            Group _Group = repository.GetGroupByID(Groupid);

            RideModel.PreviousRide = repository.GetRideByID(PreviousRideID);

            RideModel.Ride = repository.GetRideByID(RideId);

            if (RideModel.Ride != null)
            {

                RideModel.Group = _Group;
                RideModel.Riders = repository.GetRidersForRide(RideModel.Ride.id);
                RideModel.Comments = repository.GetCommentsForRide(RideModel.Ride.id);

            }
            else
            {

                GroupModel GroupModel = new GroupModel();
                GroupModel._Groups = repository.GetGroups().ToList();
                var currentUser = idb.Users.Find(User.Identity.GetUserId());
                GroupModel.CurrentGroupMembership = repository.CurrentGroupsForUser(currentUser.Id);
                return RedirectToAction("index", "group", GroupModel);

            }

            return View(RideModel);

        }

        public ActionResult AddAdHocComment(int adhocrideid)
        {

            AdHocRideCommentModel _RideCommentModel = new AdHocRideCommentModel();
            _RideCommentModel.adhocrideid = adhocrideid;

            _RideCommentModel.Ride = repository.GetAdHocRideByID(adhocrideid);

            return View(_RideCommentModel);

        }

        [HttpPost]
        public ActionResult AddAdHocComment(AdHocRideCommentModel RideComment)
        {
            var currentUser = idb.Users.Find(User.Identity.GetUserId());
            repository.AddAdHocRideComment(RideComment.Comment, RideComment.adhocrideid, currentUser.UserName);
            repository.Save();

            Ad_HocRide Ah = repository.GetAdHocRideByID(RideComment.adhocrideid);
            AdHocViewModel adHocViewModel = new AdHocViewModel { Ride = Ah, RideDate = Ah.RideDate, RideTime = Ah.RideTime, };
            adHocViewModel.Riders = repository.GetRidersForAdHocRide(RideComment.adhocrideid);
            adHocViewModel.Comments = repository.GetCommentsForAdHocRide(RideComment.adhocrideid);

            return View("ViewAdHocRide", adHocViewModel);

        }

        public ActionResult AddComment(int groupid, int RideId)
        {
            RideCommentModel _RideCommentModel = new RideCommentModel();
            _RideCommentModel.RideId = RideId;
            _RideCommentModel.GroupId = groupid;
            

            _RideCommentModel.Ride = repository.GetRideByID(RideId);

            return View(_RideCommentModel);

        }

        [HttpPost]
        public ActionResult AddComment(RideCommentModel RideComment)
        {
            var currentUser = idb.Users.Find(User.Identity.GetUserId());
            repository.AddRideComment(RideComment.Comment, RideComment.RideId, currentUser.UserName);
            repository.Save();

            RideModelIndex RideModel = new RideModelIndex();
            RideModel.Ride = repository.GetRideByID(RideComment.RideId);
            RideModel.Group = repository.GetGroupByID(RideComment.GroupId);
            RideModel.NextRide = RideModel.Group.Rides.Where(u => u.RideDate > RideModel.Ride.RideDate).OrderBy(i => i.RideDate).FirstOrDefault();
            RideModel.Riders = repository.GetRidersForRide(RideModel.Ride.id);
            RideModel.Comments = repository.GetCommentsForRide(RideModel.Ride.id);

            return View("Index",RideModel);

        }

        public ActionResult AddCommentNext(int groupid, int RideId, int PreviousRideID)
        {
            RideCommentModel _RideCommentModel = new RideCommentModel();
            _RideCommentModel.RideId = RideId;
            _RideCommentModel.GroupId = groupid;
            _RideCommentModel.PreviousRide = repository.GetRideByID(PreviousRideID);

            _RideCommentModel.Ride = repository.GetRideByID(RideId);

            return View("AddCommentNext",_RideCommentModel);

        }

        [HttpPost]
        public ActionResult AddCommentNext(RideCommentModel RideComment)
        {
            var currentUser = idb.Users.Find(User.Identity.GetUserId());
            repository.AddRideComment(RideComment.Comment, RideComment.RideId, currentUser.UserName);
            repository.Save();

            RideModelIndex RideModel = new RideModelIndex();
            RideModel.Ride = repository.GetRideByID(RideComment.RideId);
            RideModel.Group = repository.GetGroupByID(RideComment.GroupId);
            RideModel.Riders = repository.GetRidersForRide(RideModel.Ride.id);
            RideModel.Comments = repository.GetCommentsForRide(RideModel.Ride.id);
            RideModel.PreviousRide = repository.GetPreviousRideForGroup(RideModel.Group);

            return View("NextRide", RideModel);

        }

        public ActionResult Attend(int RideId, string Commitment, int Groupid)
        {
            var currentUser = idb.Users.Find(User.Identity.GetUserId());
            Ride _Ride = new Ride();
            Group _Group = new Group();


            _Ride = repository.GetRideByID(RideId);
            _Group = repository.GetGroupByID(Groupid);

            Rider _Rider = new Rider { userId = currentUser.Id, Name = currentUser.UserName, Ride = _Ride, LeaveTime = DateTime.UtcNow, PercentKeen = Commitment };
                
            repository.AddRider(_Rider, _Group);
            repository.Save();

            RideModelIndex RideModel = new RideModelIndex();
           
            RideModel.Ride = _Ride;
            RideModel.RideDate = RideModel.Ride.RideDate;
            RideModel.NextRide = _Group.Rides.Where(u => u.RideDate > RideModel.Ride.RideDate).OrderBy(i => i.RideDate).FirstOrDefault();
            RideModel.Group = _Group;
            RideModel.Comments = repository.GetCommentsForRide(RideModel.Ride.id);
            RideModel.Riders = repository.GetRidersForRide(RideModel.Ride.id);

            TimeZoneInfo EST = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");

            foreach (Rider r in RideModel.Riders)
            {

                r.LeaveTime = TimeZoneInfo.ConvertTimeFromUtc(r.LeaveTime, EST);

            }


            return View("Index", RideModel);
        }

        public ActionResult AttendAdHocRider(int adhocrideid, string Commitment)
        {
            var currentUser = idb.Users.Find(User.Identity.GetUserId());
            Ad_HocRide _Ride = new Ad_HocRide();
            Group _Group = new Group();


            _Ride = repository.GetAdHocRideByID(adhocrideid);

            AdHocRider _Rider = new AdHocRider { userId = currentUser.Id, Name = currentUser.UserName, AdHocRide = _Ride, LeaveTime = DateTime.UtcNow, PercentKeen = Commitment };

            repository.AddAdHocRider(_Rider, _Ride);
            repository.Save();

            Ad_HocRide Ah = repository.GetAdHocRideByID(adhocrideid);
            AdHocViewModel adHocViewModel = new AdHocViewModel { Ride = Ah, RideDate = Ah.RideDate, RideTime = Ah.RideTime, };
            adHocViewModel.Riders = repository.GetRidersForAdHocRide(adhocrideid);
            adHocViewModel.Comments = repository.GetCommentsForAdHocRide(adhocrideid);

            return View("ViewAdHocRide", adHocViewModel);
        }

        public ActionResult AttendNext(int RideId, string Commitment, int Groupid, int PreviousRideID)
        {
            var currentUser = idb.Users.Find(User.Identity.GetUserId());
            Ride _Ride = new Ride();
            Group _Group = new Group();

            _Ride = repository.GetRideByID(RideId);
            _Group = repository.GetGroupByID(Groupid);

            Rider _Rider = new Rider { userId = currentUser.Id, Name = currentUser.UserName, Ride = _Ride, LeaveTime = DateTime.UtcNow, PercentKeen = Commitment };

            repository.AddRider(_Rider, _Group);
            repository.Save();

            RideModelIndex RideModel = new RideModelIndex();
            RideModel.Ride = _Ride;
            RideModel.Group = _Group;
            RideModel.Riders = repository.GetRidersForRide(RideModel.Ride.id);
            RideModel.Comments = repository.GetCommentsForRide(RideModel.Ride.id);
            RideModel.PreviousRide = repository.GetRideByID(PreviousRideID);
            

            return View("NextRide", RideModel);
        }

       
	}
}