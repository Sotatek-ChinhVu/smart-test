using Domain.Models.Yousiki;

namespace UseCase.Yousiki.CommonOutputData.CommonOutputModel;

public class OutpatientConsultationModel
{
    public Yousiki1InfDetailModel ConsultationDate { get; private set; }

    public Yousiki1InfDetailModel FirstVisit { get; private set; }

    public Yousiki1InfDetailModel Referral { get; private set; }

    public Yousiki1InfDetailModel DepartmentCode { get; private set; }

    public int RowNo { get; private set; }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int DataType { get; private set; }

    public int SeqNo { get; private set; }

    public string CodeNo { get; private set; } = string.Empty;

    public string DepartmentCodeDisplay { get; private set; } = string.Empty;

    public OutpatientConsultationModel(List<Yousiki1InfDetailModel> yousiki1InfDetails, Dictionary<string, string> kacodeYousikiMstDict)
    {
        SetData(yousiki1InfDetails, kacodeYousikiMstDict);
    }

    public OutpatientConsultationModel SetData(List<Yousiki1InfDetailModel> yousiki1InfDetailList, Dictionary<string, string> kacodeYousikiMstDict)
    {
        if (!yousiki1InfDetailList.Any())
        {
            return this;
        }

        PtId = yousiki1InfDetailList[0].PtId;
        SinYm = yousiki1InfDetailList[0].SinYm;
        DataType = yousiki1InfDetailList[0].DataType;
        SeqNo = yousiki1InfDetailList[0].SeqNo;
        CodeNo = yousiki1InfDetailList[0].CodeNo;
        RowNo = yousiki1InfDetailList[0].RowNo;

        foreach (var item in yousiki1InfDetailList)
        {
            if (item.Payload == 1)
            {
                ConsultationDate = item;
            }
            else if (item.Payload == 2)
            {
                FirstVisit = item;
            }
            else if (item.Payload == 3)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    Referral = item;
                }
            }
            else if (item.Payload == 4)
            {
                DepartmentCode = item;
                var departmentValue = kacodeYousikiMstDict.ContainsKey(item.Value) ? kacodeYousikiMstDict[item.Value] : string.Empty;
                DepartmentCodeDisplay = item.Value + " " + departmentValue ?? string.Empty;
            }
        }
        return this;
    }
}
