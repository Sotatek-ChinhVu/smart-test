namespace Domain.Models.PatientInfor
{
    namespace Domain.Models.PatientInfor
    {
        public class PatientInforModel
        {
            public PatientInforModel(int hpId, long ptId, long referenceNo, long seqNo, long ptNum, string kanaName, string name, int sex, int birthday, int limitConsFlg, int isDead, int deathDate, string homePost, string homeAddress1, string homeAddress2, string tel1, string tel2, string mail, string setanusi, string zokugara, string job, string renrakuName, string renrakuPost, string renrakuAddress1, string renrakuAddress2, string renrakuTel, string renrakuMemo, string officeName, string officePost, string officeAddress1, string officeAddress2, string officeTel, string officeMemo, int isRyosyoDetail, int primaryDoctor, int isTester, int mainHokenPid, string memo, int lastVisitDate, string setainusi)
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
                Setainusi = setainusi;
            }

            public int LastVisitDate { get; private set; }

            public int HpId { get; private set; }

            public long PtId { get; private set; }

            public long SeqNo { get; private set; }

            public long ReferenceNo { get; private set; }

            public long PtNum { get; private set; }

            public string KanaName { get; private set; }

            public string Name { get; private set; }

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

            public string Setainusi { get; set; }
        }
    }
}
