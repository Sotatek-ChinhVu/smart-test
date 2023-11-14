namespace Domain.Models.PatientInfor
{
    public class PatientManagementModel
    {
        public PatientManagementModel()
        {
            KanaName = string.Empty;
            Name = string.Empty;
            AgeRefDate = string.Empty;
            HomePost = string.Empty;
            ZipCD1 = string.Empty;
            ZipCD2 = string.Empty;
            Address = string.Empty;
            PhoneNumber = string.Empty;
            ListPtNums = new();
            GroupSelected = string.Empty;
            HokensyaNoFrom = string.Empty;
            HokensyaNoTo = string.Empty;
            Kigo = string.Empty;
            Bango = string.Empty;
            EdaNo = string.Empty;
            AgeFrom = string.Empty;
            AgeTo = string.Empty;
            KohiTokusyuNoFrom = string.Empty;
            KohiTokusyuNoTo = string.Empty;
            SearchWord = string.Empty;
            KohiFutansyaNoFrom = string.Empty;
            KohiFutansyaNoTo = string.Empty;
            HokenSbt = new();
            Houbetu1 = string.Empty;
            Houbetu2 = string.Empty;
            Houbetu3 = string.Empty;
            Houbetu4 = string.Empty;
            Houbetu5 = string.Empty;
            Kogaku = string.Empty;
            RaiinAgeFrom = string.Empty;
            RaiinAgeTo = string.Empty;
            MedicalSearchWord = string.Empty;
            TenkiKbns = new();
            SikkanKbns = new();
            ByomeiCds = new();
            FreeByomeis = new();
            NanbyoCds = new();
            Statuses = new();
            UketukeSbtId = new();
            ItemCds = new();
            KaMstId = new();
            UserMstId = new();
            JikanKbns = new();
            ItemCmts = new();
            KarteKbns = new();
            KarteSearchWords = new();
            KensaItemCds = new();
        }

        public PatientManagementModel(int outputOrder, int outputOrder2, int outputOrder3, int reportType, long ptNumFrom, long ptNumTo, string kanaName, string name, int birthDayFrom, int birthDayTo, string ageFrom, string ageTo, string ageRefDate, int sex, string homePost, string zipCD1, string zipCD2, string address, string phoneNumber, int includeTestPt, List<long> listPtNums, int registrationDateFrom, int registrationDateTo, string groupSelected, string hokensyaNoFrom, string hokensyaNoTo, string kigo, string bango, string edaNo, int hokenKbn, string kohiFutansyaNoFrom, string kohiFutansyaNoTo, string kohiTokusyuNoFrom, string kohiTokusyuNoTo, int expireDateFrom, int expireDateTo, List<int> hokenSbt, string houbetu1, string houbetu2, string houbetu3, string houbetu4, string houbetu5, string kogaku, int kohiHokenNoFrom, int kohiHokenEdaNoFrom, int kohiHokenNoTo, int kohiHokenEdaNoTo, int startDateFrom, int startDateTo, int tenkiDateFrom, int tenkiDateTo, int isDoubt, string searchWord, int searchWordMode, int byomeiCdOpt, int sindateFrom, int sindateTo, int lastVisitDateFrom, int lastVisitDateTo, int isSinkan, string raiinAgeFrom, string raiinAgeTo, int dataKind, int itemCdOpt, string medicalSearchWord, int wordOpt, int karteWordOpt, int startIraiDate, int endIraiDate, int kensaItemCdOpt, List<int> tenkiKbns, List<int> sikkanKbns, List<string> byomeiCds, List<string> freeByomeis, List<int> nanbyoCds, List<int> statuses, List<int> uketukeSbtId, List<string> itemCds, List<int> kaMstId, List<int> userMstId, List<int> jikanKbns, List<string> itemCmts, List<int> karteKbns, List<string> karteSearchWords, List<string> kensaItemCds)
        {
            OutputOrder = outputOrder;
            OutputOrder2 = outputOrder2;
            OutputOrder3 = outputOrder3;
            ReportType = reportType;
            PtNumFrom = ptNumFrom;
            PtNumTo = ptNumTo;
            KanaName = kanaName;
            Name = name;
            BirthDayFrom = birthDayFrom;
            BirthDayTo = birthDayTo;
            AgeFrom = ageFrom;
            AgeTo = ageTo;
            AgeRefDate = ageRefDate;
            Sex = sex;
            HomePost = homePost;
            ZipCD1 = zipCD1;
            ZipCD2 = zipCD2;
            Address = address;
            PhoneNumber = phoneNumber;
            IncludeTestPt = includeTestPt;
            ListPtNums = listPtNums;
            RegistrationDateFrom = registrationDateFrom;
            RegistrationDateTo = registrationDateTo;
            GroupSelected = groupSelected;
            HokensyaNoFrom = hokensyaNoFrom;
            HokensyaNoTo = hokensyaNoTo;
            Kigo = kigo;
            Bango = bango;
            EdaNo = edaNo;
            HokenKbn = hokenKbn;
            KohiFutansyaNoFrom = kohiFutansyaNoFrom;
            KohiFutansyaNoTo = kohiFutansyaNoTo;
            KohiTokusyuNoFrom = kohiTokusyuNoFrom;
            KohiTokusyuNoTo = kohiTokusyuNoTo;
            ExpireDateFrom = expireDateFrom;
            ExpireDateTo = expireDateTo;
            HokenSbt = hokenSbt;
            Houbetu1 = houbetu1;
            Houbetu2 = houbetu2;
            Houbetu3 = houbetu3;
            Houbetu4 = houbetu4;
            Houbetu5 = houbetu5;
            Kogaku = kogaku;
            KohiHokenNoFrom = kohiHokenNoFrom;
            KohiHokenEdaNoFrom = kohiHokenEdaNoFrom;
            KohiHokenNoTo = kohiHokenNoTo;
            KohiHokenEdaNoTo = kohiHokenEdaNoTo;
            StartDateFrom = startDateFrom;
            StartDateTo = startDateTo;
            TenkiDateFrom = tenkiDateFrom;
            TenkiDateTo = tenkiDateTo;
            IsDoubt = isDoubt;
            SearchWord = searchWord;
            SearchWordMode = searchWordMode;
            ByomeiCdOpt = byomeiCdOpt;
            SindateFrom = sindateFrom;
            SindateTo = sindateTo;
            LastVisitDateFrom = lastVisitDateFrom;
            LastVisitDateTo = lastVisitDateTo;
            IsSinkan = isSinkan;
            RaiinAgeFrom = raiinAgeFrom;
            RaiinAgeTo = raiinAgeTo;
            DataKind = dataKind;
            ItemCdOpt = itemCdOpt;
            MedicalSearchWord = medicalSearchWord;
            WordOpt = wordOpt;
            KarteWordOpt = karteWordOpt;
            StartIraiDate = startIraiDate;
            EndIraiDate = endIraiDate;
            KensaItemCdOpt = kensaItemCdOpt;
            TenkiKbns = tenkiKbns;
            SikkanKbns = sikkanKbns;
            ByomeiCds = byomeiCds;
            FreeByomeis = freeByomeis;
            NanbyoCds = nanbyoCds;
            Statuses = statuses;
            UketukeSbtId = uketukeSbtId;
            ItemCds = itemCds;
            KaMstId = kaMstId;
            UserMstId = userMstId;
            JikanKbns = jikanKbns;
            ItemCmts = itemCmts;
            KarteKbns = karteKbns;
            KarteSearchWords = karteSearchWords;
            KensaItemCds = kensaItemCds;
        }

        public int OutputOrder { get; private set; }
        public int OutputOrder2 { get; private set; }
        public int OutputOrder3 { get; private set; }
        public int ReportType { get; private set; }
        public long PtNumFrom { get; private set; }
        public long PtNumTo { get; private set; }
        public string KanaName { get; private set; }
        public string Name { get; private set; }
        public int BirthDayFrom { get; private set; }
        public int BirthDayTo { get; private set; }
        public string AgeFrom { get; private set; }
        public string AgeTo { get; private set; }
        public string AgeRefDate { get; private set; }
        public int Sex { get; private set; }
        public string HomePost { get; private set; }
        public string ZipCD1 { get; private set; }
        public string ZipCD2 { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public int IncludeTestPt { get; private set; }
        public List<long> ListPtNums { get; private set; }
        public int RegistrationDateFrom { get; private set; }
        public int RegistrationDateTo { get; private set; }
        public string GroupSelected { get; private set; }
        public string HokensyaNoFrom { get; private set; }
        public string HokensyaNoTo { get; private set; }
        public string Kigo { get; private set; }
        public string Bango { get; private set; }
        public string EdaNo { get; private set; }
        public int HokenKbn { get; private set; }
        public string KohiFutansyaNoFrom { get; private set; }
        public string KohiFutansyaNoTo { get; private set; }
        public string KohiTokusyuNoFrom { get; private set; }
        public string KohiTokusyuNoTo { get; private set; }
        public int ExpireDateFrom { get; private set; }
        public int ExpireDateTo { get; private set; }
        public List<int> HokenSbt { get; private set; }
        public string Houbetu1 { get; private set; }
        public string Houbetu2 { get; private set; }
        public string Houbetu3 { get; private set; }
        public string Houbetu4 { get; private set; }
        public string Houbetu5 { get; private set; }
        public string Kogaku { get; private set; }
        public int KohiHokenNoFrom { get; private set; }
        public int KohiHokenEdaNoFrom { get; private set; }
        public int KohiHokenNoTo { get; private set; }
        public int KohiHokenEdaNoTo { get; private set; }
        public int StartDateFrom { get; private set; }
        public int StartDateTo { get; private set; }
        public int TenkiDateFrom { get; private set; }
        public int TenkiDateTo { get; private set; }
        public int IsDoubt { get; private set; }
        public string SearchWord { get; private set; }
        public int SearchWordMode { get; private set; }
        public int ByomeiCdOpt { get; private set; }
        public int SindateFrom { get; private set; }
        public int SindateTo { get; private set; }
        public int LastVisitDateFrom { get; private set; }
        public int LastVisitDateTo { get; private set; }
        public int IsSinkan { get; private set; }
        public string RaiinAgeFrom { get; private set; }
        public string RaiinAgeTo { get; private set; }
        public int DataKind { get; private set; }
        public int ItemCdOpt { get; private set; }
        public string MedicalSearchWord { get; private set; }
        public int WordOpt { get; private set; }
        public int KarteWordOpt { get; private set; }
        public int StartIraiDate { get; private set; }
        public int EndIraiDate { get; private set; }
        public int KensaItemCdOpt { get; private set; }
        public List<int> TenkiKbns { get; private set; }
        public List<int> SikkanKbns { get; private set; }
        public List<string> ByomeiCds { get; private set; }
        public List<string> FreeByomeis { get; private set; }
        public List<int> NanbyoCds { get; private set; }
        public List<int> Statuses { get; private set; }
        public List<int> UketukeSbtId { get; private set; }
        public List<string> ItemCds { get; private set; }
        public List<int> KaMstId { get; private set; }
        public List<int> UserMstId { get; private set; }
        public List<int> JikanKbns { get; private set; }
        public List<string> ItemCmts { get; private set; }
        public List<int> KarteKbns { get; private set; }
        public List<string> KarteSearchWords { get; private set; }
        public List<string> KensaItemCds { get; private set; }

        public string TenkiKbnStr
        {
            get => string.Join(",", TenkiKbns.ToArray());
        }
        public string SikkanKbnStr
        {
            get => string.Join(",", SikkanKbns.ToArray());
        }
        public string ByomeiCdStr
        {
            get => string.Join(",", ByomeiCds.ToArray());
        }
        public string HokenSbtStr
        {
            get => string.Join(",", HokenSbt.ToArray());
        }
        public string FreeByomeiStr
        {
            get => string.Join(",", FreeByomeis.ToArray());
        }
        public string NanbyoCdsStr
        {
            get => string.Join(",", NanbyoCds.ToArray());
        }
        public string StatuseStr
        {
            get => string.Join(",", Statuses.ToArray());
        }
        public string UketukeSbtStr
        {
            get => string.Join(",", UketukeSbtId.ToArray());
        }
        public string KaMstStr
        {
            get => string.Join(",", KaMstId.ToArray());
        }
        public string UserMstStr
        {
            get => string.Join(",", UserMstId.ToArray());
        }
        public string JikanKbnStr
        {
            get => string.Join(",", JikanKbns.ToArray());
        }
        public string ItemCdStr
        {
            get => string.Join(",", ItemCds.ToArray());
        }
        public string ItemCmtStr
        {
            get => string.Join(",", ItemCmts.ToArray());
        }
        public string KarteKbnsStr
        {
            get => string.Join(",", KarteKbns.ToArray());
        }
        public string KarteSearchWordsStr
        {
            get => string.Join(",", KarteSearchWords.ToArray());
        }
        public string KensaItemCdsStr
        {
            get => string.Join(",", KensaItemCds.ToArray());
        }
    }
}
