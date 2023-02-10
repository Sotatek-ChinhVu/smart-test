namespace UseCase.Receipt.ReceiptListAdvancedSearch;

public class ItemSearchInputItem
{
    public ItemSearchInputItem(string itemCd, string inputName, string rangeSeach, int amount, int orderStatus, bool isComment)
    {
        ItemCd = itemCd;
        InputName = inputName;
        RangeSeach = rangeSeach;
        Amount = amount;
        OrderStatus = orderStatus;
        IsComment = isComment;
    }

    public string ItemCd { get; private set; }

    public string InputName { get; private set; }

    public string RangeSeach { get; private set; }

    public int Amount { get; private set; }

    public int OrderStatus { get; private set; }

    public bool IsComment { get; private set; }
}
