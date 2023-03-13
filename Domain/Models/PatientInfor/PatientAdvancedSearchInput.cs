namespace Domain.Models.PatientInfor;

public class PatientAdvancedSearchInput
{
    public PatientAdvancedSearchInput(long fromPtNum, long toPtNum, string name, int sex, int fromAge, int toAge,
        int fromBirthDay, int toBirthDay, string postalCode1, string postalCode2, string address, string phoneNum,
        int fromVisitDate, int toVisitDate, int fromLastVisitDate, int toLastVisitDate, long fromInsuranceNum,
        long toInsuranceNum, long fromPublicExpensesNum, long toPublicExpensesNum, string fromSpecialPublicExpensesNum,
        string toSpecialPublicExpensesNum, int hokenNum, int kohi1Num, int kohi1EdaNo, int kohi2Num, int kohi2EdaNo,
        int kohi3Num, int kohi3EdaNo, int kohi4Num, int kohi4EdaNo, List<PatientGroupSearchInput> patientGroups,
        LogicalOperator orderLogicalOperator, int departmentId, int doctorId,
        LogicalOperator byomeiLogicalOperator, List<ByomeiSearchInput> byomeis, List<TenMstSearchInput> tenMsts, int byomeiStartDate,
        int byomeiEndDate, int resultKbn, bool isSuspectedDisease, bool isOrderOr)
    {
        FromPtNum = fromPtNum;
        ToPtNum = toPtNum;
        Name = name;
        Sex = sex;
        FromAge = fromAge;
        ToAge = toAge;
        FromBirthDay = fromBirthDay;
        ToBirthDay = toBirthDay;
        PostalCode1 = postalCode1;
        PostalCode2 = postalCode2;
        Address = address;
        PhoneNum = phoneNum;
        FromVisitDate = fromVisitDate;
        ToVisitDate = toVisitDate;
        FromLastVisitDate = fromLastVisitDate;
        ToLastVisitDate = toLastVisitDate;
        FromInsuranceNum = fromInsuranceNum;
        ToInsuranceNum = toInsuranceNum;
        FromPublicExpensesNum = fromPublicExpensesNum;
        ToPublicExpensesNum = toPublicExpensesNum;
        FromSpecialPublicExpensesNum = fromSpecialPublicExpensesNum;
        ToSpecialPublicExpensesNum = toSpecialPublicExpensesNum;
        HokenNum = hokenNum;
        Kohi1Num = kohi1Num;
        Kohi1EdaNo = kohi1EdaNo;
        Kohi2Num = kohi2Num;
        Kohi2EdaNo = kohi2EdaNo;
        Kohi3Num = kohi3Num;
        Kohi3EdaNo = kohi3EdaNo;
        Kohi4Num = kohi4Num;
        Kohi4EdaNo = kohi4EdaNo;
        PatientGroups = patientGroups;
        OrderLogicalOperator = orderLogicalOperator;
        DepartmentId = departmentId;
        DoctorId = doctorId;
        ByomeiLogicalOperator = byomeiLogicalOperator;
        Byomeis = byomeis;
        TenMsts = tenMsts;
        ByomeiStartDate = byomeiStartDate;
        ByomeiEndDate = byomeiEndDate;
        ResultKbn = resultKbn;
        IsSuspectedDisease = isSuspectedDisease;
        IsOrderOr = isOrderOr;
    }

    // 基本情報
    public long FromPtNum { get; private set; }
    public long ToPtNum { get; private set; }
    public string Name { get; private set; }
    public int Sex { get; private set; }
    public int FromAge { get; private set; }
    public int ToAge { get; private set; }
    public int FromBirthDay { get; private set; }
    public int ToBirthDay { get; private set; }
    public string PostalCode1 { get; private set; }
    public string PostalCode2 { get; private set; }
    public string Address { get; private set; }
    public string PhoneNum { get; private set; }
    public int FromVisitDate { get; private set; }
    public int ToVisitDate { get; private set; }
    public int FromLastVisitDate { get; private set; }
    public int ToLastVisitDate { get; private set; }

    // 保険
    public long FromInsuranceNum { get; private set; }
    public long ToInsuranceNum { get; private set; }
    public long FromPublicExpensesNum { get; private set; }
    public long ToPublicExpensesNum { get; private set; }
    public string FromSpecialPublicExpensesNum { get; private set; }
    public string ToSpecialPublicExpensesNum { get; private set; }
    public int HokenNum { get; private set; }
    public int Kohi1Num { get; private set; }
    public int Kohi1EdaNo { get; private set; }
    public int Kohi2Num { get; private set; }
    public int Kohi2EdaNo { get; private set; }
    public int Kohi3Num { get; private set; }
    public int Kohi3EdaNo { get; private set; }
    public int Kohi4Num { get; private set; }
    public int Kohi4EdaNo { get; private set; }

    // グループ
    public List<PatientGroupSearchInput> PatientGroups { get; private set; }

    // オーダー
    public LogicalOperator OrderLogicalOperator { get; private set; }
    public int DepartmentId { get; private set; }
    public int DoctorId { get; private set; }

    // 傷病名
    public LogicalOperator ByomeiLogicalOperator { get; private set; }
    public List<ByomeiSearchInput> Byomeis { get; private set; }

    public List<TenMstSearchInput> TenMsts { get; private set; }

    public int ByomeiStartDate { get; private set; }
    public int ByomeiEndDate { get; private set; }
    public int ResultKbn { get; private set; }
    public bool IsSuspectedDisease { get; private set; }
    public bool IsOrderOr { get; private set; }
}

public class PatientGroupSearchInput
{
    public PatientGroupSearchInput(int groupId, string groupCode)
    {
        GroupId = groupId;
        GroupCode = groupCode;
    }

    public int GroupId { get; private set; }
    public string GroupCode { get; private set; }
}

public class ByomeiSearchInput
{
    public ByomeiSearchInput(string code, string name, bool isFreeWord)
    {
        Code = code;
        Name = name;
        IsFreeWord = isFreeWord;
    }

    public string Code { get; private set; }
    public string Name { get; private set; }
    public bool IsFreeWord { get; private set; }
}


public class TenMstSearchInput
{
    public TenMstSearchInput(string itemCd, string inputName, bool isComment)
    {
        ItemCd = itemCd;
        InputName = inputName;
        IsComment = isComment;
    }

    public string ItemCd { get; private set; }

    public string InputName { get; private set; }

    public bool IsComment { get; private set; }
}

public enum LogicalOperator
{
    Or = 0,
    And = 1
}
