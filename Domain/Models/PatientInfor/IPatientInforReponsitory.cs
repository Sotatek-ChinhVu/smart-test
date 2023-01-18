using Domain.Common;
using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.MaxMoney;
using Helper.Constants;
using HokenInfModel = Domain.Models.Insurance.HokenInfModel;

namespace Domain.Models.PatientInfor
{
    public interface IPatientInforRepository : IRepositoryBase
    {
        PatientInforModel? GetById(int hpId, long ptId, int sinDate, int raiinNo);

        (PatientInforModel, bool) SearchExactlyPtNum(int ptNum, int hpId);

        List<PatientInforModel> SearchContainPtNum(int ptNum, string keyword, int hpId, int pageIndex, int pageSize);

        List<PatientInforModel> SearchBySindate(int sindate, int hpId, int pageIndex, int pageSize);

        List<PatientInforModel> SearchPhone(string keyword, bool isContainMode, int hpId, int pageIndex, int pageSize);

        List<PatientInforModel> SearchName(string keyword, bool isContainMode, int hpId, int pageIndex, int pageSize);

        List<PatientInforModel> SearchSimple(string keyword, bool isContainMode, int hpId);

        List<PatientInforModel> GetAdvancedSearchResults(PatientAdvancedSearchInput input, int hpId, int pageIndex, int pageSize);

        PatientInforModel PatientCommentModels(int hpId, long ptId);

        List<PatientInforModel> SearchEmptyId(int hpId, long ptNum, int pageIndex, int pageSize);

        bool CheckExistListId(List<long> ptIds);

        List<TokkiMstModel> GetListTokki(int hpId, int sinDate);

        List<DefHokenNoModel> GetDefHokenNoModels(int hpId, string futansyaNo);

        List<PtKyuseiInfModel> PtKyuseiInfModels(int hpId, long ptId, bool isDeleted);

        bool SaveInsuranceMasterLinkage(List<DefHokenNoModel> defHokenNoModels, int hpId, int userId);
        (bool, long) CreatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps, List<LimitListModel> maxMoneys, int userId);

        (bool, long) UpdatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps, List<LimitListModel> maxMoneys, int userId);

        bool DeletePatientInfo(long ptId, int hpId, int userId);
        bool IsAllowDeletePatient(int hpId, long ptId);

        HokenMstModel GetHokenMstByInfor(int hokenNo, int hokenEdaNo);

        HokensyaMstModel GetHokenSyaMstByInfor(int hpId, string houbetu, string hokensya);
    }
}