﻿using Reporting.Sokatu.AfterCareSeikyu.Model;
using Reporting.Sokatu.Common.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.AfterCareSeikyu.DB;

public interface ICoAfterCareSeikyuFinder
{
    CoSeikyuInfModel GetSeikyuInf(int hpId, int seikyuYm, SeikyuType seikyuType);

    CoHpInfModel GetHpInf(int hpId, int seikyuYm);
}
