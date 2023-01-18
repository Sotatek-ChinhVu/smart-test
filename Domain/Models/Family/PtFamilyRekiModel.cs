﻿namespace Domain.Models.Family;

public class PtFamilyRekiModel
{
    public PtFamilyRekiModel(long id, string byomeiCd, string byomei, string cmt, int sortNo, bool isDeleted)
    {
        Id = id;
        ByomeiCd = byomeiCd;
        Byomei = byomei;
        Cmt = cmt;
        SortNo = sortNo;
        IsDeleted = isDeleted;
    }

    public PtFamilyRekiModel(long id, string byomeiCd, string byomei, string cmt, int sortNo)
    {
        Id = id;
        ByomeiCd = byomeiCd;
        Byomei = byomei;
        Cmt = cmt;
        SortNo = sortNo;
        IsDeleted = false;
    }

    public long Id { get; private set; }

    public string ByomeiCd { get; private set; }

    public string Byomei { get; private set; }

    public string Cmt { get; private set; }

    public int SortNo { get; private set; }

    public bool IsDeleted { get; private set; }
}
