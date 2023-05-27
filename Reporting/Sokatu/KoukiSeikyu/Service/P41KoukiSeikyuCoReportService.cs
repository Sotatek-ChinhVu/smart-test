using Helper.Constants;
using Infrastructure.Services;
using Reporting.Mappers.Common;
using Reporting.Sokatu.Common.Models;
using Reporting.Sokatu.KoukiSeikyu.DB;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public class P41KoukiSeikyuCoReportService : IP41KoukiSeikyuCoReportService
{
    #region Constant
    private const int MyPrefNo = 41;
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoKoukiSeikyuFinder _kokhoFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private string currentHokensyaNo;
    private List<string> hokensyaNos;
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<CoReceInfModel> receInfs;
    private CoHpInfModel hpInf;
    private List<CoKohiHoubetuMstModel> kohiHoubetuMsts;
    #endregion

    /// <summary>
    /// OutPut Data
    /// </summary>
    private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private const string _formFileName = "p28KoukiSeikyu.rse";

    #region Constructor and Init
    public P41KoukiSeikyuCoReportService(ICoKoukiSeikyuFinder kokhoFinder)
    {
        _kokhoFinder = kokhoFinder;
    }
    #endregion

    public CommonReportingRequestModel GetP41KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType)
    {
        throw new NotImplementedException();
    }
}
