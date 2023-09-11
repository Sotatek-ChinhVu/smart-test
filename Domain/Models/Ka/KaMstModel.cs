﻿namespace Domain.Models.Ka;

public class KaMstModel
{
    public KaMstModel()
    {
        Id = 0;
        KaId = 0;
        SortNo = 0;
        ReceKaCd = string.Empty;
        KaSname = string.Empty;
        KaName = string.Empty;
        YousikiKaCd =  String.Empty;
    }
    public KaMstModel(long id, int kaId, int sortNo, string receKaCd, string kaSname, string kaName, string yousikiKaCd)
    {
        Id = id;
        KaId = kaId;
        SortNo = sortNo;
        ReceKaCd = receKaCd;
        KaSname = kaSname;
        KaName = kaName;
        YousikiKaCd = yousikiKaCd;
    }

    public long Id { get; private set; }

    public int KaId { get; private set; }

    public int SortNo { get; private set; }

    public string ReceKaCd { get; private set; }

    public string KaSname { get; private set; }

    public string KaName { get; private set; }

    public string YousikiKaCd { get; private set; }
}
