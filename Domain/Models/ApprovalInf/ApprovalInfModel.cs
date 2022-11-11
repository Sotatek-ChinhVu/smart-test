using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ApprovalInf
{
    public class ApprovalInfModel
    {
        public ApprovalInfModel(int starDate, int endDate, int kaId, int tantoId)
        {
            starDate = starDate;
            endDate = endDate;
            kaId = kaId;
            tantoId = tantoId;
        }
        public int starDate { get; private set; }
        public int endDate { get; private set; }
        public int kaId { get; private set; }
        public int tantoId { get; private set; }
    }
}
