using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class ReceiptCheckCmtErrDto
{
    public ReceiptCheckCmtErrDto(ReceiptCheckCmtErrListItem output)
    {
        SeqNo = output.SeqNo;
        SortNo = output.SortNo;
        IsChecked = output.IsChecked;
        TextDisplay1 = output.TextDisplay1;
        TextDisplay2 = output.TextDisplay2;
        StatusColor = output.StatusColor;
        ReceiptCheckIsErrItem = output.ReceiptCheckIsErrItem;
    }

    public int SeqNo { get; private set; }

    public int SortNo { get; private set; }

    public bool IsChecked { get; private set; }

    public string TextDisplay1 { get; private set; }

    public string TextDisplay2 { get; private set; }

    public int StatusColor { get; private set; }

    public bool ReceiptCheckIsErrItem { get; private set; }
}
