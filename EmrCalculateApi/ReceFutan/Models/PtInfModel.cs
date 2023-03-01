using Entity.Tenant;
using Helper.Common;

namespace EmrCalculateApi.ReceFutan.Models
{
    public class PtInfModel
    {
        public PtInf PtInf { get; }

        public PtInfModel(PtInf ptInf)
        {
            PtInf = ptInf;
        }

        /// <summary>
        /// 患者ID
        ///  患者を識別するためのシステム固有の番号       
        /// </summary>
        public long PtId
        {
            get { return PtInf.PtId; }
        }

        /// <summary>
        /// 生年月日
        ///  yyyymmdd 
        /// </summary>
        public int Birthday
        {
            get { return PtInf.Birthday; }
        }

        /// <summary>
        /// テスト患者区分
        ///		1:テスト患者
        /// </summary>
        public int IsTester
        {
            get { return PtInf.IsTester; }
        }

        public bool IsAge(int age, int sinDate)
        {
            return CIUtil.AgeChk(Birthday, sinDate, age);
        }
    }

}
