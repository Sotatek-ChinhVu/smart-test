using Helper.Common;
using Helper.Extension;

namespace Domain.Models.PatientInfor
{
    public class PatientInforModel
    {
        public PatientInforModel(int hpId, long ptId, long referenceNo, long seqNo, long ptNum, string kanaName, string name, int sex, int birthday, int limitConsFlg, int isDead, int deathDate, string homePost, string homeAddress1, string homeAddress2, string tel1, string tel2, string mail, string setanusi, string zokugara, string job, string renrakuName, string renrakuPost, string renrakuAddress1, string renrakuAddress2, string renrakuTel, string renrakuMemo, string officeName, string officePost, string officeAddress1, string officeAddress2, string officeTel, string officeMemo, int isRyosyoDetail, int primaryDoctor, int isTester, int mainHokenPid, string memo, int lastVisitDate, int firstVisitDate, string rainCount)
        {
            HpId = hpId;
            PtId = ptId;
            ReferenceNo = referenceNo;
            SeqNo = seqNo;
            PtNum = ptNum;
            KanaName = kanaName;
            Name = name;
            Sex = sex;
            Birthday = birthday;
            LimitConsFlg = limitConsFlg;
            IsDead = isDead;
            DeathDate = deathDate;
            HomePost = homePost;
            HomeAddress1 = homeAddress1;
            HomeAddress2 = homeAddress2;
            Tel1 = tel1;
            Tel2 = tel2;
            Mail = mail;
            Setanusi = setanusi;
            Zokugara = zokugara;
            Job = job;
            RenrakuName = renrakuName;
            RenrakuPost = renrakuPost;
            RenrakuAddress1 = renrakuAddress1;
            RenrakuAddress2 = renrakuAddress2;
            RenrakuTel = renrakuTel;
            RenrakuMemo = renrakuMemo;
            OfficeName = officeName;
            OfficePost = officePost;
            OfficeAddress1 = officeAddress1;
            OfficeAddress2 = officeAddress2;
            OfficeTel = officeTel;
            OfficeMemo = officeMemo;
            IsRyosyoDetail = isRyosyoDetail;
            PrimaryDoctor = primaryDoctor;
            IsTester = isTester;
            MainHokenPid = mainHokenPid;
            Memo = memo;
            LastVisitDate = lastVisitDate;
            FirstVisitDate = firstVisitDate;
            RainCount = rainCount;
            Comment = string.Empty;
        }

        public PatientInforModel(int hpId, long ptId, string comment)
        {
            HpId = hpId;
            PtId = ptId;
            Comment = comment;
            ReferenceNo = 0;
            SeqNo = 0;
            PtNum = 0;
            KanaName = string.Empty;
            Name = string.Empty;
            Sex = 0;
            Birthday = 0;
            LimitConsFlg = 0;
            IsDead = 0;
            DeathDate = 0;
            HomePost = string.Empty;
            HomeAddress1 = string.Empty;
            HomeAddress2 = string.Empty;
            Tel1 = string.Empty;
            Tel2 = string.Empty;
            Mail = string.Empty;
            Setanusi = string.Empty;
            Zokugara = string.Empty;
            Job = string.Empty;
            RenrakuName = string.Empty;
            RenrakuPost = string.Empty;
            RenrakuAddress1 = string.Empty;
            RenrakuAddress2 = string.Empty;
            RenrakuTel = string.Empty;
            RenrakuMemo = string.Empty;
            OfficeName = string.Empty;
            OfficePost = string.Empty;
            OfficeAddress1 = string.Empty;
            OfficeAddress2 = string.Empty;
            OfficeTel = string.Empty;
            OfficeMemo = string.Empty;
            IsRyosyoDetail = 0;
            PrimaryDoctor = 0;
            IsTester = 0;
            MainHokenPid = 0;
            Memo = string.Empty;
            LastVisitDate = 0;
            FirstVisitDate = 0;
            RainCount = string.Empty;
        }

        public PatientInforModel()
        {
            HpId = 0;
            PtId = 0;
            ReferenceNo = 0;
            SeqNo = 0;
            PtNum = 0;
            KanaName = string.Empty;
            Name = string.Empty;
            Sex = 0;
            Birthday = 0;
            LimitConsFlg = 0;
            IsDead = 0;
            DeathDate = 0;
            HomePost = string.Empty;
            HomeAddress1 = string.Empty;
            HomeAddress2 = string.Empty;
            Tel1 = string.Empty;
            Tel2 = string.Empty;
            Mail = string.Empty;
            Setanusi = string.Empty;
            Zokugara = string.Empty;
            Job = string.Empty;
            RenrakuName = string.Empty;
            RenrakuPost = string.Empty;
            RenrakuAddress1 = string.Empty;
            RenrakuAddress2 = string.Empty;
            RenrakuTel = string.Empty;
            RenrakuMemo = string.Empty;
            OfficeName = string.Empty;
            OfficePost = string.Empty;
            OfficeAddress1 = string.Empty;
            OfficeAddress2 = string.Empty;
            OfficeTel = string.Empty;
            OfficeMemo = string.Empty;
            IsRyosyoDetail = 0;
            PrimaryDoctor = 0;
            IsTester = 0;
            MainHokenPid = 0;
            Memo = string.Empty;
            LastVisitDate = 0;
            FirstVisitDate = 0;
            RainCount = string.Empty;
            Comment = string.Empty;
        }

        public PatientInforModel(int hpId, long ptId, long ptNum, string name)
        {
            HpId = hpId;
            PtId = ptId;
            ReferenceNo = 0;
            SeqNo = 0;
            PtNum = ptNum;
            KanaName = string.Empty;
            Name = name;
            Sex = 0;
            Birthday = 0;
            LimitConsFlg = 0;
            IsDead = 0;
            DeathDate = 0;
            HomePost = string.Empty;
            HomeAddress1 = string.Empty;
            HomeAddress2 = string.Empty;
            Tel1 = string.Empty;
            Tel2 = string.Empty;
            Mail = string.Empty;
            Setanusi = string.Empty;
            Zokugara = string.Empty;
            Job = string.Empty;
            RenrakuName = string.Empty;
            RenrakuPost = string.Empty;
            RenrakuAddress1 = string.Empty;
            RenrakuAddress2 = string.Empty;
            RenrakuTel = string.Empty;
            RenrakuMemo = string.Empty;
            OfficeName = string.Empty;
            OfficePost = string.Empty;
            OfficeAddress1 = string.Empty;
            OfficeAddress2 = string.Empty;
            OfficeTel = string.Empty;
            OfficeMemo = string.Empty;
            IsRyosyoDetail = 0;
            PrimaryDoctor = 0;
            IsTester = 0;
            MainHokenPid = 0;
            Memo = string.Empty;
            LastVisitDate = 0;
            FirstVisitDate = 0;
            RainCount = string.Empty;
            Comment = string.Empty;
        }

        public PatientInforModel(long ptId, long ptNum, string name, string kanaName, int sex, int birthDay)
        {
            PtId = ptId;
            PtNum = ptNum;
            KanaName = kanaName;
            Name = name;
            Sex = sex;
            Birthday = birthDay;
            HomePost = string.Empty;
            HomeAddress1 = string.Empty;
            HomeAddress2 = string.Empty;
            Tel1 = string.Empty;
            Tel2 = string.Empty;
            Mail = string.Empty;
            Setanusi = string.Empty;
            Zokugara = string.Empty;
            Job = string.Empty;
            RenrakuName = string.Empty;
            RenrakuPost = string.Empty;
            RenrakuAddress1 = string.Empty;
            RenrakuAddress2 = string.Empty;
            RenrakuTel = string.Empty;
            RenrakuMemo = string.Empty;
            OfficeName = string.Empty;
            OfficePost = string.Empty;
            OfficeAddress1 = string.Empty;
            OfficeAddress2 = string.Empty;
            OfficeTel = string.Empty;
            OfficeMemo = string.Empty;
            Memo = string.Empty;
            RainCount = string.Empty;
            Comment = string.Empty;
        }

        public PatientInforModel(int hpId, long ptId, long ptNum, string kanaName, string name, int birthday, int lastVisitDate)
        {
            HpId = hpId;
            PtId = ptId;
            PtNum = ptNum;
            KanaName = kanaName;
            Name = name;
            Birthday = birthday;
            LastVisitDate = lastVisitDate;
            HomePost = string.Empty;
            HomeAddress1 = string.Empty;
            HomeAddress2 = string.Empty;
            Tel1 = string.Empty;
            Tel2 = string.Empty;
            Mail = string.Empty;
            Setanusi = string.Empty;
            Zokugara = string.Empty;
            Job = string.Empty;
            RenrakuName = string.Empty;
            RenrakuPost = string.Empty;
            RenrakuAddress1 = string.Empty;
            RenrakuAddress2 = string.Empty;
            RenrakuTel = string.Empty;
            RenrakuMemo = string.Empty;
            OfficeName = string.Empty;
            OfficePost = string.Empty;
            OfficeAddress1 = string.Empty;
            OfficeAddress2 = string.Empty;
            OfficeTel = string.Empty;
            OfficeMemo = string.Empty;
            Memo = string.Empty;
            RainCount = string.Empty;
            Comment = string.Empty;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long PtNum { get; private set; }

        public string KanaName { get; private set; }

        public string Name { get; private set; }

        public long SeqNo { get; private set; }

        public long ReferenceNo { get; private set; }

        public int Sex { get; private set; }

        public int Birthday { get; private set; }

        public int LimitConsFlg { get; private set; }

        public int IsDead { get; private set; }

        public int DeathDate { get; private set; }

        public string HomePost { get; private set; }

        public string HomeAddress1 { get; private set; }

        public string HomeAddress2 { get; private set; }

        public string Tel1 { get; private set; }

        public string Tel2 { get; private set; }

        public string Mail { get; private set; }

        public string Setanusi { get; private set; }

        public string Zokugara { get; private set; }

        public string Job { get; private set; }

        public string RenrakuName { get; private set; }

        public string RenrakuPost { get; private set; }

        public string RenrakuAddress1 { get; private set; }

        public string RenrakuAddress2 { get; private set; }

        public string RenrakuTel { get; private set; }

        public string RenrakuMemo { get; private set; }

        public string OfficeName { get; private set; }

        public string OfficePost { get; private set; }

        public string OfficeAddress1 { get; private set; }

        public string OfficeAddress2 { get; private set; }

        public string OfficeTel { get; private set; }

        public string OfficeMemo { get; private set; }

        public int IsRyosyoDetail { get; private set; }

        public int PrimaryDoctor { get; private set; }

        public int IsTester { get; private set; }

        public int MainHokenPid { get; private set; }

        public string Memo { get; private set; }

        public int FirstVisitDate { get; private set; }

        public string RainCount { get; private set; }

        public string Comment { get; private set; }

        public int LastVisitDate { get; private set; }
    }
}
