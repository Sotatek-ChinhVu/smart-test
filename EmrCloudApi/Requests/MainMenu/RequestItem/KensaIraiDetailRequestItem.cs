﻿namespace EmrCloudApi.Requests.MainMenu.RequestItem;

public class KensaIraiDetailRequestItem
{
    public string TenKensaItemCd { get; set; } = string.Empty;

    public string ItemCd { get; set; } = string.Empty;

    public string ItemName { get; set; } = string.Empty;

    public string KanaName1 { get; set; } = string.Empty;

    public string CenterCd { get; set; } = string.Empty;

    public string KensaItemCd { get; set; } = string.Empty;

    public string CenterItemCd { get; set; } = string.Empty;

    public string KensaKana { get; set; } = string.Empty;

    public string KensaName { get; set; } = string.Empty;

    public long ContainerCd { get; set; }

    public long RpNo { get; set; }

    public long RpEdaNo { get; set; }

    public int RowNo { get; set; }

    public int SeqNo { get; set; }
}
