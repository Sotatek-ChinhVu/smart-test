﻿namespace Domain.Models.Insurance;

public class RousaiTenkiModel
{
    public RousaiTenkiModel(int rousaiTenkiSinkei, int rousaiTenkiTenki, int rousaiTenkiEndDate, int rousaiTenkiIsDeleted, long seqNo)
    {
        RousaiTenkiSinkei = rousaiTenkiSinkei;
        RousaiTenkiTenki = rousaiTenkiTenki;
        RousaiTenkiEndDate = rousaiTenkiEndDate;
        RousaiTenkiIsDeleted = rousaiTenkiIsDeleted;
        SeqNo = seqNo;
    }

    public int RousaiTenkiSinkei { get; private set; }

    public int RousaiTenkiTenki { get; private set; }

    public int RousaiTenkiEndDate { get; private set; }

    public int RousaiTenkiIsDeleted { get; private set; }

    public long SeqNo { get; private set; }

    public bool CheckDefaultValue()
    {
        return RousaiTenkiSinkei <= 0 && RousaiTenkiTenki <= 0 && (RousaiTenkiEndDate == 0 || RousaiTenkiEndDate == 999999);
    }
}
