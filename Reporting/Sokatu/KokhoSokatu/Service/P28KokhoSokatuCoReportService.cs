using Helper.Common;
using Helper.Extension;
using Reporting.Calculate.Constants;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public class P28KokhoSokatuCoReportService : IP28KokhoSokatuCoReportService
    {
        #region Constructor and Init

        private const int myPrefNo = 28;
        private int _hpId;
        private int _seikyuYm;
        private SeikyuType _seikyuType;
        private bool _hasNextPage;
        private int _currentPage;

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
        private const string _formFileName = "p28KokhoSokatu.rse";
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;

        /// <summary>
        /// Finder
        /// </summary>
        private readonly ICoKokhoSokatuFinder _kokhoFinder;


        public P28KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
        {
            _kokhoFinder = kokhoFinder;
            _singleFieldData = new();
            _setFieldData = new();
            _listTextData = new();
            _extralData = new();
            _visibleFieldData = new();
        }
        #endregion

        public CommonReportingRequestModel GetP28KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
        {
            _hpId = hpId;
            _seikyuYm = seikyuYm;
            _seikyuType = seikyuType;
            var getData = GetData();
            _hasNextPage = true;
            _currentPage = 1;

            if (getData)
            {
                while (getData && _hasNextPage)
                {
                    UpdateDrawForm();
                    _currentPage++;
                }
            }

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new KokhoSokatuMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData).GetData();
        }

        private bool GetData()
        {
            hpInf = _kokhoFinder.GetHpInf(_hpId, _seikyuYm);
            kaMsts = _kokhoFinder.GetKaMst(_hpId);
            receInfs = _kokhoFinder.GetReceInf(_hpId, _seikyuYm, _seikyuType, KokhoKind.All, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
            //保険者番号リストを取得
            hokensyaNos = receInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
            //保険者名を取得
            hokensyaNames = _kokhoFinder.GetHokensyaName(_hpId, hokensyaNos);

            return (receInfs?.Count ?? 0) > 0;
        }

        private void UpdateDrawForm()
        {
            #region SubMethod

            #region Header
            void UpdateFormHeader()
            {
                List<ListTextObject> listDataPerPage = new();
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
                    if (i == 0)
                    {
                        SetFieldData("kaName", kaMsts[i].KaName);
                    }
                    else
                    {
                        listDataPerPage.Add(new("kaNames", 0, (short)(i - 1), kaMsts.Any() ? kaMsts[i].KaName : string.Empty));
                    }
                }

                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(_seikyuYm * 100 + 1);
                SetFieldData("seikyuGengo", wrkYmd.Gengo);
                SetFieldData("seikyuYear", wrkYmd.Year.ToString());
                SetFieldData("seikyuMonth", wrkYmd.Month.ToString());
            }
            #endregion

            #region Body
            void UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                #region Body
                const int maxRow = 6;

                if (_currentPage == 1)
                {
                    //1枚目のみ記載する
                    for (short rowNo = 0; rowNo < maxRow; rowNo++)
                    {
                        List<CoReceInfModel> wrkReces = null;
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
                        listDataPerPage.Add(new("count", 0, rowNo, wrkData.Count.ToString()));
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                        listDataPerPage.Add(new("tensu", 0, rowNo, wrkData.Tensu.ToString()));

                        //請求書枚数
                        if (_seikyuType.IsNormal && _seikyuType.IsDelay && !_seikyuType.IsHenrei && !_seikyuType.IsOnline && !_seikyuType.IsPaper)
                        {
                            //電子請求の場合は記載なし
                        }
                        else
                        {
                            int seikyuCount = wrkReces.GroupBy(r => r.HokensyaNo).Select(r => r.Key).ToList().Count();
                            listDataPerPage.Add(new("seikyuCount", 0, rowNo, seikyuCount.ToString()));
                        }
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
                }

                for (short rowNo = 0; rowNo < maxHokRow; rowNo++)
                {
                    if (hokIndex < kokhoNos.Count)
                    {
                        string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == kokhoNos[hokIndex])?.Name ?? "";
                        listDataPerPage.Add(new("kokhoHokensyaName", 0, rowNo, hokensyaName == "" ? kokhoNos[hokIndex] : hokensyaName.ToString()));
                    }
                    if (hokIndex < koukiNos.Count)
                    {
                        string prefName = PrefCode.PrefName(koukiNos.Any() ? koukiNos[hokIndex].Substring(2, 2).AsInteger() : 0);
                        listDataPerPage.Add(new("koukiHokensyaName", 0, rowNo, prefName == "" ? koukiNos[hokIndex] : prefName.ToString()));
                    }

                    hokIndex++;
                    if (hokIndex >= kokhoNos.Count && hokIndex >= koukiNos.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                _listTextData.Add(pageIndex, listDataPerPage);
                #endregion
            }
            #endregion

            #endregion
            UpdateFormHeader();
            UpdateFormBody();
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }
    }
}
