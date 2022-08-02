﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.IsuranceMst
{
    public interface IInsuranceMstRepository
    {
        public InsuranceMstModel GetDataInsuranceMst(int hpId, long ptId, int sinDate, int hokenId);

        public IEnumerable<HokensyaMstModel> SearchListDataHokensyaMst(int hpId, int pageIndex, int pageCount, int sinDate, string keyword);
    }
}
