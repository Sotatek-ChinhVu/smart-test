using Domain.Common;
using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.MaxMoney;
using HokenInfModel = Domain.Models.Insurance.HokenInfModel;

namespace Domain.Models.PatientInfor
{
    public interface IPatientInforRepository : IRepositoryBase
    {
        PatientInforModel? GetById(int hpId, long ptId, int sinDate, long raiinNo, bool isShowKyuSeiName = false);

        (PatientInforModel ptInfModel, bool isFound) SearchExactlyPtNum(long ptNum, int hpId, int sinDate);

        List<PatientInforModel> SearchContainPtNum(int ptNum, string keyword, int hpId, int pageIndex, int pageSize, Dictionary<string, string> sortData);

        List<PatientInforModel> SearchBySindate(int sindate, int hpId, int pageIndex, int pageSize, Dictionary<string, string> sortData);

        List<PatientInforModel> SearchPhone(string keyword, bool isContainMode, int hpId, int pageIndex, int pageSize, Dictionary<string, string> sortData);

        List<PatientInforModel> SearchName(string originKeyword, string halfsizeKeyword, bool isContainMode, int hpId, int pageIndex, int pageSize, Dictionary<string, string> sortData);

        List<PatientInforModel> SearchSimple(string keyword, bool isContainMode, int hpId);

        List<PatientInforModel> GetAdvancedSearchResults(PatientAdvancedSearchInput input, int hpId, int pageIndex, int pageSize, Dictionary<string, string> sortData);

        PatientInforModel PatientCommentModels(int hpId, long ptId);

        PatientInforModel GetPtInfByRefNo(int hpId, long refNo);

        List<PatientInforModel> SearchEmptyId(int hpId, long ptNum, int pageIndex, int pageSize, bool isPtNumCheckDigit, int autoSetting);

        bool CheckExistIdList(List<long> ptIds);

        List<TokkiMstModel> GetListTokki(int hpId, int sinDate);

        List<DefHokenNoModel> GetDefHokenNoModels(int hpId, string futansyaNo);

        List<PtKyuseiInfModel> PtKyuseiInfModels(int hpId, long ptId, bool isDeleted);

        PtKyuseiInfModel GetDocumentKyuSeiInf(int hpId, long ptId, int sinDay);

        bool SaveInsuranceMasterLinkage(List<DefHokenNoModel> defHokenNoModels, int hpId, int userId);

        (bool resultSave, long ptId) CreatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps, List<LimitListModel> maxMoneys, Func<int, long, long, IEnumerable<InsuranceScanModel>> handlerInsuranceScans, int userId);

        (bool resultSave, long ptId) UpdatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps, List<LimitListModel> maxMoneys, Func<int, long, long, IEnumerable<InsuranceScanModel>> handlerInsuranceScans, int userId, List<int> hokenIdList);

        bool DeletePatientInfo(long ptId, int hpId, int userId);
        bool IsAllowDeletePatient(int hpId, long ptId);

        HokenMstModel GetHokenMstByInfor(int hokenNo, int hokenEdaNo, int sinDate);

        HokensyaMstModel GetHokenSyaMstByInfor(int hpId, string houbetu, string hokensya);

        PatientInforModel GetPtInf(int hpId, long ptId);

        List<PatientInforModel> SearchPatient(int hpId, long ptId, int pageIndex, int pageSize);

        List<PatientInforModel> SearchPatient(int hpId, int startDate, string startTime, int endDate, string endTime);

        List<PatientInforModel> SearchPatient(int hpId, List<long> ptIdList);

        public bool IsRyosyoFuyou(int hpId, long ptId);

        long GetPtIdFromPtNum(int hpId, long ptNum);

        int GetCountRaiinAlreadyPaidOfPatientByDate(int fromDate, int toDate, long ptId, int raiintStatus);

        List<PatientInforModel> FindSamePatient(int hpId, string kanjiName, int sex, int birthDay);

        List<PatientInforModel> GetPtInfModelsByName(int hpId, string kanaName, string name, int birthDate, int sex1, int sex2);

        List<PatientInforModel> GetPtInfModels(int hpId, long refNo);

        bool SavePtKyusei(int hpId, int userId, List<PtKyuseiModel> ptKyuseiList);

        List<VisitTimesManagementModel> GetVisitTimesManagementModels(int hpId, int sinYm, long ptId, int kohiId);

        bool UpdateVisitTimesManagement(int hpId, int userId, long ptId, int kohiId, int sinYm, List<VisitTimesManagementModel> visitTimesManagementList);

        bool UpdateVisitTimesManagementNeedSave(int hpId, int userId, long ptId, List<VisitTimesManagementModel> visitTimesManagementList);
    }
}