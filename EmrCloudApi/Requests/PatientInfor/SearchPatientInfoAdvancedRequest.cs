using Domain.Models.PatientInfor;

namespace EmrCloudApi.Requests.PatientInfor;

public class SearchPatientInfoAdvancedRequest
{
    public long FromPtNum { get; set; }

    public long ToPtNum { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Sex { get; set; }

    public int FromAge { get; set; }

    public int ToAge { get; set; }

    public int FromBirthDay { get; set; }

    public int ToBirthDay { get; set; }

    public string PostalCode1 { get; set; } = string.Empty;

    public string PostalCode2 { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string PhoneNum { get; set; } = string.Empty;

    public int FromVisitDate { get; set; }

    public int ToVisitDate { get; set; }

    public int FromLastVisitDate { get; set; }

    public int ToLastVisitDate { get; set; }


    // 保険
    public long FromInsuranceNum { get; set; }

    public long ToInsuranceNum { get; set; }

    public long FromPublicExpensesNum { get; set; }

    public long ToPublicExpensesNum { get; set; }

    public string FromSpecialPublicExpensesNum { get; set; } = string.Empty;

    public string ToSpecialPublicExpensesNum { get; set; } = string.Empty;

    public int HokenNum { get; set; }

    public int Kohi1Num { get; set; }

    public int Kohi1EdaNo { get; set; }

    public int Kohi2Num { get; set; }

    public int Kohi2EdaNo { get; set; }

    public int Kohi3Num { get; set; }

    public int Kohi3EdaNo { get; set; }

    public int Kohi4Num { get; set; }

    public int Kohi4EdaNo { get; set; }


    // グループ
    public List<PatientGroupSearchInput> PatientGroups { get; set; } = new();



    // オーダー
    public LogicalOperator OrderLogicalOperator { get; set; }

    public List<string> OrderItemCodes { get; set; } = new();

    public int DepartmentId { get; set; }

    public int DoctorId { get; set; }


    // 傷病名
    public LogicalOperator ByomeiLogicalOperator { get; set; }

    public List<ByomeiSearchInput> Byomeis { get; set; } = new();

    public int ByomeiStartDate { get; set; }

    public int ByomeiEndDate { get; set; }

    public int ResultKbn { get; set; }

    public bool IsSuspectedDisease { get; set; }

    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}
