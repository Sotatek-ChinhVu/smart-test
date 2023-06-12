﻿namespace EmrCloudApi.Requests.SetMst;

public class SaveSetMstRequest
{
    public long PtId { get; set; }

    public long RaiinNo { get; set; }

    public int SinDate { get; set; }

    public int SetCd { get; set; } = 0;

    public int SetKbn { get; set; }

    public int SetKbnEdaNo { get; set; }

    public int GenerationId { get; set; } = 0;

    public int Level1 { get; set; }

    public int Level2 { get; set; } = 0;

    public int Level3 { get; set; } = 0;

    public string SetName { get; set; } = string.Empty;

    public int WeightKbn { get; set; } = 0;

    public int Color { get; set; } = 0;

    public int IsDeleted { get; set; } = 0;

    public bool IsGroup { get; set; } = false;
}
