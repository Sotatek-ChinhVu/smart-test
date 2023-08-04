namespace UseCase.MstItem.GetListDrugImage;

public class DrugImageOutputItem
{
    public DrugImageOutputItem(string fileLink, bool isEnable, bool isSelected)
    {
        FileLink = fileLink;
        IsEnable = isEnable;
        IsSelected = isSelected;
    }

    public string FileLink { get; private set; }

    public bool IsEnable { get; private set; }

    public bool IsSelected { get; private set; }
}
