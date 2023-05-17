namespace Reporting.ReceTarget.Model;

public class CoReceTargetPrintDataModel
{
    public CoReceTargetPrintDataModel(int colCount)
    {
        LineDatas = new List<string>();

        for (int i = 0; i < colCount; i++)
        {
            LineDatas.Add(string.Empty);
        }
    }


    /// <summary>
    /// 1行に印字するデータ
    /// </summary>
    public List<string> LineDatas { get; set; }

    public string Comment { get; set; }
}
