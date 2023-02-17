using Domain.Constant;
using Entity.Tenant;
using Helper.Common;

namespace Reporting.Byomei.Models
{
    public class CoByomeiModel 
    {
        public PtByomei PtByomei { get; private set; }

        public CoByomeiModel(PtByomei ptByomei)
        {
            PtByomei = ptByomei;
        }

        public string ByomeiName
        {
            get
            {
                string ret = PtByomei.Byomei ?? string.Empty;
                
                if(PtByomei.SyubyoKbn == 1)
                {
                    ret = "（主）" + ret;
                }

                if(string.IsNullOrEmpty(PtByomei.HosokuCmt) == false)
                {
                    ret += "（" + PtByomei.HosokuCmt.Trim() + "）";
                }

                return ret;
            }
        }
        public string StartDate
        {
            get => CIUtil.SDateToShowSWDate(PtByomei.StartDate);
        }
        public string TenkiDate
        {
            get => CIUtil.SDateToShowSWDate(PtByomei.TenkiDate);
        }

        public string DisplayTenki
        {
            get
            {
                if (this.PtByomei.TenkiKbn < TenkiKbnConst.Continued) return string.Empty;
                return TenkiKbnConst.DisplayedTenkiKbnDict[PtByomei.TenkiKbn];
            }
        }
    }
}
