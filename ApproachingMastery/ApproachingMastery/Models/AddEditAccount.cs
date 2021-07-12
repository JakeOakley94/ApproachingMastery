using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseInteraction.Models;
namespace ApproachingMastery.Models
{
    public class AddEditAccount
    {
        public bool IsEditing = false;
 
        public string ExistingPassword { get; set; }
        public UserLogin UserLogin { get; set; }
        public User UserInformation { get; set; }



    }
}