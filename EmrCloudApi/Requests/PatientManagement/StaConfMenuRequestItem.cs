using System.Text.Json.Serialization;

namespace EmrCloudApi.Requests.PatientManagement
{
    public class StaConfMenuRequestItem
    {
        [JsonConstructor]
        public StaConfMenuRequestItem(int menuId, int grpId, int reportId, int sortNo, string menuName, bool isDeleted, bool isModified, PatientManagementItem patientManagement)
        {
            MenuId = menuId;
            GrpId = grpId;
            ReportId = reportId;
            SortNo = sortNo;
            MenuName = menuName;
            IsDeleted = isDeleted;
            IsModified = isModified;
            PatientManagement = patientManagement;
        }

        public StaConfMenuRequestItem()
        {
        }

        public int MenuId { get; private set; }
        public int GrpId { get; private set; }
        public int ReportId { get; private set; }
        public int SortNo { get; private set; }
        public string MenuName { get; private set; } = string.Empty;
        public bool IsDeleted { get; private set; }
        public bool IsModified { get; private set; }
        public PatientManagementItem PatientManagement { get; set; } = new();
    }

    public class PatientManagementItem
    {
        public int OutputOrder1 { get; set; }
        public int OutputOrder2 { get; set; }
        public int OutputOrder3 { get; set; }
        public int ReportType { get; set; }
        public long PtNumFrom { get; set; }
        public long PtNumTo { get; set; }
        public string KanaName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int BirthDayFrom { get; set; }
        public int BirthDayTo { get; set; }
        public string AgeFrom { get; set; } = string.Empty;
        public string AgeTo { get; set; } = string.Empty;
        public string AgeRefDate { get; set; } = string.Empty;
        public int Sex { get; set; }
        public string HomePost { get; set; } = string.Empty;
        public string ZipCD1 { get; set; } = string.Empty;
        public string ZipCD2 { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int IncludeTestPt { get; set; }
        public List<long> ListPtNums { get; set; } = new();
        public int RegistrationDateFrom { get; set; }
        public int RegistrationDateTo { get; set; }
        public string GroupSelected { get; set; } = string.Empty;
        public string HokensyaNoFrom { get; set; } = string.Empty;
        public string HokensyaNoTo { get; set; } = string.Empty;
        public string Kigo { get; set; } = string.Empty;
        public string Bango { get; set; } = string.Empty;
        public string EdaNo { get; set; } = string.Empty;
        public int HokenKbn { get; set; }
        public string KohiFutansyaNoFrom { get; set; } = string.Empty;
        public string KohiFutansyaNoTo { get; set; } = string.Empty;
        public string KohiTokusyuNoFrom { get; set; } = string.Empty;
        public string KohiTokusyuNoTo { get; set; } = string.Empty;
        public int ExpireDateFrom { get; set; }
        public int ExpireDateTo { get; set; }
        public List<int> HokenSbt { get; set; } = new();
        public string Houbetu1 { get; set; } = string.Empty;
        public string Houbetu2 { get; set; } = string.Empty;
        public string Houbetu3 { get; set; } = string.Empty;
        public string Houbetu4 { get; set; } = string.Empty;
        public string Houbetu5 { get; set; } = string.Empty;
        public string Kogaku { get; set; } = string.Empty;
        public int KohiHokenNoFrom { get; set; }
        public int KohiHokenEdaNoFrom { get; set; }
        public int KohiHokenNoTo { get; set; }
        public int KohiHokenEdaNoTo { get; set; }
        public int StartDateFrom { get; set; }
        public int StartDateTo { get; set; }
        public int TenkiDateFrom { get; set; }
        public int TenkiDateTo { get; set; }
        public int IsDoubt { get; set; }
        public string SearchWord { get; set; } = string.Empty;
        public int SearchWordMode { get; set; }
        public int ByomeiCdOpt { get; set; }
        public int SindateFrom { get; set; }
        public int SindateTo { get; set; }
        public int LastVisitDateFrom { get; set; }
        public int LastVisitDateTo { get; set; }
        public int IsSinkan { get; set; }
        public string RaiinAgeFrom { get; set; } = string.Empty;
        public string RaiinAgeTo { get; set; } = string.Empty;
        public int DataKind { get; set; }
        public int ItemCdOpt { get; set; }
        public string MedicalSearchWord { get; set; } = string.Empty;
        public int WordOpt { get; set; }
        public int KarteWordOpt { get; set; }
        public int StartIraiDate { get; set; }
        public int EndIraiDate { get; set; }
        public int KensaItemCdOpt { get; set; }
        public List<int> TenkiKbns { get; set; } = new();
        public List<int> SikkanKbns { get; set; } = new();
        public List<string> ByomeiCds { get; set; } = new();
        public List<string> FreeByomeis { get; set; } = new();
        public List<int> NanbyoCds { get; set; } = new();
        public List<int> Statuses { get; set; } = new();
        public List<int> UketukeSbtId { get; set; } = new();
        public List<string> ItemCds { get; set; } = new();
        public List<int> KaMstId { get; set; } = new();
        public List<int> UserMstId { get; set; } = new();
        public List<int> JikanKbns { get; set; } = new();
        public List<string> ItemCmts { get; set; } = new();
        public List<int> KarteKbns { get; set; } = new();
        public List<string> KarteSearchWords { get; set; } = new();
        public List<string> KensaItemCds { get; set; } = new();
    }
}
