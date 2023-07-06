using Reporting.Mappers.Common;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Reporting.Sokatu.WelfareSeikyu.Models.CoP43WelfareReceInfModel2;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public class P43AmakusaSeikyu41CoReportService : IP43AmakusaSeikyu41CoReportService
    {
        #region Constant
        private List<string> kohiHoubetus = new List<string> { "41" };
        private List<CityKohi> kohiHokens = new List<CityKohi> {
            new CityKohi() { HokenNo = 141, HokenEdaNo = 5 }
        };

        private struct countData
        {
            public int KohiNissu;
            public int KohiTensu;
            public int IchibuFutan;
        }
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
        public P43AmakusaSeikyu41CoReportService(ICoWelfareSeikyuFinder welfareFinder)
        {
            _welfareFinder = welfareFinder;
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        #endregion

        public CommonReportingRequestModel GetP43AmakusaSeikyu41sReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            throw new NotImplementedException();
        }

        #region Private function
        private bool UpdateDrawForm()
        {
            int maxRow = CoRep.GetListRowCount("jyukyusyaNo");
            bool _hasNextPage = true;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //医療機関コード
                CoRep.SetFieldData("hpCode", hpInf.HpCd);
                //医療機関情報
                CoRep.SetFieldData("address1", hpInf.Address1);
                CoRep.SetFieldData("address2", hpInf.Address2);
                CoRep.SetFieldData("hpName", hpInf.ReceHpName);
                CoRep.SetFieldData("kaisetuName", hpInf.KaisetuName);
                CoRep.SetFieldData("hpTel", hpInf.Tel);
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
                CoRep.SetFieldData("seikyuGengo", wrkYmd.Gengo);
                CoRep.SetFieldData("seikyuYear", wrkYmd.Year);
                CoRep.SetFieldData("seikyuMonth", wrkYmd.Month);
                //提出年月日
                wrkYmd = CIUtil.SDateToShowWDate3(
                    CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
                );
                CoRep.SetFieldData("reportGengo", wrkYmd.Gengo);
                CoRep.SetFieldData("reportYear", wrkYmd.Year);
                CoRep.SetFieldData("reportMonth", wrkYmd.Month);
                CoRep.SetFieldData("reportDay", wrkYmd.Day);
                //ページ数
                CoRep.SetFieldData("page", CurrentPage);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                int ptIndex = (CurrentPage - 1) * maxRow;

                countData totalData = new countData();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var curReceInf = receInfs[ptIndex];

                    //受給者番号
                    CoRep.ListText("jyukyusyaNo", 0, rowNo, curReceInf.JyukyusyaNo);

                    //氏名
                    CoRep.ListText("ptName", 0, rowNo, curReceInf.PtName);
                    //生年月日
                    CoRep.ListText("birthday", 0, rowNo, curReceInf.BirthDayW);

                    //実日数
                    CoRep.ListText("nissu", 0, rowNo, curReceInf.KohiNissu);
                    totalData.KohiNissu += curReceInf.KohiNissu;
                    //総点数
                    CoRep.ListText("tensu", 0, rowNo, curReceInf.KohiTensu);
                    totalData.KohiTensu += curReceInf.KohiTensu;
                    //公費負担額
                    CoRep.ListText("kohiFutan", 0, rowNo, 0);
                    //一部負担額
                    CoRep.ListText("futan", 0, rowNo, curReceInf.IchibuFutan);
                    totalData.IchibuFutan += curReceInf.IchibuFutan;
                    //負担割合
                    CoRep.ListText("futanRate", 0, rowNo, curReceInf.HokenRate);

                    ptIndex++;
                    if (ptIndex >= receInfs.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                //小計
                short wrkRow = (short)maxRow;
                CoRep.ListText("nissu", 0, wrkRow, totalData.KohiNissu);
                CoRep.ListText("tensu", 0, wrkRow, totalData.KohiTensu);
                CoRep.ListText("kohiFutan", 0, wrkRow, 0);
                CoRep.ListText("futan", 0, wrkRow, totalData.IchibuFutan);

                if (!_hasNextPage)
                {
                    //合計
                    wrkRow = (short)(maxRow + 1);
                    CoRep.ListText("nissu", 0, wrkRow, receInfs.Sum(r => r.KohiNissu));
                    CoRep.ListText("tensu", 0, wrkRow, receInfs.Sum(r => r.KohiTensu));
                    CoRep.ListText("kohiFutan", 0, wrkRow, 0);
                    CoRep.ListText("futan", 0, wrkRow, receInfs.Sum(r => r.IchibuFutan));
                }

                return 1;
            }
            #endregion

            #endregion

            try
            {
                if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
                {
                    hasNextPage = _hasNextPage;
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.WriteLogError(ModuleName, this, nameof(UpdateDrawForm), e);
                hasNextPage = _hasNextPage;
                return false;
            }

            hasNextPage = _hasNextPage;
            return true;
        }

        private bool GetData()
        {
            hpInf = welfareFinder.GetHpInf(seikyuYm);
            var wrkReces = welfareFinder.GetReceInf(seikyuYm, seikyuType, kohiHoubetus, FutanCheck.KohiFutan, -1);
            //熊本県用のモデルにコピー
            receInfs = wrkReces.Select(x => new CoP43WelfareReceInfModel2(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHokens)).ToList();
            //天草市こども医療費の対象に絞る
            receInfs = receInfs.Where(x => x.IsWelfare).OrderBy(x => x.JyukyusyaNo.PadLeft(7, '0')).ToList();

            return (receInfs?.Count ?? 0) > 0;
        }
        #endregion
    }
}
