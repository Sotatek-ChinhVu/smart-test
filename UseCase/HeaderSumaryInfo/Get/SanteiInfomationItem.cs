using Domain.Models.OrdInfDetails;
using Domain.Models.SanteiInfo;
using Domain.Models.TenMst;
using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class SanteiInfomationItem
    {
        public SanteiInfoModel? SanteiInf { get; }
        public SanteiInfoDetailModel? SanteiInfDetail { get; }
        public TenMstModel? TenMst { get; }
        public OrdInfDetailModel? OdrInfDetail { get; }

        public SanteiInfomationItem(SanteiInfoModel? santeiInf, SanteiInfoDetailModel? santeiInfDetail, TenMstModel? tenMst, OrdInfDetailModel? odrInfDetail, int sinDate)
        {
            SanteiInf = santeiInf;
            SanteiInfDetail = santeiInfDetail;
            TenMst = tenMst;
            OdrInfDetail = odrInfDetail;
            SinDate = sinDate;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SanteiInf?.HpId ?? 0; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return SanteiInf?.PtId ?? 0; }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return SanteiInf?.ItemCd ?? String.Empty; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public int SeqNo
        {
            get { return SanteiInf?.SeqNo ?? 0; }
        }

        /// <summary>
        /// 警告日数
        /// 
        /// </summary>
        public int AlertDays
        {
            get { return SanteiInf?.AlertDays ?? 0; }
        }

        /// <summary>
        /// 警告単位
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        public int AlertTerm
        {
            get { return SanteiInf?.AlertTerm ?? 0; }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public long Id
        {
            get { return SanteiInf?.Id ?? 0; }
        }

        public string ItemName
        {
            get
            {
                if (TenMst != null)
                {
                    return TenMst.Name;
                }
                return string.Empty;
            }
        }

        public int LastOdrDate
        {
            get
            {
                if (SanteiInfDetail != null)
                {
                    return SanteiInfDetail?.KisanDate ?? 0;
                }
                else if (OdrInfDetail != null)
                {
                    return OdrInfDetail?.SinDate ?? 0;
                }
                return 0;
            }
        }

        public string KisanType
        {
            get
            {
                if (SanteiInfDetail != null)
                {
                    return GetKisanName(SanteiInfDetail?.KisanSbt);
                }
                return "前回日";
            }
        }

        private string GetKisanName(int? kisanSbt)
        {
            switch (kisanSbt)
            {
                case 1:
                    return "初回日";
                case 2:
                    return "発症日";
                case 3:
                    return "急性増悪";
                case 4:
                    return "治療開始";
                case 5:
                    return "手術日";
                default:
                    return "前回日";
            }
        }

        public int SinDate { get; set; }

        public int DayCount
        {
            get
            {
                return CIUtil.GetSanteInfDayCount(SinDate, LastOdrDate, AlertTerm);
            }
        }

        public string DayCountDisplay
        {
            get
            {
                string dayCountDisplay = string.Empty;
                if (DayCount != 0)
                {
                    switch (AlertTerm)
                    {
                        case 2:
                            dayCountDisplay = DayCount + "日";
                            break;
                        case 3:
                            dayCountDisplay = DayCount + "週";
                            break;
                        case 4:
                            dayCountDisplay = DayCount + "ヶ月";
                            break;
                        case 5:
                            dayCountDisplay = DayCount + "週";
                            break;
                        case 6:
                            dayCountDisplay = DayCount + "ヶ月";
                            break;
                    }
                }
                return dayCountDisplay;
            }
        }
    }
}
