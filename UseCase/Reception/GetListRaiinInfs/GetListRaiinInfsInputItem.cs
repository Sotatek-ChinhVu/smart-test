using Domain.Constant;
using Entity.Tenant;
using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UseCase.Reception.GetListRaiinInfs
{
    public class GetListRaiinInfsInputItem
    {
        public GetListRaiinInfsInputItem(int hpId, long ptId, int sinDate, int uketukeNo, int status, string kaSname, string sName, string houbetu, string hokensyaNo, int hokenKbn, int hokenId, int hokenPid, long raiinNo)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            UketukeNo = uketukeNo;
            Status = status;
            KaSname = kaSname;
            SName = sName;
            Houbetu = houbetu;
            HokensyaNo = hokensyaNo;
            HokenKbn = hokenKbn;
            HokenId = hokenId;
            HokenPid = hokenPid;
            RaiinNo = raiinNo;
        }

        public int HpId { get; private set;}

        public long PtId { get; private set;}

        public string SinDateLabel
        {
            get { return CIUtil.SDateToShowSDate(SinDate); }
        }

        public int UketukeNo { get; private set; }

        public string StatusLbl
        {
            get
            {
                if (CheckDefaultValue())
                {
                    return string.Empty;
                }
                string result = string.Empty;
                switch (Status)
                {
                    case 0:
                        result = "予約";
                        break;
                    case 1:
                        result = string.Empty;
                        break;
                    case 3:
                        result = "一時保存";
                        break;
                    case 5:
                        result = "計算";
                        break;
                    case 7:
                        result = "精算待ち";
                        break;
                    case 9:
                        result = "精算済";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

        public string KaSname { get; private set; }

        public string SName { get; private set; }

        public int SinDate { get; private set;} 

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

        public int Status { get; private set; }

        public string Houbetu { get; private set; }

        public string HokensyaNo { get; private set; }

        public int HokenKbn { get; private set; }

        public int HokenId { get; private set; }

        public bool CheckDefaultValue()
        {
            return PtId == 0 && SinDate == 0 && RaiinNo == 0;
        }

        public long RaiinNo { get; private set; }

        public int HokenPid { get; private set; }
    }
}
