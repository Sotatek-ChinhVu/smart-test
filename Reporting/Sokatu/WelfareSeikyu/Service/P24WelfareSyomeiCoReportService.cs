using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Mapper;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public class P24WelfareSyomeiCoReportService : IP24WelfareSyomeiCoReportService
    {
        #region Constant
        private List<int> kohiHokenNos = new List<int> { 101, 102, 103, 105, 106, 107, 203, 206 };
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoWelfareSeikyuFinder _welfareFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoP24WelfareReceInfModel> receInfs;
        private CoP24WelfareReceInfModel curReceInf;
        private CoHpInfModel hpInf;
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p24WelfareSyomei.rse";

        #region Constructor and Init
        public P24WelfareSyomeiCoReportService(ICoWelfareSeikyuFinder welfareFinder)
        {
            _welfareFinder = welfareFinder;
            _singleFieldData = new();
            _setFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
            _visibleAtPrint = new();
        }
        #endregion

        #region Init properties
        private int hpId;
        private int seikyuYm;
        private SeikyuType seikyuType;
        private List<long> printPtIds;
        private int currentPage;
        private bool hasNextPage;
        #endregion

        public CommonReportingRequestModel GetP24WelfareSyomeiReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<long> printPtIds)
        {
            try
            {
                this.hpId = hpId;
                this.seikyuYm = seikyuYm;
                this.seikyuType = seikyuType;
                this.printPtIds = printPtIds;
                var getData = GetData();

                if (getData)
                {
                    foreach (var receInf in receInfs)
                    {
                        curReceInf = receInf;
                        currentPage = 1;
                        hasNextPage = true;
                        while (getData && hasNextPage)
                        {
                            UpdateDrawForm();
                            currentPage++;
                        }
                    }
                }

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
                _extralData.Add("totalPage", pageIndex.ToString());
                return new WelfareSeikyuMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
            }
            finally
            {
                _welfareFinder.ReleaseResource();
            }
        }

        #region Private function
        private bool UpdateDrawForm()
        {
            bool _hasNextPage = false;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //医療機関コード
                SetFieldData("hpCode", string.Format("241{0, 7}", hpInf.HpCd));
                //医療機関情報
                SetFieldData("postCd", hpInf.PostCdDsp);
                SetFieldData("address1", hpInf.Address1);
                SetFieldData("address2", hpInf.Address2);
                SetFieldData("hpName", hpInf.ReceHpName);
                SetFieldData("hpTel", hpInf.Tel);
                SetFieldData("kaisetuName", hpInf.KaisetuName);
                //提出年月日
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(
                    CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
                );
                SetFieldData("reportGengo", wrkYmd.Gengo);
                SetFieldData("reportYear", wrkYmd.Year.ToString());
                SetFieldData("reportMonth", wrkYmd.Month.ToString());
                SetFieldData("reportDay", wrkYmd.Day.ToString());
                //申請者記入欄
                SetFieldData("seikyuGengo", wrkYmd.Gengo);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

                //市町コード
                SetFieldData("cityCode", curReceInf.CityCode);
                //市町村名
                SetFieldData("cityName", curReceInf.CityName);
                //公費種別
                listDataPerPage.Add(new("kohiSbt", 0, (short)(curReceInf.KohiSbt - 1), "○"));
                //受給者証資格番号
                SetFieldData("jyukyusyaNo", curReceInf.WelfareJyukyusyaNo);
                //氏名
                SetFieldData("ptName", curReceInf.PtName);
                //性別
                listDataPerPage.Add(new("ptSex", (short)(curReceInf.Sex - 1), 0, "○"));
                //生年月日
                listDataPerPage.Add(new("birthGengo", (short)(CIUtil.SDateToWDate(curReceInf.BirthDay) / 1000000 - 1), 0, "○"));
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(curReceInf.BirthDay);
                SetFieldData("birthYear", wrkYmd.Year.ToString());
                SetFieldData("birthMonth", wrkYmd.Month.ToString());
                SetFieldData("birthDay", wrkYmd.Day.ToString());
                //診療月
                wrkYmd = CIUtil.SDateToShowWDate3(curReceInf.SinYm * 100 + 1);
                SetFieldData("sinGengo", wrkYmd.Gengo);
                SetFieldData("sinYear", wrkYmd.Year.ToString());
                SetFieldData("sinMonth", wrkYmd.Month.ToString());
                for (int i = 0; i <= 2; i++)
                {
                    SetFieldData(string.Format("sinGengo{0}", i), wrkYmd.Gengo);
                }
                //一部負担割合
                listDataPerPage.Add(new("hokenRate", (short)(curReceInf.HokenRate / 10 - 1), 0, "○"));
                //保険請求点数
                SetFieldData("tensu", curReceInf.Tensu.ToString());
                //一部負担額
                SetFieldData("futan", curReceInf.HokenReceFutan.ToString());

                //公費・長 区分、公費請求点数、公費・長 一部負担額
                if (curReceInf.IsChoki)
                {
                    SetFieldData("kohiKbn", "99");
                    SetFieldData("kohiTensu", curReceInf.KohiReceTensu(1).ToString());
                    SetFieldData("kohiFutan", curReceInf.Kohi1Limit.ToString());
                }
                else
                {
                    string kohiHoubetu = "";
                    int kohiTensu = 0;
                    int kohiFutan = 0;
                    for (int i = 1; i <= 4; i++)
                    {
                        if (curReceInf.KohiReceKisai(i) == 1 && curReceInf.KohiReceTensu(i) > kohiTensu)
                        {
                            //公費が複数ある場合は公費請求点数の高い方の法別番号を記録する
                            if (curReceInf.KohiReceTensu(i) > kohiTensu)
                            {
                                kohiHoubetu = curReceInf.KohiHoubetu(i);
                            }
                            //公費が複数ある場合は公費請求点数を合算して記録する
                            kohiTensu += curReceInf.KohiReceTensu(i);
                            //公費が複数ある場合は公費一部負担額を合算して記録する
                            kohiFutan += curReceInf.KohiReceFutan(i);
                        }
                    }
                    if (kohiTensu > 0)
                    {
                        SetFieldData("kohiKbn", kohiHoubetu);
                        SetFieldData("kohiTensu", kohiTensu.ToString());
                        SetFieldData("kohiFutan", kohiFutan.ToString());
                    }
                }

                //処方せん発行区分
                listDataPerPage.Add(new("outDrug", (short)(curReceInf.IsOutDrug ? 0 : 1), 0, "○"));
                _listTextData.Add(pageIndex, listDataPerPage);
                return 1;
            }
            #endregion

            #endregion

            if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
            {
                hasNextPage = _hasNextPage;
                return false;
            }

            hasNextPage = _hasNextPage;
            return true;
        }

        private bool GetData()
        {
            hpInf = _welfareFinder.GetHpInf(hpId, seikyuYm);
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHokenNos, FutanCheck.None, 0);

            //患者指定がある場合は絞り込み
            wrkReces = (printPtIds?.Count ?? 0) == 0 ? wrkReces.ToList() :
                wrkReces.Where(r => printPtIds.Contains(r.PtId)).ToList();

            //三重県用のモデルクラスにコピー
            receInfs = wrkReces
                .Select(x => new CoP24WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHokenNos))
                .OrderBy(r => r.CityCode)
                .ThenBy(r => r.KohiSbt)
                .ThenBy(r => r.WelfareJyukyusyaNo)
                .ToList();

            //処方せん発行区分
            foreach (var receInf in receInfs)
            {
                receInf.IsOutDrug = _welfareFinder.IsOutDrugOrder(hpId, receInf.PtId, receInf.SinYm);
            }

            return (receInfs?.Count ?? 0) > 0;
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }
        #endregion
    }
}
