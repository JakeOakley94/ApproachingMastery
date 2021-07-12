using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseInteraction.Models;

namespace ApproachingMastery.Models
{
    public class AddEditAirTestModal
    {
        public AirTest Test { get; set; } = new AirTest();

        public bool IsEditing { get; set; } = false;

        public AddEditAirTestModal(AirTest test)
        {
            Test = test;
        }

        public AddEditAirTestModal()
        {

        }




    }
}