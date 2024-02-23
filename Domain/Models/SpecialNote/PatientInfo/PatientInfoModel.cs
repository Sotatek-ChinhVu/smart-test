using Domain.Models.PtCmtInf;
using Helper.Extension;

namespace Domain.Models.SpecialNote.PatientInfo;

public class PatientInfoModel
{
    public PatientInfoModel(List<PtPregnancyModel> pregnancyItems, PtCmtInfModel ptCmtInfItems, SeikaturekiInfModel seikatureInfItems, List<PhysicalInfoModel> physicalInfItems)
    {
        PregnancyItems = pregnancyItems;
        PtCmtInfItems = ptCmtInfItems;
        SeikatureInfItems = seikatureInfItems;
        PhysicalInfItems = physicalInfItems;
        FunctionName = string.Empty;
        FunctionCd = string.Empty;
    }

    public PatientInfoModel()
    {
        PregnancyItems = new();
        PtCmtInfItems = new();
        SeikatureInfItems = new();
        PhysicalInfItems = new();
        FunctionName = string.Empty;
        FunctionCd = string.Empty;
    }

    public PatientInfoModel(long ptId, string functionCd, int sinDate, long raiinNo, long oyaRaiinNo, string functionName, long ptNum)
    {
        PtId = ptId;
        FunctionCd = functionCd;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        OyaRaiinNo = oyaRaiinNo;
        PregnancyItems = new();
        PtCmtInfItems = new();
        SeikatureInfItems = new();
        PhysicalInfItems = new();
        FunctionName = functionName;
        PtNum = ptNum;
    }

    public long PtId { get; private set; }

    public string FunctionName { get; private set; }

    public long PtNum { get; private set; }

    public int SinDate { get; private set; }

    public string FunctionCd { get; private set; }

    public long RaiinNo { get; private set; }

    public long OyaRaiinNo { get; private set; }

    public List<PtPregnancyModel> PregnancyItems { get; private set; }

    public PtCmtInfModel PtCmtInfItems { get; private set; }

    public SeikaturekiInfModel SeikatureInfItems { get; private set; }

    public List<PhysicalInfoModel> PhysicalInfItems { get; private set; }

    public int SinDateInt
    {
        get
        {
            return SinDate.AsInteger();
        }
    }
}
