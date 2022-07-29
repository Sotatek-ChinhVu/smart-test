﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.GroupInf
{
    public class GroupInfModel
    {
        public GroupInfModel(int hpPt, long ptId, int groupId, string groupCode, string groupName)
        {
            HpPt = hpPt;
            PtId = ptId;
            GroupId = groupId;
            GroupCode = groupCode;
            GroupName = groupName;
        }

        public int HpPt { get; private set; }

        public long PtId { get; private set; }

        public int GroupId { get; private set; }

        public string GroupCode { get; private set; }

        public string GroupName { get; private set; }
    }
}
