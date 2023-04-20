using Helper.Common;
using Helper.Constants;

namespace UseCase.Todo.GetTodoInfFinder
{
    public class GetListTodoInfFinderOutputItem
    {

        public GetListTodoInfFinderOutputItem(int hpId, long ptId, long ptNum, string patientName, int sinDate, string primaryDoctorName, string kaSname, string todoKbnName, string cmt1, DateTime createDate, string createrName, string tantoName, string cmt2, DateTime updateDate, string updaterName, string todoGrpName, int term, int hokenPid, string houbetu, int hokenKbn, string hokensyaNo, int hokenId)
        {
            HpId = hpId;
            PtId = ptId;
            PtNum = ptNum;
            SinDate = sinDate;
            PatientName = patientName;
            PrimaryDoctorName = primaryDoctorName;
            KaSname = kaSname;
            TodoKbnName = todoKbnName;
            TantoName = tantoName;
            Cmt2 = cmt2;
            CreateDate = createDate;
            UpdaterName = updaterName;
            Cmt1 = cmt1;
            CreaterName = createrName;
            TodoGrpName = todoGrpName;
            Term = term;
            HokenPid = hokenPid;
            Houbetu = houbetu;
            HokenKbn = hokenKbn;
            HokensyaNo = hokensyaNo;
            HokenId = hokenId;
            UpdateDate = updateDate;
        }

        public long PtNum { get; private set; }

        public string PatientName { get; private set; }

        public string DisplaySinDate
        {
            get { return CIUtil.SDateToShowSDate(SinDate); }
        }

        public string PrimaryDoctorName { get; private set; }

        public string KaSname { get; private set; }

        public string HokenKbnName
        {
            get
            {
                string result = string.Empty;
                if (PtId == 0 && HokenPid == 0 && HpId == 0)
                {
                    return string.Empty;
                }
                if (PtId == 0 && HokenId == 0 && HpId == 0)
                {
                    result = "公費";
                    return result;
                }
                if (Houbetu == HokenConstant.HOUBETU_NASHI)
                {
                    result = "公費";
                    return result;
                }
                switch (HokenKbn)
                {
                    case 0:
                        result = "自費";
                        break;
                    case 1:
                        result = "社保";
                        break;
                    case 2:
                        if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("39"))
                        {
                            result = "後期";
                        }
                        else if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("67"))
                        {
                            result = "退職";
                        }
                        else
                        {
                            result = "国保";
                        }
                        break;
                    case 11:
                    case 12:
                    case 13:
                        result = "労災";
                        break;
                    case 14:
                        result = "自賠";
                        break;
                }
                return result;
            }
        }

        public string TodoKbnName { get; private set; }

        public string Cmt1 { get; private set; }


        public string CreateTimeDisplay
        {
            get { return CreateDate.ToString(); }
        }

        public string CreaterName { get; private set; }

        public string TantoName { get; private set; }

        public string Cmt2 { get; private set; }

        public string UpdatTimeDisplay
        {
            get { return UpdateDate.ToString(); }
        }

        public string UpdaterName { get; private set; }

        public string TodoGrpName { get; private set; }

        public string DisplayTerm
        {
            get { return CIUtil.SDateToShowSDate(Term); }
        }

        public DateTime CreateDate { get; private set; }

        public DateTime UpdateDate { get; private set; }

        public int Term { get; private set; }

        public string Houbetu { get; private set; }

        public string HokensyaNo { get; private set; }

        public int HokenKbn { get; private set; }

        public int HokenId { get; private set; }

        public int HokenPid { get; private set; }

        public int SinDate { get; private set; }

        public int HpId { get; private set; }

        public long PtId { get; private set; }
    }
}
