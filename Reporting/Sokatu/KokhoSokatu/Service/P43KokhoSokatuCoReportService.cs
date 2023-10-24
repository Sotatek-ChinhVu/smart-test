using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.Common.Utils;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Mapper;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P43KokhoSokatuCoReportService : IP43KokhoSokatuCoReportService
{
    #region Constant
    private const int myPrefNo = 43;
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoKokhoSokatuFinder _kokhoFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<string> hokensyaNos;
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    private List<CoKaMstModel> kaMsts;

    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private const string _formFileName = "p43KokhoSokatu.rse";
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;

    #region Constructor and Init
    public P43KokhoSokatuCoReportService(ICoKokhoSokatuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
        _singleFieldData = new();
        _setFieldData = new();
        _extralData = new();
        _listTextData = new();
        _visibleFieldData = new();
    }
    #endregion

    #region Init properties
    private int hpId;
    private int seikyuYm;
    private SeikyuType seikyuType;
    private bool hasNextPage;
    private int currentPage;
    #endregion

    public CommonReportingRequestModel GetP43KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        this.hpId = hpId;
        this.seikyuYm = seikyuYm;
        this.seikyuType = seikyuType;
        this.currentPage = 1;
        var getData = GetData();
        hasNextPage = true;

        if (getData)
        {
            while (getData && hasNextPage)
            {
                UpdateDrawForm();
                currentPage++;
            } 
        }

        var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
        _extralData.Add("totalPage", pageIndex.ToString());
        return new KokhoSokatuMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData).GetData();
    }

    #region Private function

    private bool UpdateDrawForm()
    {
        bool _hasNextPage = true;
        bool flgNextPage = true;

        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            //医療機関コード
            SetFieldData("hpCode", hpInf.HpCd);
            //医療機関情報
            SetFieldData("postCd1", CIUtil.Copy(hpInf.PostCd, 1, 3));
            SetFieldData("postCd2", hpInf.PostCd.Length == 7 ? CIUtil.Copy(hpInf.PostCd, 4, 4) : CIUtil.Copy(hpInf.PostCd, 5, 4));
            SetFieldData("address1", hpInf.Address1);
            SetFieldData("address2", hpInf.Address2);
            SetFieldData("hpName", hpInf.ReceHpName);
            SetFieldData("kaisetuName", hpInf.KaisetuName);
            SetFieldData("hpTel", hpInf.Tel);
            //請求年月
            CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
            for (int i = 0; i <= 1; i++)
            {
                SetFieldData(string.Format("seikyuGengo{0}", i), wrkYmd.Gengo);
                SetFieldData(string.Format("seikyuYear{0}", i), wrkYmd.Year.ToString());
                SetFieldData(string.Format("seikyuMonth{0}", i), wrkYmd.Month.ToString());
            }
            //診療科
            SetFieldData("kaName", kaMsts[0].KaName);

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            List<ListTextObject> listDataPerPage = new();
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;

            #region 合計
            const int maxRow = 10;

            if (currentPage == 1)
            {
                //1枚目のみ記載する
                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    switch (rowNo)
                    {
                        case 0: wrkReces = receInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                        case 1: wrkReces = receInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                        case 2: wrkReces = receInfs.Where(r => (r.IsNrMine || r.IsNrFamily)).ToList(); break;
                        case 3: wrkReces = receInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                        case 4: wrkReces = receInfs.Where(r => r.IsRetMine).ToList(); break;
                        case 5: wrkReces = receInfs.Where(r => r.IsRetFamily).ToList(); break;
                        case 6: wrkReces = receInfs.Where(r => r.IsRetPreSchool).ToList(); break;
                        case 7: wrkReces = receInfs.Where(r => r.IsNrAll).ToList(); break;
                        case 8: wrkReces = receInfs.Where(r => r.IsRetAll).ToList(); break;
                        case 9: wrkReces = receInfs.Where(r => r.IsNrAll || r.IsRetAll).ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    countData wrkData = new countData();
                    //件数
                    pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                    Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();

                    wrkData.Count = wrkReces.Count;
                    fieldDataPerPage.Add(string.Format("totalCount{0}", rowNo), wrkData.Count.ToString());

                    if (rowNo < 7)
                    {
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                        fieldDataPerPage.Add(string.Format("totalTensu{0}", rowNo), wrkData.Tensu.ToString());
                    }

                    if (!_setFieldData.ContainsKey(pageIndex))
                    {
                        _setFieldData.Add(pageIndex, fieldDataPerPage);
                    }
                }
            }
            #endregion

            #region 保険者単位の集計
            const int maxHokensyaRow = 8;
            const int maxHokensyaCol = 7;
            int hokensyaIndex = (currentPage - 1) * maxHokensyaRow;

            for (short rowNo = 0; rowNo < maxHokensyaRow; rowNo++)
            {
                if (hokensyaIndex < hokensyaNos.Count)
                {
                    var currentNo = hokensyaNos[hokensyaIndex];
                    var curReceInfs = receInfs.Where(r => r.HokensyaNo == currentNo);
                    //保険者名
                    var hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == currentNo)?.Name ?? "";
                    listDataPerPage.Add(new("hokensyaName", 0, rowNo, hokensyaName == "" ? currentNo : hokensyaName));

                    for (short colNo = 0; colNo < maxHokensyaCol; colNo++)
                    {
                        List<CoReceInfModel> wrkReces = null;
                        switch (colNo)
                        {
                            //国保
                            case 0: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                            case 1: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                            case 2: wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily)).ToList(); break;
                            case 3: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                            //退職
                            case 4: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                            case 5: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                            case 6: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
                        }
                        if (wrkReces == null) continue;

                        countData wrkData = new countData();
                        //件数
                        wrkData.Count = wrkReces.Count;
                        listDataPerPage.Add(new(string.Format("count{0}", colNo), 0, rowNo, wrkData.Count.ToString()));
                        //点数
                        wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                        listDataPerPage.Add(new(string.Format("tensu{0}", colNo), 0, rowNo, wrkData.Tensu.ToString()));
                    }
                }

                hokensyaIndex++;
                if (hokensyaIndex >= hokensyaNos.Count)
                {
                    //_listTextData.Add(pageIndex, listDataPerPage);
                    flgNextPage = false;
                    break;
                }
            }
            #endregion

            #region 公費単位の集計
            const int maxKohiRow = 3;
            const int maxKohiCol = 2;
            int kohiIndex = (currentPage - 1) * maxKohiRow * maxKohiCol;

            var curHeiyoReceInfs = receInfs;
            var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curHeiyoReceInfs.ToList(), null);

            if (kohiHoubetus.Count != 0)
            {
                //集計
                for (short colNo = 0; colNo < maxKohiCol; colNo++)
                {
                    if (kohiIndex < kohiHoubetus.Count)
                    {
                        for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
                        {
                            var wrkReces = curHeiyoReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiIndex])).ToList();
                            //法別番号
                            listDataPerPage.Add(new(string.Format("kohiNo{0}", colNo), 0, rowNo, kohiHoubetus[kohiIndex]));

                            countData wrkData = new countData();

                            //件数
                            wrkData.Count = wrkReces.Count;
                            listDataPerPage.Add(new(string.Format("kohiCount{0}", colNo), 0, rowNo, wrkData.Count.ToString()));
                            //点数
                            wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiIndex]));
                            listDataPerPage.Add(new(string.Format("kohiTensu{0}", colNo), 0, rowNo, wrkData.Tensu.ToString()));

                            kohiIndex++;
                            if (kohiIndex >= kohiHoubetus.Count)
                            {
                                break;
                            }
                        }

                    }

                    if (kohiIndex >= kohiHoubetus.Count)
                    {
                        if (flgNextPage == false)
                        {
                            _hasNextPage = false;
                        }
                        break;
                    }
                }
            }
            else
            {
                if (flgNextPage == false)
                {
                    _hasNextPage = false;
                }
            }
            #endregion
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
        hpInf = _kokhoFinder.GetHpInf(hpId, seikyuYm);
        kaMsts = _kokhoFinder.GetKaMst(hpId);
        receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kokho, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
        //保険者番号リストを取得（県内→県外）
        hokensyaNos = receInfs.Where(r => r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
        hokensyaNos.AddRange(
            receInfs.Where(r => !r.IsPrefIn).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList()
        );
        //保険者名を取得
        hokensyaNames = _kokhoFinder.GetHokensyaName(hpId, hokensyaNos);

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
