﻿using Domain.Common;
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

        (PatientInforModel ptInfModel, bool isFound) SearchExactlyPtNum(long ptNum, int hpId);

        List<PatientInforModel> SearchContainPtNum(int ptNum, string keyword, int hpId, int pageIndex, int pageSize);

        List<PatientInforModel> SearchBySindate(int sindate, int hpId, int pageIndex, int pageSize);

        List<PatientInforModel> SearchPhone(string keyword, bool isContainMode, int hpId, int pageIndex, int pageSize);

        List<PatientInforModel> SearchName(string originKeyword, string halfsizeKeyword, bool isContainMode, int hpId, int pageIndex, int pageSize);

        List<PatientInforModel> SearchSimple(string keyword, bool isContainMode, int hpId);

        List<PatientInforModel> GetAdvancedSearchResults(PatientAdvancedSearchInput input, int hpId, int pageIndex, int pageSize);

        PatientInforModel PatientCommentModels(int hpId, long ptId);

        List<PatientInforModel> SearchEmptyId(int hpId, long ptNum, int pageIndex, int pageSize);

        bool CheckExistIdList(List<long> ptIds);

        List<TokkiMstModel> GetListTokki(int hpId, int sinDate);

        List<DefHokenNoModel> GetDefHokenNoModels(int hpId, string futansyaNo);

        List<PtKyuseiInfModel> PtKyuseiInfModels(int hpId, long ptId, bool isDeleted);

        bool SaveInsuranceMasterLinkage(List<DefHokenNoModel> defHokenNoModels, int hpId, int userId);

        (bool, long) CreatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps, List<LimitListModel> maxMoneys, Func<int, long , long, IEnumerable<InsuranceScanModel>> handlerInsuranceScans, int userId);

        (bool, long) UpdatePatientInfo(PatientInforSaveModel ptInf, List<PtKyuseiModel> ptKyuseis, List<CalculationInfModel> ptSanteis, List<InsuranceModel> insurances, List<HokenInfModel> hokenInfs, List<KohiInfModel> hokenKohis, List<GroupInfModel> ptGrps, List<LimitListModel> maxMoneys, Func<int, long, long, IEnumerable<InsuranceScanModel>> handlerInsuranceScans, int userId);

        bool DeletePatientInfo(long ptId, int hpId, int userId);
        bool IsAllowDeletePatient(int hpId, long ptId);

        HokenMstModel GetHokenMstByInfor(int hokenNo, int hokenEdaNo, int sinDate);

        HokensyaMstModel GetHokenSyaMstByInfor(int hpId, string houbetu, string hokensya);

        PatientInforModel GetPtInf(int hpId, long ptId);

        List<PatientInforModel> SearchPatient(int hpId, long ptId, int pageIndex, int pageSize);

        List<PatientInforModel> SearchPatient(int hpId, int startDate, string startTime, int endDate, string endTime);

        public bool IsRyosyoFuyou(int hpId, long ptId);
    }
}