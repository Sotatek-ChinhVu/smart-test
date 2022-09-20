﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MonshinInf
{
    public interface IMonshinInforRepository
    {
        public List<MonshinInforModel> MonshinInforModels(int hpId, long ptId);

        void SaveList(List<MonshinInforModel> monshinInforModels);
    }
}
