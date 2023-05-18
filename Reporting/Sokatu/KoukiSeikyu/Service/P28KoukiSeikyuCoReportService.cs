using Helper.Common;
using Helper.Extension;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KoukiSeikyu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service
{
    public class P28KoukiSeikyuCoReportService : IP28KoukiSeikyuCoReportService
    {
        #region Constructor and Init
        private const int MyPrefNo = 28;
        private int _hpId;
        private int _seikyuYm;
        private SeikyuType _seikyuType;
        private bool _hasNextPage;
        private int _currentPage;
        private string _currentHokensyaNo;
        private string _formYm = string.Empty;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<string> hokensyaNos;
        private List<CoHokensyaMstModel> hokensyaNames;
        private List<CoReceInfModel> receInfs;
        private CoHpInfModel hpInf;
        private List<CoKaMstModel> kaMsts;

        /// <summary>
        /// OutPut Data
        /// </summary>
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();
        private readonly Dictionary<string, bool> _visibleFieldData = new Dictionary<string, bool>();
        private readonly Dictionary<string, string> _fileNamePageMap = new Dictionary<string, string>();
        private readonly string _rowCountFieldName = string.Empty;
        private readonly int _reportType = (int)CoReportType.KoukiSeikyu;

        /// <summary>
        /// Finder
        /// </summary>

        private readonly ICoKokhoSeikyuFinder _kokhoFinder;

        public P28KoukiSeikyuCoReportService(ICoKokhoSeikyuFinder kokhoFinder)
        {
            _kokhoFinder = kokhoFinder;
        }

        #endregion
        public CommonReportingRequestModel GetP28KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            _hpId = hpId;
            _seikyuYm = seikyuYm;
            _seikyuType = seikyuType;
            var getData = GetData();
            _hasNextPage = true;
            _currentPage = 1;

            while (getData && _hasNextPage)
            {
                UpdateDrawForm();
                _currentPage++;
            }

            _extralData.Add("maxRow", "2");
            _fileNamePageMap.Add("1", string.Concat("p28KoukiSeikyu.rse"));
            return new KoukiSeikyuMapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName, _reportType, _visibleFieldData, _fileNamePageMap).GetData();
        }

        private bool UpdateDrawForm()
        {
            _hasNextPage = true;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //医療機関コード
                SetFieldData("hpCode", hpInf.ReceHpCd);
                //医療機関情報
                SetFieldData("postCd", hpInf.PostCdDsp);
                SetFieldData("address1", hpInf.Address1);
                SetFieldData("address2", hpInf.Address2);
                SetFieldData("hpName", hpInf.ReceHpName);
                SetFieldData("kaisetuName", hpInf.KaisetuName);
                SetFieldData("hpTel", hpInf.Tel);
                //診療科
                const int maxKaRow = 4;
                for (int i = 0; i <= kaMsts.Count - 1 && i <= maxKaRow; i++)
                {
                    Dictionary<string, CellModel> data = new();
                    if (i == 0)
                    {
                        SetFieldData("kaName", kaMsts[i].KaName);
                    }
                    else
                    {
                        AddListData(ref data, "kaNames", kaMsts[i].KaName);
                    }

                    _tableFieldData.Add(data);
                }
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(_seikyuYm * 100 + 1);
                SetFieldData("seikyuGengo", wrkYmd.Gengo);
                SetFieldData("seikyuYear", wrkYmd.Year.AsString());
                SetFieldData("seikyuMonth", wrkYmd.Month.AsString());
                //提出年月日
                SetFieldData("reportGengo", wrkYmd.Gengo);
                SetFieldData("reportYear", wrkYmd.Year.AsString());
                SetFieldData("reportMonth", wrkYmd.Month.AsString());
                SetFieldData("reportDay", wrkYmd.Day.AsString());
                //保険者
                SetFieldData("hokensyaNo", _currentHokensyaNo);

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                #region Body
                const int maxRow = 2;

                if (_currentPage == 1)
                {
                    //1枚目のみ記載する
                    for (short rowNo = 0; rowNo < maxRow; rowNo++)
                    {
                        List<CoReceInfModel> wrkReces = null;
                        Dictionary<string, CellModel> data = new();
                        switch (rowNo)
                        {
                            case 0: wrkReces = receInfs.Where(r => r.IsKoukiIppan).ToList(); break;
                            case 1: wrkReces = receInfs.Where(r => r.IsKoukiUpper).ToList(); break;
                        }
                        if (wrkReces == null) continue;

                        countData wrkData = new countData();
                        //件数
                        wrkData.Count = wrkReces.Count;
                        AddListData(ref data, "count", wrkData.Count.AsString());
                        //日数
                        wrkData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                        AddListData(ref data, "nissu", wrkData.Nissu.AsString());
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                        AddListData(ref data, "tensu", wrkData.Tensu.AsString());
                        //一部負担金
                        wrkData.Futan = wrkReces.Sum(r => r.HokenReceFutan);
                        AddListData(ref data, "futan", wrkData.Futan.AsString());
                    }
                }
                #endregion

                #region 公費負担医療
                const int maxKohiRow = 12;
                int kohiIndex = (_currentPage - 1) * maxKohiRow;

                var kohiHoubetus = SokatuUtil.GetKohiHoubetu(receInfs.Where(r => r.IsHeiyo).ToList(), null);
                if (kohiHoubetus.Count == 0)
                {
                    _hasNextPage = false;
                    return 1;
                }

                //集計
                for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
                {
                    Dictionary<string, CellModel> data = new();
                    var wrkReces = receInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiIndex])).ToList();

                    short curRowNo = rowNo;

                    string fieldBiko = "";
                    if (rowNo >= 2)
                    {
                        //3つ以上ある場合は備考欄に記載する
                        fieldBiko = "Biko";
                        for (int i = 1; i <= 5; i++)
                        {
                            SetVisibleFieldData("kohiTitleBiko", true);
                        }
                        curRowNo -= 2;
                    }

                    //法別番号
                    AddListData(ref data, "kohiHoubetu" + fieldBiko, kohiHoubetus[kohiIndex]);

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    AddListData(ref data, "kohiCount" + fieldBiko, wrkData.Count.AsString());
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[kohiIndex]));
                    AddListData(ref data, "kohiNissu" + fieldBiko, wrkData.Nissu.AsString());
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiIndex]));
                    AddListData(ref data, "kohiTensu" + fieldBiko, wrkData.Tensu.AsString());
                    //一部負担金
                    wrkData.Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[kohiIndex]));
                    AddListData(ref data, "kohiFutan" + fieldBiko, wrkData.Futan.AsString());

                    kohiIndex++;
                    if (kohiIndex >= kohiHoubetus.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }
                #endregion

                return 1;
            }
            #endregion

            #endregion

            try
            {
                if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }

        private void SetVisibleFieldData(string field, bool value)
        {
            if (!string.IsNullOrEmpty(field) && !_visibleFieldData.ContainsKey(field))
            {
                _visibleFieldData.Add(field, value);
            }
        }

        private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
            {
                dictionary.Add(field, new CellModel(value));
            }
        }

        private bool GetData()
        {
            hpInf = _kokhoFinder.GetHpInf(_hpId, _seikyuYm);
            kaMsts = _kokhoFinder.GetKaMst(_hpId);
            receInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.All, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.SumAll);
            //保険者番号リストを取得
            hokensyaNos = receInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
            //保険者名を取得
            hokensyaNames = _kokhoFinder.GetHokensyaName(_hpId, hokensyaNos);

            return (receInfs?.Count ?? 0) > 0;
        }

    }
}
