﻿using FreeWheeling.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeWheeling.Domain.Abstract
{
    public interface ICycleRepository
    {

        //Group
        IEnumerable<Group> GetGroups();
        IEnumerable<Group> GetGroupsWithRiders();
        Group GetGroupByID(int id);
        Ride GetRideByID(int id);
        List<Rider> GetRidersForRide(int id);

        //Member
        void AddMember(string UserId, Group _Group);

        void AddRider(string UserId, string RiderName, Ride _Ride, Group _Group, string Percent);

        void Save();
       

    }
}
