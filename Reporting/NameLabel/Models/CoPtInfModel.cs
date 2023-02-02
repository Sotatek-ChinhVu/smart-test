using Entity.Tenant;

namespace Reporting.NameLabel.Models
{
    public class CoPtInfModel
    {
        public PtInf PtInf { get; } = null;

        public CoPtInfModel(PtInf ptInf)
        {
            PtInf = ptInf;
        }

        /// <summary>
        /// 患者番号
        /// </summary>

        public long PtNum
        {
            get { return PtInf.PtNum; }
        }

        /// <summary>
        /// 患者カナ
        /// </summary>

        public string KanaName
        {
            get { return PtInf.KanaName; }
        }

        /// <summary>
        /// 患者氏名
        /// </summary>

        public string Name
        {
            get { return PtInf.Name; }
        }

        /// <summary>
        /// 生年月日
        /// </summary>

        public int BirthDay
        {
            get { return PtInf.Birthday; }
        }

        /// <summary>
        /// 性別
        /// </summary>

        public int Sex
        {
            get { return PtInf.Sex; }
        }
    }
}
