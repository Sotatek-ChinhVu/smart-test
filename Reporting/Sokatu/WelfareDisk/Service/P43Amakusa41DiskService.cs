using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;
using static Reporting.Sokatu.WelfareSeikyu.Models.CoP43WelfareReceInfModel2;

namespace Reporting.Sokatu.WelfareDisk.Service
{
    public class P43Amakusa41DiskService : IP43Amakusa41DiskService
    {
        #region Constant
        private List<string> kohiHoubetus = new List<string> { "41" };
        private List<CityKohi> kohiHokens = new List<CityKohi> {
            new CityKohi() { HokenNo = 141, HokenEdaNo = 5 }
        };

        private List<string> csvTitles = new List<string>
        {
            "受給者証番号",
            "対象者氏名",
            "生年月日",
            "入院/実日数",
            "入院/総点数",
            "入院/公費負担額",
            "入院/一部負担額",
            "外来/実日数",
            "外来/総点数",
            "外来/公費負担額",
            "外来/一部負担額",
            "備考/負担割合"
        };
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoP43WelfareReceInfModel2> receInfs;
        private CoHpInfModel hpInf;
        #endregion

        #region Constructor and Init
        public P43Amakusa41DiskService(ICoWelfareSeikyuFinder welfareFinder)
        {
            _welfareFinder = welfareFinder;
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        private List<string> retDatas = new();
        #endregion

        public CommonExcelReportingModel GetDataP43Amakusa41Disk(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            this.hpId = hpId;
            this.seikyuType = seikyuType;
            this.seikyuYm = seikyuYm;
            GetData();
            if (GetData())
            {
                retDatas = new List<string>
                {
                    "\"" + string.Join("\",\"", csvTitles) + "\""
                };

                foreach (var receInf in receInfs)
                {
                    retDatas.Add(RecordData(receInf));
                }
            }

            string sheetName = string.Format("天草市子ども医療費請求書_{0}", seikyuYm);

            return new CommonExcelReportingModel(sheetName + ".csv", sheetName, retDatas);
        }

        #region SubMethod
        string RecordData(CoP43WelfareReceInfModel2 receInf)
        {
            List<string> colDatas = new List<string>
            {
                //受給者証番号
                receInf.JyukyusyaNo,
                //対象者氏名
                receInf.PtName,
                //生年月日
                receInf.BirthDayW,
                //入院/実日数
                "",
                //入院/総点数
                "",
                //入院/公費負担額
                "",
                //入院/一部負担額
                "",
                //外来/実日数
                receInf.KohiNissu.ToString(),
                //外来/総点数
                receInf.KohiTensu.ToString(),
                //外来/公費負担額
                "0",
                //外来/一部負担額
                receInf.IchibuFutan.ToString(),
                //備考/負担割合
                receInf.HokenRate
            };

            return "\"" + string.Join("\",\"", colDatas) + "\"";
        }
        #endregion

        #region Private function
        private bool GetData()
        {
            hpInf = _welfareFinder.GetHpInf(hpId, seikyuYm);
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, -1);
            //熊本県用のモデルにコピー
            receInfs = wrkReces.Select(x => new CoP43WelfareReceInfModel2(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHokens)).ToList();
            //天草市こども医療費の対象に絞る
            receInfs = receInfs.Where(x => x.IsWelfare).OrderBy(x => x.JyukyusyaNo.PadLeft(7, '0')).ToList();

            return (receInfs?.Count ?? 0) > 0;
        }
        #endregion
    }
}
