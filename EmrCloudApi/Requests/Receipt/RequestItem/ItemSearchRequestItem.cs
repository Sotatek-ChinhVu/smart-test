﻿namespace EmrCloudApi.Requests.Receipt.RequestItem;

public class ItemSearchRequestItem
{
    public string ItemCd { get; set; } = string.Empty;

    public string InputName { get; set; } = string.Empty;

    public string RangeSeach { get; set; } = string.Empty;

    public int Amount { get; set; }

    public int OrderStatus { get; set; }

    public bool IsComment { get; set; }
}
