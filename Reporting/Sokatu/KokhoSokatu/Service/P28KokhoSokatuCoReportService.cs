using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.Calculate.Constants;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Service;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public class P28KokhoSokatuCoReportService : IP28KokhoSokatuCoReportService
    {
        #region Constructor and Init

        private const int MyPrefNo = 28;
        private int _hpId;
        private int _seikyuYm;
        private SeikyuType _seikyuType;
        private bool _hasNextPage;
        private int _currentPage;
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
        private readonly Dictionary<string, string> _fileNamePageMap = new Dictionary<string, string>();
        private readonly string _rowCountFieldName = string.Empty;
        private readonly int _reportType = (int)CoReportType.KokhoSokatu;

        /// <summary>
        /// Finder
        /// </summary>
        private readonly ICoKokhoSokatuFinder _kokhoFinder;
        private readonly IReadRseReportFileService _readRseReportFileService;


        public P28KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder, IReadRseReportFileService readRseReportFileService)
        {
            _kokhoFinder = kokhoFinder;
            _readRseReportFileService = readRseReportFileService;
        }
        #endregion

        public CommonReportingRequestModel GetP28KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            _hpId = hpId;
            _seikyuYm = seikyuYm;
            _formYm = seikyuYm >= KaiseiDate.m202210 ? "_2210" : string.Empty;
            _seikyuType = seikyuType;
            var getData = GetData();

            _hasNextPage = true;
            _currentPage = 1;

            AddFileNamePageMap();

            while (getData && _hasNextPage)
            {
                UpdateDrawForm();
                _currentPage++;
            }

            _extralData.Add("maxRow", "6");
            return new KokhoSokatuMapper(_singleFieldData, _tableFieldData, _extralData, _fileNamePageMap, _rowCountFieldName, _reportType).GetData();
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
                SetFieldData("seikyuYear", wrkYmd.Year.ToString());
                SetFieldData("seikyuMonth", wrkYmd.Month.ToString());

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                #region Body
                const int maxRow = 6;

                if (_currentPage == 1)
                {
                    //1枚目のみ記載する
                    for (short rowNo = 0; rowNo < maxRow; rowNo++)
                    {
                        List<CoReceInfModel> wrkReces = null;
                        Dictionary<string, CellModel> data = new();
                        switch (rowNo)
                        {
                            //国保
                            case 0: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsPrefIn).ToList(); break;
                            case 1: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && !r.IsPrefIn).ToList(); break;
                            case 2: wrkReces = receInfs.Where(r => r.IsNrAll || r.IsRetAll).ToList(); break;
                            //後期
                            case 3: wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsPrefIn).ToList(); break;
                            case 4: wrkReces = receInfs.Where(r => r.IsKoukiAll && !r.IsPrefIn).ToList(); break;
                            case 5: wrkReces = receInfs.Where(r => r.IsKoukiAll).ToList(); break;
                        }
                        if (wrkReces == null) continue;

                        countData wrkData = new countData();
                        //件数
                        wrkData.Count = wrkReces.Count;
                        AddListData(ref data, "count", wrkData.Count.ToString());
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                        AddListData(ref data, "tensu", wrkData.Tensu.ToString());

                        //請求書枚数
                        if (_seikyuType.IsNormal && _seikyuType.IsDelay && !_seikyuType.IsHenrei && !_seikyuType.IsOnline && !_seikyuType.IsPaper)
                        {
                            //電子請求の場合は記載なし
                        }
                        else
                        {
                            int seikyuCount = wrkReces.GroupBy(r => r.HokensyaNo).Select(r => r.Key).ToList().Count();
                            AddListData(ref data, "seikyuCount", seikyuCount.ToString());
                        }

                        _tableFieldData.Add(data);
                    }

                    //社保福祉医療費請求件数
                    //CoRep.SetFieldData("welfareCount", xxxxx);
                }
                #endregion

                #region 摘要（県外保険者名）
                const int maxHokRow = 14;
                int hokIndex = (_currentPage - 1) * maxHokRow;

                var kokhoNos = receInfs.Where(r => !r.IsPrefIn && (r.IsNrAll || r.IsRetAll)).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
                var koukiNos = receInfs.Where(r => !r.IsPrefIn && r.IsKoukiAll).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

                if (kokhoNos.Count == 0 && koukiNos.Count == 0)
                {
                    _hasNextPage = false;
                    return 1;
                }

                for (short rowNo = 0; rowNo < maxHokRow; rowNo++)
                {
                    Dictionary<string, CellModel> data = new();

                    if (hokIndex < kokhoNos.Count)
                    {
                        string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == kokhoNos[hokIndex])?.Name ?? "";
                        AddListData(ref data, "kokhoHokensyaName", hokensyaName == "" ? kokhoNos[hokIndex] : hokensyaName);
                    }
                    if (hokIndex < koukiNos.Count)
                    {
                        string prefName = PrefCode.PrefName(koukiNos[hokIndex].Substring(2, 2).AsInteger());
                        AddListData(ref data, "koukiHokensyaName", prefName == "" ? koukiNos[hokIndex] : prefName);
                    }

                    _tableFieldData.Add(data);

                    hokIndex++;
                    if (hokIndex >= kokhoNos.Count && hokIndex >= koukiNos.Count)
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

        private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
            {
                dictionary.Add(field, new CellModel(value));
            }
        }

        private void AddFileNamePageMap()
        {
            _fileNamePageMap.Add("1", string.Concat("p28KokhoSokatu", _formYm + ".rse"));
        }

    }
}
