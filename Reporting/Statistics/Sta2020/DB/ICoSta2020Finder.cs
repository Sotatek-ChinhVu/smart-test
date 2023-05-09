﻿using Reporting.Statistics.Model;
using Reporting.Statistics.Sta2020.Models;

namespace Reporting.Statistics.Sta2020.DB
{
    public interface ICoSta2020Finder
    {
        List<CoSinKouiModel> GetSinKouis(int hpId, CoSta2020PrintConf printConf);

        CoHpInfModel GetHpInf(int hpId, int sinDate);
    }
}
