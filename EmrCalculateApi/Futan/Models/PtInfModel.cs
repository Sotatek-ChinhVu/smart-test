using Entity.Tenant;
using Helper.Common;

namespace EmrCalculateApi.Futan.Models
{
    public class PtInfModel
    {
        public PtInf PtInf { get; }

        public PtInfModel(PtInf ptInf, int sinDate)
        {
            PtInf = ptInf;
            _sinDate = sinDate;
        }

        private readonly int _sinDate;

        /// <summary>
        /// 生年月日
        ///  yyyymmdd 
        /// </summary>
        public int Birthday
        {
            get { return PtInf.Birthday; }
        }

        public bool IsPreSchool()
        {
            return !CIUtil.IsStudent(Birthday, _sinDate);
        }

        public bool IsElder()
        {
            return CIUtil.AgeChk(Birthday, _sinDate, 70);
        }

        public bool IsElder20per()
        {
            return CIUtil.Is70Zenki_20per(Birthday, _sinDate);
        }

        public bool IsElderExpat()
        {
            //75歳以上で海外居住者の方は後期高齢者医療には加入せず、
            //協会、健保組合に加入することになり、高齢受給者証を提示した場合、
            //H26.5診療分からは所得に合わせ2割または3割負担となる。
            return CIUtil.AgeChk(Birthday, _sinDate, 75) && _sinDate >= 20140501;
        }

        public bool IsAge(int age)
        {
            return CIUtil.AgeChk(Birthday, _sinDate, age);
        }

        public int Age()
        {
            return CIUtil.SDateToAge(Birthday, _sinDate);
        }
    }
}
