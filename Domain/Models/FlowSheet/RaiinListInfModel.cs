﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.FlowSheet
{
    public class RaiinListInfModel
    {
        public long RaiinNo { get; private set; }

        public int GrpId { get; private set; }
        
        public int KbnCd { get; private set; }
        
        public int RaiinListKbn { get; private set; }
        
        public RaiinListInfModel(long raiinNo, int grpId, int kbnCd, int raiinListKbn)
        {
            RaiinNo = raiinNo;
            GrpId = grpId;
            KbnCd = kbnCd;
            RaiinListKbn = raiinListKbn;
        }
    }
}
