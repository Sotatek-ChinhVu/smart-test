namespace Domain.Models.Receipt;

public class ItemSearchModel
{
    public ItemSearchModel(string itemCd, string inputName, string rangeSeach, int amount, bool isTestPatientSearch, int orderStatus, bool isComment)
    {
        ItemCd = itemCd;
        InputName = inputName;
        RangeSeach = rangeSeach;
        Amount = amount;
        IsTestPatientSearch = isTestPatientSearch;
        OrderStatus = orderStatus;
        IsComment = isComment;
    }

    public string ItemCd { get; private set; }

    public string InputName { get; private set; }

    public string RangeSeach { get; private set; }

    public int Amount { get; private set; }

    public bool IsTestPatientSearch { get; private set; }

    public int OrderStatus { get; private set; }

    public bool IsComment { get; private set; }
}
