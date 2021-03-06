﻿using FreeWheeling.Domain.Abstract;
using FreeWheeling.UI.DataContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FreeWheeling.Domain.Entities;
using System.ComponentModel;

namespace FreeWheeling.UI.Models
{

    public class UserExpandHelper
    { 
    
        private IdentityDb idb = new IdentityDb();

        private ICycleRepository repository;

        public UserExpandHelper(ICycleRepository repoParam)
        {

            repository = repoParam;

        }

        public void UpdateOrInsertUserExpand(UserExpandModel _UserExpandModel)
        {

        //IDs for collapse user remember
        // 1 = FirstBunchCollapse
        // 2 = FirstKeen
        // 3 = FirstComment
        // 4 = SecondBunchCollapse
        // 5 = SecondKeen
        // 6 = SecondComment

            UserExpand CurrentUserExpand = repository.GetUserExpandByUserID(_UserExpandModel.userid);

            if (CurrentUserExpand == null)
            {

                if (_UserExpandModel.Id == 1)
                {

                    UserExpand _UserExpand = new UserExpand { userId = _UserExpandModel.userid,  FirstBunch = _UserExpandModel.collapsed };
                    repository.AddUserExpand(_UserExpand);
                    repository.Save();
                }

                if (_UserExpandModel.Id == 2 )
                {

                    UserExpand _UserExpand = new UserExpand { userId = _UserExpandModel.userid, FirstKeen = _UserExpandModel.collapsed };
                    repository.AddUserExpand(_UserExpand);
                    repository.Save();
                }

                if (_UserExpandModel.Id == 3)
                {

                    UserExpand _UserExpand = new UserExpand { userId = _UserExpandModel.userid, FirstComment = _UserExpandModel.collapsed };
                    repository.AddUserExpand(_UserExpand);
                    repository.Save();
                }

                //Second Ride

                if (_UserExpandModel.Id == 4)
                {

                    UserExpand _UserExpand = new UserExpand { userId = _UserExpandModel.userid, SecondBunch = _UserExpandModel.collapsed };
                    repository.AddUserExpand(_UserExpand);
                    repository.Save();
                }

                if (_UserExpandModel.Id == 5)
                {

                    UserExpand _UserExpand = new UserExpand { userId = _UserExpandModel.userid, SecondKeen = _UserExpandModel.collapsed };
                    repository.AddUserExpand(_UserExpand);
                    repository.Save();
                }

                if (_UserExpandModel.Id == 6)
                {

                    UserExpand _UserExpand = new UserExpand { userId = _UserExpandModel.userid, SecondComment = _UserExpandModel.collapsed };
                    repository.AddUserExpand(_UserExpand);
                    repository.Save();
                }


            }
            else
            {

                if (_UserExpandModel.Id == 1)
                {
                    UserExpand _UserExpand = new UserExpand
                    {
                        userId = _UserExpandModel.userid,
                        FirstKeen = CurrentUserExpand.FirstKeen,
                        FirstComment = CurrentUserExpand.FirstComment,
                        FirstBunch = _UserExpandModel.collapsed,
                        SecondBunch = CurrentUserExpand.SecondBunch,
                        SecondKeen = CurrentUserExpand.SecondKeen,
                        SecondComment = CurrentUserExpand.SecondComment
                    };

                    repository.UpdateUserExpand(_UserExpand);
                    repository.Save();
                }

                if (_UserExpandModel.Id == 2)
                {
                    UserExpand _UserExpand = new UserExpand 
                    { 
                        userId = _UserExpandModel.userid,
                        FirstKeen = _UserExpandModel.collapsed ,
                        FirstComment = CurrentUserExpand.FirstComment,
                        FirstBunch = CurrentUserExpand.FirstBunch,
                        SecondBunch = CurrentUserExpand.SecondBunch,
                        SecondKeen = CurrentUserExpand.SecondKeen,
                        SecondComment = CurrentUserExpand.SecondComment
                    };

                    repository.UpdateUserExpand(_UserExpand);
                    repository.Save();
                }

                if (_UserExpandModel.Id == 3)
                {

                    UserExpand _UserExpand = new UserExpand
                    {
                        userId = _UserExpandModel.userid,
                        FirstKeen = CurrentUserExpand.FirstKeen,
                        FirstComment = _UserExpandModel.collapsed,
                        FirstBunch = CurrentUserExpand.FirstBunch,
                        SecondBunch = CurrentUserExpand.SecondBunch,
                        SecondKeen = CurrentUserExpand.SecondKeen,
                        SecondComment = CurrentUserExpand.SecondComment
                    };

                    repository.UpdateUserExpand(_UserExpand);
                    repository.Save();
                }

                //Second Ride

                if (_UserExpandModel.Id == 4)
                {
                    UserExpand _UserExpand = new UserExpand
                    {
                        userId = _UserExpandModel.userid,
                        FirstKeen = CurrentUserExpand.FirstKeen,
                        FirstComment = CurrentUserExpand.FirstComment,
                        FirstBunch = CurrentUserExpand.FirstBunch,
                        SecondBunch = _UserExpandModel.collapsed,
                        SecondKeen = CurrentUserExpand.SecondKeen,
                        SecondComment = CurrentUserExpand.SecondComment
                    };

                    repository.UpdateUserExpand(_UserExpand);
                    repository.Save();
                }

                if (_UserExpandModel.Id == 5)
                {
                    UserExpand _UserExpand = new UserExpand
                    {
                        userId = _UserExpandModel.userid,
                        FirstKeen = CurrentUserExpand.FirstKeen,
                        FirstComment = CurrentUserExpand.FirstComment,
                        FirstBunch = CurrentUserExpand.FirstBunch,
                        SecondBunch = CurrentUserExpand.SecondBunch,
                        SecondKeen = _UserExpandModel.collapsed,
                        SecondComment = CurrentUserExpand.SecondComment
                    };

                    repository.UpdateUserExpand(_UserExpand);
                    repository.Save();
                }

                if (_UserExpandModel.Id == 6)
                {

                    UserExpand _UserExpand = new UserExpand
                    {
                        userId = _UserExpandModel.userid,
                        FirstKeen = CurrentUserExpand.FirstKeen,
                        FirstComment = CurrentUserExpand.FirstComment,
                        FirstBunch = CurrentUserExpand.FirstBunch,
                        SecondBunch = CurrentUserExpand.SecondBunch,
                        SecondKeen = CurrentUserExpand.SecondKeen,
                        SecondComment = _UserExpandModel.collapsed
                    };

                    repository.UpdateUserExpand(_UserExpand);
                    repository.Save();
                }

            }

        }
       
    }

    public class UserModels
    {
    }

    public class UserExpandModel
    {
        [Key]
        public int Id { get; set; }
        public bool collapsed { get; set; }
        public string userid { get; set; }
    }

    public class FeedBackModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        [DisplayName("Sum")]
        public int SumValue { get; set; }
    }
}