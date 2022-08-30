using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class SinRpInfModel
    {
        public SinRpInf SinRpInf { get; }

        public SinRpInfModel(SinRpInf sinRpInf)
        {
            SinRpInf = sinRpInf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SinRpInf.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return SinRpInf.PtId; }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return SinRpInf.SinYm; }
        }

        /// <summary>
        /// 剤番号
        /// SEQUENCE
        /// </summary>
        public int RpNo
        {
            get { return SinRpInf.RpNo; }
        }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        public int HokenKbn
        {
            get { return SinRpInf.HokenKbn; }
        }

        /// <summary>
        /// 算定区分
        /// 1:自費算定
        /// </summary>
        public int SanteiKbn
        {
            get { return SinRpInf.SanteiKbn; }
        }
    }
}
