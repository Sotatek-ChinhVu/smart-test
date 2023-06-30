using Entity.Tenant;
using Helper.Extension;

namespace Domain.Models.SinKoui
{
    public class KaikeiInfModel
    {
        public KaikeiInf KaikeiInf { get; }

        public KaikeiInfModel(KaikeiInf kaikeiInf)
        {
            KaikeiInf = kaikeiInf;
        }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId => KaikeiInf.PtId;

        /// <summary>
        /// 診療日
        /// </summary>
        public int SinDate => KaikeiInf.SinDate;

        public int SinYm => SinDate / 100;

        public string SinYmText => SinYm.AsString();

        public string SinYmBinding => SinYmText.Length == 6 ? SinYmText.Insert(4, "/") : "";

        /// <summary>
        /// 保険ID
        ///     PT_HOKEN_INF.HOKEN_ID
        /// </summary>
        public int HokenId => KaikeiInf.HokenId;
    }
}
