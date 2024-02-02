using Helper.Enum;
using Helper.Extension;

namespace Domain.Models.Yousiki.CommonModel.CommonOutputModel;

public class CommonForm1Model
{
    public int PayLoadValueSelect { get; set; } = 1;

    public Yousiki1InfDetailModel ValueSelect { get; set; }

    public int PayLoadInjuryName { get; set; } = 9;

    public Yousiki1InfDetailModel InjuryNameLast { get; set; }

    public int PayLoadICD10Code { get; set; } = 2;

    public Yousiki1InfDetailModel Icd10 { get; set; }

    public int PayLoadInjuryNameCode { get; set; } = 3;

    public Yousiki1InfDetailModel InjuryNameCode { get; set; }

    public int PayLoadModifierCode { get; set; } = 4;

    public Yousiki1InfDetailModel ModifierCode { get; set; }

    public int SortNo { get; set; }

    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int DataType { get; set; }

    public int SeqNo { get; set; }

    public string CodeNo { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    #region 入院の状況
    /// <summary>
    /// 入院年月日
    /// </summary>
    public int DateOfHospitalizationPayLoad { get; set; }

    public Yousiki1InfDetailModel DateOfHospitalization { get; set; }

    /// <summary>
    /// 退院年月日
    /// </summary>
    public int DischargeDatePayLoad { get; set; }

    public Yousiki1InfDetailModel DischargeDate { get; set; }

    /// <summary>
    /// 受診先
    /// </summary>
    public int DestinationPayLoad { get; set; }

    public Yousiki1InfDetailModel Destination { get; set; }

    public bool IsEnableICD10Code { get; set; }
    #endregion

    #region 往診の状況
    /// <summary>
    /// 往診日
    /// </summary>
    public int HouseCallDatePayLoad { get; set; }

    public Yousiki1InfDetailModel HouseCallDate { get; set; }

    /// <summary>
    /// 主たる訪問診療を行う医療機関	
    /// </summary>
    public Yousiki1InfDetailModel MedicalInstitution { get; set; }

    public int MedicalInstitutionPayLoad { get; set; }
    #endregion

    #region リハビリテーションが必要となった主病
    public int StartDatePayLoad { get; set; }

    public Yousiki1InfDetailModel StartDate { get; set; }

    public int OnsetDatePayLoad { get; set; }

    public Yousiki1InfDetailModel OnsetDate { get; set; }

    public int MaximumNumberDatePayLoad { get; set; }

    public Yousiki1InfDetailModel MaximumNumberDate { get; set; }

    public ByomeiListType GridType { get; set; } = ByomeiListType.None;
    #endregion

    #region byomei
    public string ByomeiCd { get; set; } = string.Empty;

    public string Byomei { get; set; } = string.Empty;

    public List<PrefixSuffixModel> PrefixSuffixList { get; set; } = new();
    #endregion

    public CommonForm1Model(string codeNo, Yousiki1InfModel yousiki1Inf)
    {
        CodeNo = codeNo;
        if (yousiki1Inf != null)
        {
            PtId = yousiki1Inf.PtId;
            SinYm = yousiki1Inf.SinYm;
            DataType = yousiki1Inf.DataType;
            SeqNo = yousiki1Inf.SeqNo;
        }
    }

    public CommonForm1Model(List<Yousiki1InfDetailModel> yousiki1InfDetails)
    {
        SetData(yousiki1InfDetails);
    }

    public CommonForm1Model SetData(List<Yousiki1InfDetailModel> yousiki1InfDetails, ByomeiListType listType = ByomeiListType.None)
    {
        if (!yousiki1InfDetails.Any())
        {
            return this;
        }

        PtId = yousiki1InfDetails[0].PtId;
        SinYm = yousiki1InfDetails[0].SinYm;
        DataType = yousiki1InfDetails[0].DataType;
        SeqNo = yousiki1InfDetails[0].SeqNo;
        CodeNo = yousiki1InfDetails[0].CodeNo;
        SortNo = yousiki1InfDetails[0].RowNo;
        GridType = listType;

        foreach (var item in yousiki1InfDetails)
        {
            if (listType == ByomeiListType.None)
            {
                if (item.Payload == PayLoadValueSelect)
                {
                    ValueSelect = item;
                    if (ValueSelect.Value.AsInteger() == 1)
                    {
                        IsEnableICD10Code = true;
                    }
                }
                else if (item.Payload == PayLoadInjuryName)
                {
                    InjuryNameLast = item;
                }
                else if (item.Value.AsInteger() == PayLoadICD10Code)
                {
                    Icd10 = item;
                }
                else if (item.Payload == PayLoadInjuryNameCode)
                {
                    InjuryNameCode = item;
                }
                else if (item.Payload == PayLoadModifierCode)
                {
                    ModifierCode = item;
                }
            }
            else if (listType == ByomeiListType.HospitalizationStatus)
            {
                if (item.Payload == PayLoadInjuryName)
                {
                    InjuryNameLast = item;
                }
                else if (item.Payload == PayLoadICD10Code)
                {
                    Icd10 = item;
                }
                else if (item.Payload == PayLoadInjuryNameCode)
                {
                    InjuryNameCode = item;
                }
                else if (item.Payload == PayLoadModifierCode)
                {
                    ModifierCode = item;
                }
                else if (item.Payload == DateOfHospitalizationPayLoad)
                {
                    DateOfHospitalization = item;
                }
                else if (item.Payload == DischargeDatePayLoad)
                {
                    DischargeDate = item;
                }
                else if (item.Payload == DestinationPayLoad)
                {
                    Destination = item;
                    IsEnableICD10Code = Destination.Value == "1";
                }
            }
            else if (listType == ByomeiListType.StatusHomeVisit)
            {
                if (item.Payload == PayLoadInjuryName)
                {
                    InjuryNameLast = item;
                }
                else if (item.Payload == PayLoadICD10Code)
                {
                    Icd10 = item;
                }
                else if (item.Payload == PayLoadInjuryNameCode)
                {
                    InjuryNameCode = item;
                }
                else if (item.Payload == PayLoadModifierCode)
                {
                    ModifierCode = item;
                }
                else if (item.Payload == HouseCallDatePayLoad)
                {
                    HouseCallDate = item;
                }
                else if (item.Payload == MedicalInstitutionPayLoad)
                {
                    MedicalInstitution = item;
                    IsEnableICD10Code = MedicalInstitution.Value == "1";
                }
            }
            else if (listType == ByomeiListType.Rehabilitation)
            {
                IsEnableICD10Code = true;
                if (item.Payload == PayLoadInjuryName)
                {
                    InjuryNameLast = item;
                }
                else if (item.Payload == PayLoadICD10Code)
                {
                    Icd10 = item;
                }
                else if (item.Payload == PayLoadInjuryNameCode)
                {
                    InjuryNameCode = item;
                }
                else if (item.Payload == PayLoadModifierCode)
                {
                    ModifierCode = item;
                }
                else if (item.Payload == StartDatePayLoad)
                {
                    StartDate = item;
                }
                else if (item.Payload == OnsetDatePayLoad)
                {
                    OnsetDate = item;
                }
                else if (item.Payload == MaximumNumberDatePayLoad)
                {
                    MaximumNumberDate = item;
                }
            }
        }
        return this;
    }

    public List<string> GetByomeiCdList()
    {
        List<string> result = new();
        var byomeiCd = InjuryNameCode?.Value;
        if (!string.IsNullOrEmpty(byomeiCd))
        {
            result.Add(byomeiCd);
        }

        var modifierCode = ModifierCode?.Value;
        int i = 1;
        while (!string.IsNullOrEmpty(modifierCode) && modifierCode.Length >= 4)
        {
            var mstItemCd = modifierCode.Substring(0, 4);
            if (!string.IsNullOrEmpty(mstItemCd))
            {
                result.Add(mstItemCd);
            }
            modifierCode = modifierCode.Remove(0, 4);
            i++;
        }

        return result;
    }

    public CommonForm1Model GetCommonImageInf(Dictionary<string, string> byomeiDictionary)
    {
        ByomeiCd = InjuryNameCode?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(ByomeiCd))
        {
            return this;
        }

        Byomei = byomeiDictionary.ContainsKey(ByomeiCd) ? byomeiDictionary[ByomeiCd] : string.Empty;
        var modifierCode = ModifierCode?.Value;

        int i = 1;
        while (!string.IsNullOrEmpty(modifierCode) && modifierCode.Length >= 4)
        {
            var mstItemCd = modifierCode.Substring(0, 4);
            if (byomeiDictionary.ContainsKey(mstItemCd))
            {
                PrefixSuffixList.Add(new PrefixSuffixModel(mstItemCd, byomeiDictionary[mstItemCd]));
            }

            modifierCode = modifierCode.Remove(0, 4);
            i++;
        }

        return this;
    }
}
