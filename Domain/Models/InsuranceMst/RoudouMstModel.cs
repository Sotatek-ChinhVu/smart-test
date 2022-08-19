using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InsuranceMst
{
    public class RoudouMstModel
    {
        public RoudouMstModel(string roudouCD, string roudouName)
        {
            RoudouCD = roudouCD;
            RoudouName = roudouName;
        }

        public string RoudouCD { get; private set; }

        public string RoudouName { get; private set; }

        public string RoudouDisplay { get { return RoudouCD + " " + RoudouName; } }

    }
}
