using Entity.Tenant;
using Helper.Common;

namespace Reporting.Byomei.Models
{
    public class CoPtByomeiModel 
    {
        public PtInf PtInf { get; private set; }

        public CoPtHokenInfModel PtHokenInfModel { get; private set; }

        public CoPtByomeiModel(int fromDate, int toDate, PtInf PtInf, CoPtHokenInfModel ptHokenInfModel, IEnumerable<PtByomei> ptByomeis)
        {
            this.PtInf = PtInf;
            PtHokenInfModel = ptHokenInfModel;
            ListByomei = ptByomeis.Select(b => new CoByomeiModel(b)).ToList();
            FromDay = fromDate;
            ToDay = toDate;
        }

        public int FromDay { get; set; }

        public int ToDay { get; set; }

        public long PtNum => PtInf.PtNum;

        public string KanjiName => PtInf.Name ?? string.Empty;

        public string KanaName => PtInf.KanaName ?? string.Empty;

        public string Sex
        {
            get
            {
                switch(PtInf.Sex)
                {
                    case 1:
                        return "男";
                    case 2:
                        return "女";
                    default:
                        return string.Empty;
                }
            }
        }

        public int BirthYmd
        {
            get => PtInf.Birthday;
        }

        public string BirthDay
        {
            get => CIUtil.SDateToShowWDate2(PtInf.Birthday);
        }

        public string HokenPatternName
        {
            get
            {
                string ret = "";

                if(PtHokenInfModel != null)
                {
                    if(new int[] { 0 }.Contains(PtHokenInfModel.HokenKbn ))
                    {
                        ret = "自費";
                    }
                    else if (new int[] { 1 }.Contains(PtHokenInfModel.HokenKbn))
                    {
                        ret = "社保";
                    }
                    else if (new int[] { 2 }.Contains(PtHokenInfModel.HokenKbn))
                    {
                        if (PtHokenInfModel.Houbetu == "39")
                        {
                            ret = "後期";
                        }
                        else if (PtHokenInfModel.Houbetu == "67")
                        {
                            ret = "退職";
                        }
                        else
                        {
                            ret = "国保";
                        }
                    }
                    else if (new int[] { 11, 12, 13 }.Contains(PtHokenInfModel.HokenKbn))
                    {
                        ret = "労災";
                    }
                    else if (new int[] { 14 }.Contains(PtHokenInfModel.HokenKbn))
                    {
                        ret = "自賠";
                    }
                }

                return ret;
            }

        }

        public List<CoByomeiModel> ListByomei { get; set; }
    }
}
