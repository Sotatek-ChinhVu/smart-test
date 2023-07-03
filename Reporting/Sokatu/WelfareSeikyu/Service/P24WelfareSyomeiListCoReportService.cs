using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Models;
using Reporting.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public class P24WelfareSyomeiListCoReportService : IP24WelfareSyomeiListCoReportService
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
        private CoHpInfModel hpInf;
        private List<string> cityCodes;
        string currentCityCode;
        #endregion

        #region Constructor and Init
        public P24WelfareSyomeiListCoReportService(ICoWelfareSeikyuFinder welfareFinder)
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
        private int currentPage;
        private bool hasNextPage;
        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "p24WelfareSyomeiList.rse";

        public CommonReportingRequestModel GetP24WelfareSyomeiListReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            var getData = GetData();

            foreach (string currentCode in cityCodes)
            {
                currentCityCode = currentCode;
                currentPage = 1;
                hasNextPage = true;

                while (getData && hasNextPage)
                {
                    UpdateDrawForm();
                    currentPage++;
                }
            }
        }
        #region Private function
        private bool UpdateDrawForm()
        {
            bool _hasNextPage = true;

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
                CoRep.SetFieldData("reportYear", wrkYmd.Year);
                CoRep.SetFieldData("reportMonth", wrkYmd.Month);
                CoRep.SetFieldData("reportDay", wrkYmd.Day);
                //申請者記入欄
                CoRep.SetFieldData("seikyuGengo", wrkYmd.Gengo);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                const int maxRow = 16;
                int ptIndex = (CurrentPage - 1) * maxRow;

                var curReceInfs = receInfs
                    .Where(r => r.CityCode == currentCityCode)
                    .OrderBy(r => r.CityCode)
                    .ThenBy(r => r.KohiSbt)
                    .ThenBy(r => r.WelfareJyukyusyaNo)
                    .ThenBy(r => r.PtId)
                    .ToList();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var curReceInf = curReceInfs[ptIndex];

                    //市町コード
                    CoRep.ListText("cityCode", 0, rowNo, curReceInf.CityCode);
                    //助成種別
                    CoRep.ListText("kohiSbt", 0, rowNo, curReceInf.KohiSbt);
                    //受給者証資格番号
                    CoRep.ListText("jyukyusyaNo", 0, rowNo, curReceInf.WelfareJyukyusyaNo);
                    //氏名
                    CoRep.ListText("ptName", 0, rowNo, curReceInf.PtName);
                    //性別
                    CoRep.ListText("ptSex", 0, rowNo, curReceInf.Sex);
                    //生年月日
                    CoRep.ListText("birthday", 0, rowNo, CIUtil.SDateToWDate(curReceInf.BirthDay));
                    //診療年月
                    CoRep.ListText("sinYm", 0, rowNo, CIUtil.SDateToWDate(curReceInf.SinYm * 100 + 1) / 100);
                    //一部負担割合
                    CoRep.ListText("hokenRate", 0, rowNo, curReceInf.HokenRate / 10);
                    //保険請求点数
                    CoRep.ListText("tensu", 0, rowNo, curReceInf.Tensu);
                    //一部負担額
                    CoRep.ListText("futan", 0, rowNo, curReceInf.HokenReceFutan);

                    //公費・長 区分、公費請求点数、公費・長 一部負担額
                    if (curReceInf.IsChoki)
                    {
                        CoRep.ListText("kohiKbn", 0, rowNo, 99);
                        CoRep.ListText("kohiTensu", 0, rowNo, curReceInf.KohiReceTensu(1));
                        CoRep.ListText("kohiFutan", 0, rowNo, curReceInf.Kohi1Limit);
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
                            CoRep.ListText("kohiKbn", 0, rowNo, kohiHoubetu);
                            CoRep.ListText("kohiTensu", 0, rowNo, kohiTensu);
                            CoRep.ListText("kohiFutan", 0, rowNo, kohiFutan);
                        }
                    }

                    //処方せん発行区分
                    CoRep.ListText("outDrug", 0, rowNo, curReceInf.IsOutDrug ? "1" : "");


                    ptIndex++;
                    if (ptIndex >= curReceInfs.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
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
            hpInf = _welfareFinder.GetHpInf(hpId, seikyuYm);
            var wrkReces = _welfareFinder.GetReceInf(hpId, seikyuYm, seikyuType, kohiHokenNos, FutanCheck.None, 0);

            //三重県用のモデルクラスにコピー
            receInfs = wrkReces
                .Select(x => new CoP24WelfareReceInfModel(x.ReceInf, x.PtInf, x.PtKohi1, x.PtKohi2, x.PtKohi3, x.PtKohi4, kohiHokenNos))
                .OrderBy(r => r.CityCode)
                .ThenBy(r => r.KohiSbt)
                .ThenBy(r => r.WelfareJyukyusyaNo)
                .ThenBy(r => r.PtId)
                .ToList();
            //処方せん発行区分
            foreach (var receInf in receInfs)
            {
                receInf.IsOutDrug = _welfareFinder.IsOutDrugOrder(hpId, receInf.PtId, receInf.SinYm);
            }
            //市町コードリストを取得
            cityCodes = receInfs.GroupBy(r => r.CityCode).OrderBy(r => r.Key).Select(r => r.Key).ToList();

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
