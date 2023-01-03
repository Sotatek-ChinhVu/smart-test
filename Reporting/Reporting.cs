﻿using Reporting.Interface;
using Reporting.Model.ExportKarte1;

namespace Reporting;

public class Reporting : IReporting
{
    private readonly IExportKarte1 _exportKarte1;

    public Reporting(IExportKarte1 exportKarte1)
    {
        _exportKarte1 = exportKarte1;
    }

    public Karte1ExportModel GetDataKarte1(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei)
    {
        return _exportKarte1.GetDataKarte1(hpId, ptId, sinDate, hokenPid, tenkiByomei);
    }
}
