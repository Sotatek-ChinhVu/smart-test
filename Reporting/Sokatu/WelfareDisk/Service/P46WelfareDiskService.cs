using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareDisk.Service
{
    public class P46WelfareDiskService : IP46WelfareDiskService
    {
        #region Constant
        private List<string> kohiHoubetus = new List<string> { "99" };
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoWelfareReceInfModel> receInfs;
        private CoHpInfModel hpInf;
        #endregion

        #region Constructor and Init
        public P46WelfareDiskService(ICoWelfareSeikyuFinder welfareFinder)
        {
            _welfareFinder = welfareFinder;
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        #endregion


        public CommonExcelReportingModel GetDataP46WelfareDisk(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            this.hpId = hpId;
            this.seikyuType = seikyuType;
            this.seikyuYm = seikyuYm;
            GetData();

            #region Header
            List<string> headerDatas = new List<string>();
            //データ区分
            headerDatas.Add("1");
            //診療年月
            string seiYm = CIUtil.SDateToWDate(seikyuYm * 100 + 1).ToString();
            headerDatas.Add(seiYm.Substring(0, 5));
            //医療機関番号
            string hpCd = "\"" + "46" + "1" + hpInf.HpCd + "\"";
            headerDatas.Add(hpCd);
            //事業番号
            headerDatas.Add("1");
            //件数
            int count = receInfs.Count();
            headerDatas.Add(count.ToString());
            //医療機関名称
            headerDatas.Add("\"" + CIUtil.Copy(hpInf.ReceHpName, 1, 30) + "\"");
            #endregion

            List<string> retDatas = new();

            if (GetData())
            {
                retDatas.Add(string.Join(",", headerDatas));

                foreach (var receInf in receInfs)
                {
                    retDatas.Add(RecordData(receInf));
                }
            }

            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd")));
            string sheetName = string.Format("461{0}-1-{1}{2}", hpInf.HpCd, wrkYmd.Year.ToString("D2"), wrkYmd.Month.ToString("D2"));
            return new CommonExcelReportingModel(sheetName + ".csv", sheetName, retDatas);
        }

        #region SubMethod
        string RecordData(CoWelfareReceInfModel receInf)
        {
            List<string> colDatas = new List<string>();

            //レコード区分
            colDatas.Add("2");
            //医療機関番号
            string hpCd = "\"" + "46" + "1" + hpInf.HpCd + "\"";
            colDatas.Add(hpCd);
            //診療科コード
            colDatas.Add("");
            //診療年月
            string seiYm = CIUtil.SDateToWDate(seikyuYm * 100 + 1).ToString();
            colDatas.Add(seiYm.Substring(0, 5));
            //市町村番号
            string townNo = receInf.JyukyusyaNo(kohiHoubetus).PadLeft(2, '0');
            colDatas.Add(townNo.Substring(townNo.Length - 2));
            //事業番号
            colDatas.Add("1");
            //乳幼児資格者証受給者番号
            string tokusyuNo = receInf.TokusyuNo(kohiHoubetus).PadLeft(9, '0');
            colDatas.Add("\"" + tokusyuNo.Substring(tokusyuNo.Length - 9) + "\"");
            //氏名
            colDatas.Add("\"" + receInf.PtName + "\"");
            //生年月日
            colDatas.Add(CIUtil.SDateToWDate(receInf.BirthDay).ToString());
            //保険区分
            switch (receInf.HokenKbn)
            {
                case HokenKbn.Syaho:
                    colDatas.Add("2");
                    break;
                case HokenKbn.Kokho:
                    colDatas.Add("1");
                    break;
                default:
                    colDatas.Add("");
                    break;
            }
            //保険者番号
            colDatas.Add(receInf.HokensyaNo);
            //入院・入院外区分
            colDatas.Add("2");
            //割合
            colDatas.Add((receInf.HokenRate / 10).ToString());
            //実日数
            colDatas.Add(receInf.HokenNissu.ToString());
            //合計点数
            colDatas.Add(receInf.Tensu.ToString());
            //自己負担支払額
            colDatas.Add(receInf.PtFutan.ToString());
            //実診療年月
            colDatas.Add((CIUtil.SDateToWDate(receInf.SinYm * 100 + 1) / 100).ToString());
            //公費番号１・２
            const int maxKohi = 4, maxRow = 2;
            short kohi = 1, row = 0;
            while ((kohi <= maxKohi) && (row < maxRow))
            {
                if ((!string.IsNullOrEmpty(receInf.KohiHoubetu(kohi))) && (!kohiHoubetus.Contains(receInf.KohiHoubetu(kohi))) && (receInf.KohiReceKisai(kohi) == 1))
                {
                    colDatas.Add(receInf.KohiHoubetu(kohi));
                    row++;
                }
                kohi++;
            }
            //公費番号が0件の時は、一件目に0、二件目に空欄を出力
            for (; row < maxRow; row++)
            {
                if (row == 0) colDatas.Add("0");
                else colDatas.Add("");
            }

            return string.Join(",", colDatas);
        }
        #endregion

        #region Private function
        private bool GetData()
        {
            hpInf = _welfareFinder.GetHpInf(hpId, seikyuYm);
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.None, 0);
            //自己負担額がある人のみ
            //第一ソート市町村番号順(JyukyusyaNo)、第二ソート受給者番号順(TokusyuNo)
            receInfs = wrkReces.Where(r => r.PtFutan > 0).OrderBy(r => r.JyukyusyaNo(kohiHoubetus)).ThenBy(r => r.TokusyuNo(kohiHoubetus)).ToList();

            return (receInfs?.Count ?? 0) > 0;
        }
        #endregion
    }
}
