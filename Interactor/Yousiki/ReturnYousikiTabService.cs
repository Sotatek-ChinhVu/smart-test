using Domain.Models.Diseases;
using Domain.Models.Yousiki;
using Domain.Models.Yousiki.CommonModel;
using Domain.Models.Yousiki.CommonModel.CommonOutputModel;
using Helper.Enum;
using Helper.Extension;
using Infrastructure.Interfaces;

namespace Interactor.Yousiki;

public class ReturnYousikiTabService : IReturnYousikiTabService
{
    private const string _outpatientConsultationInfCodeNo = "LR00001";
    private readonly List<string> _listStrokeCodeNo = new() { "LMHCA01", "LMHCA02" };
    private readonly List<string> _listAcuteCoronaryCodeNo = new() { "LMHACS1", "LMHACS2" };
    private readonly List<string> _listAcuteAorticCodeNo = new() { "LMHAAD1", "LMHAAD2" };
    private const string CodeNo_DiagnosticInfInjuries = "CD00001";
    private const string CodeNo_StatusOfVisit = "HCVMT01";
    private const string CodeNo_StatusOfNursingVisit = "HCVNS01";
    private const string CodeNo_StatusOfEmergencyConsultation = "HCEC001";
    private const string CodeNo_StatusOfShortTermAdmission = "HCSA001";
    private const string CodeNo_PatientSituation = "HPS0001";
    private const string CodeNo_BartherIndex = "HPS0002";
    private const string CodeNo_PresenceNurtrition = "HPS0006";
    private const string CodeNo_HospitalizationStatus = "HCH0001";
    private const string CodeNo_CommonHospitalizationStatus = "CH00001";
    private const string CodeNo_EndOfMedicineInf = "CDF0001";
    private const string CodeNo_StatusHomeVisit = "HCHC001";
    private const string CodeNo_OutpatientConsultate = "RR00001";
    private const string CodeNo_ByomeiRehabilitation = "RCD0001";
    private const string CodeNo_PatientStatus = "RPADL01";
    private const string CodeNo_MainInjuryOfVisit = "HCVD001";
    private const string CodeNo_ApprearanceDiagnosis = "HPCD001";

    private readonly IPtDiseaseRepository _ptDiseaseRepository;

    public ReturnYousikiTabService(IPtDiseaseRepository ptDiseaseRepository)
    {
        _ptDiseaseRepository = ptDiseaseRepository;
    }

    /// <summary>
    /// RenderTabYousiki
    /// </summary>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    public TabYousikiModel RenderTabYousiki(Yousiki1InfModel yousiki1Inf, Dictionary<string, string> kacodeYousikiMstDict)
    {
        var commonYousiki1InfDetailList = yousiki1Inf.Yousiki1InfDetailList.Where(item => item.DataType == 0).ToList();
        var livingHabitYousiki1InfDetailList = yousiki1Inf.Yousiki1InfDetailList.Where(item => item.DataType == 1).ToList();
        var atHomeYousiki1InfDetailList = yousiki1Inf.Yousiki1InfDetailList.Where(item => item.DataType == 2).ToList();
        var rehabilitationYousiki1InfDetailList = yousiki1Inf.Yousiki1InfDetailList.Where(item => item.DataType == 3).ToList();

        var commonData = RenderCommon(yousiki1Inf, commonYousiki1InfDetailList);
        var livingHabitData = RenderLivingHabit(livingHabitYousiki1InfDetailList);
        var atHomeData = RenderAtHomeYousiki(yousiki1Inf, atHomeYousiki1InfDetailList);
        var rehabilitationData = RenderRehabilitation(yousiki1Inf, rehabilitationYousiki1InfDetailList, kacodeYousikiMstDict);
        return new TabYousikiModel(commonData, atHomeData, livingHabitData, rehabilitationData);
    }

    #region RenderCommon
    /// <summary>
    /// RenderCommon
    /// </summary>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private CommonModel RenderCommon(Yousiki1InfModel yousiki1Inf, List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var diagnosticInjuryList = GetDiagnosticInfInjuries(yousiki1Inf, ref yousiki1InfDetailList);
        var hospitalizationStatusInf = GetCommonHospitalizationStatus(yousiki1Inf, ref yousiki1InfDetailList);
        var finalExaminationInf = GetEndOfMedicineInf(yousiki1Inf, ref yousiki1InfDetailList);

        #region set byomei data
        List<string> byomeiCdList = new();
        byomeiCdList.AddRange(hospitalizationStatusInf.ByomeiInf.GetByomeiCdList());
        byomeiCdList.AddRange(finalExaminationInf.ByomeiInf.GetByomeiCdList());
        foreach (var item in diagnosticInjuryList)
        {
            byomeiCdList.AddRange(item.GetByomeiCdList());
        }
        byomeiCdList = byomeiCdList.Distinct().ToList();
        if (byomeiCdList.Any())
        {
            var byomeiDictionary = _ptDiseaseRepository.GetByomeiMst(yousiki1Inf.HpId, byomeiCdList);
            hospitalizationStatusInf.ByomeiInf.GetCommonImageInf(byomeiDictionary);
            finalExaminationInf.ByomeiInf.GetCommonImageInf(byomeiDictionary);
            foreach (var item in diagnosticInjuryList)
            {
                item.GetCommonImageInf(byomeiDictionary);
            }
        }
        #endregion

        var commonModel = new CommonModel(yousiki1InfDetailList, diagnosticInjuryList, hospitalizationStatusInf, finalExaminationInf);
        return commonModel;
    }

    /// <summary>
    /// GetDiagnosticInfInjuries
    /// </summary>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private List<CommonForm1Model> GetDiagnosticInfInjuries(Yousiki1InfModel yousiki1Inf, ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var yousiki1DiagnosticInjuryList = yousiki1InfDetailList.Where(x => x.CodeNo == CodeNo_DiagnosticInfInjuries).ToList();
        RemoveRange(ref yousiki1InfDetailList, yousiki1DiagnosticInjuryList);

        return SetData(CodeNo_DiagnosticInfInjuries, yousiki1Inf, yousiki1DiagnosticInjuryList, 10, true);
    }

    /// <summary>
    /// GetCommonHospitalizationStatus
    /// </summary>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private InputByomeiCommonModel GetCommonHospitalizationStatus(Yousiki1InfModel yousiki1Inf, ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        string codeNo = CodeNo_CommonHospitalizationStatus;
        var commonForm1Model = new CommonForm1Model(codeNo, yousiki1Inf)
        {
            PayLoadValueSelect = 1,
            PayLoadInjuryName = 9,
            PayLoadICD10Code = 2,
            PayLoadInjuryNameCode = 3,
            PayLoadModifierCode = 4,
        };

        var hospitalizationStatusYousikiList = yousiki1InfDetailList.Where(x => x.CodeNo == codeNo && x.RowNo == 0).ToList();
        if (hospitalizationStatusYousikiList.Any())
        {
            RemoveRange(ref yousiki1InfDetailList, hospitalizationStatusYousikiList);

            commonForm1Model.SetData(hospitalizationStatusYousikiList);
        }

        var result = new InputByomeiCommonModel(commonForm1Model);
        return result;
    }

    private InputByomeiCommonModel GetEndOfMedicineInf(Yousiki1InfModel yousiki1Inf, ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        string codeNo = CodeNo_EndOfMedicineInf;
        var duringMonthMedicineInfModel = GetYousiki1InfDetailModel(ref yousiki1InfDetailList, codeNo, 0, 1);
        var finalMedicineDateModel = GetYousiki1InfDetailModel(ref yousiki1InfDetailList, codeNo, 0, 2);

        var commonForm1Model = new CommonForm1Model(codeNo, yousiki1Inf)
        {
            IsEnableICD10Code = true,
            PayLoadInjuryName = 9,
            PayLoadICD10Code = 3,
            PayLoadInjuryNameCode = 4,
            PayLoadModifierCode = 5,
        };

        var finalExaminationYousikiList = yousiki1InfDetailList.Where(x => x.CodeNo == codeNo && x.RowNo == 0 && x.Payload > 2).ToList();
        if (finalExaminationYousikiList.Any())
        {
            RemoveRange(ref yousiki1InfDetailList, finalExaminationYousikiList);
            commonForm1Model.SetData(finalExaminationYousikiList);
        }

        var result = new InputByomeiCommonModel(commonForm1Model, duringMonthMedicineInfModel, finalMedicineDateModel);
        return result;
    }

    /// <summary>
    /// SetData
    /// </summary>
    /// <param name="codeNo"></param>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <param name="maxRow"></param>
    /// <param name="isCanSortRow"></param>
    /// <param name="listType"></param>
    /// <returns></returns>
    private List<CommonForm1Model> SetData(string codeNo, Yousiki1InfModel yousiki1Inf, List<Yousiki1InfDetailModel> yousiki1InfDetailList, int maxRow = 0, bool isCanSortRow = false, ByomeiListType listType = ByomeiListType.None)
    {
        List<CommonForm1Model> listCommonImageModel = new();
        if (yousiki1InfDetailList.Any())
        {
            var listRowNo = yousiki1InfDetailList.Select(x => x.RowNo).Distinct().OrderBy(x => x).ToList();
            foreach (var row in listRowNo)
            {
                switch (listType)
                {
                    case ByomeiListType.None:
                        listCommonImageModel.Add(new CommonForm1Model(yousiki1InfDetailList.Where(x => x.RowNo == row).ToList()));
                        break;
                    case ByomeiListType.HospitalizationStatus:
                        CommonForm1Model commonForm1 = new CommonForm1Model(codeNo, yousiki1Inf)
                        {
                            GridType = listType,
                            DateOfHospitalizationPayLoad = 1,
                            DischargeDatePayLoad = 2,
                            DestinationPayLoad = 3,
                            PayLoadInjuryName = 9,
                            PayLoadICD10Code = 4,
                            PayLoadInjuryNameCode = 5,
                            PayLoadModifierCode = 6,
                        };
                        commonForm1.SetData(yousiki1InfDetailList.Where(x => x.RowNo == row).ToList(), listType);
                        listCommonImageModel.Add(commonForm1);
                        break;
                    case ByomeiListType.StatusHomeVisit:
                        CommonForm1Model commonForm2 = new CommonForm1Model(codeNo, yousiki1Inf)
                        {
                            GridType = listType,
                            HouseCallDatePayLoad = 1,
                            MedicalInstitutionPayLoad = 2,
                            PayLoadInjuryName = 9,
                            PayLoadICD10Code = 3,
                            PayLoadInjuryNameCode = 4,
                            PayLoadModifierCode = 5,
                        };
                        commonForm2.SetData(yousiki1InfDetailList.Where(x => x.RowNo == row).ToList(), listType);
                        listCommonImageModel.Add(commonForm2);
                        break;
                    case ByomeiListType.Rehabilitation:
                        CommonForm1Model commonForm3 = new CommonForm1Model(codeNo, yousiki1Inf)
                        {
                            GridType = listType,
                            IsEnableICD10Code = true,
                            StartDatePayLoad = 1,
                            OnsetDatePayLoad = 2,
                            MaximumNumberDatePayLoad = 3,
                            PayLoadInjuryName = 9,
                            PayLoadICD10Code = 4,
                            PayLoadInjuryNameCode = 5,
                            PayLoadModifierCode = 6,
                        };
                        commonForm3.SetData(yousiki1InfDetailList.Where(x => x.RowNo == row).ToList(), listType);
                        listCommonImageModel.Add(commonForm3);
                        break;
                }
            }
        }
        return listCommonImageModel;
    }

    private Yousiki1InfDetailModel? GetYousiki1InfDetailModel(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList, string codeNo, int rowNo, int payLoad, string valueDefault = "")
    {
        var detail = yousiki1InfDetailList.FirstOrDefault(x => x.CodeNo == codeNo && x.RowNo == rowNo && x.Payload == payLoad);
        if (detail != null)
        {
            yousiki1InfDetailList.Remove(detail);
        }
        return detail;
    }
    #endregion

    #region RenderAtHomeYousiki
    /// <summary>
    /// RenderAtHomeYousiki
    /// </summary>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private AtHomeModel RenderAtHomeYousiki(Yousiki1InfModel yousiki1Inf, List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var statusVisitList = GetStatusVisitModels(ref yousiki1InfDetailList, yousiki1Inf.PtId, yousiki1Inf.SinYm, yousiki1Inf.DataType, yousiki1Inf.SeqNo, CodeNo_StatusOfVisit, 1, 2);
        var statusVisitNursingList = GetStatusVisitModels(ref yousiki1InfDetailList, yousiki1Inf.PtId, yousiki1Inf.SinYm, yousiki1Inf.DataType, yousiki1Inf.SeqNo, CodeNo_StatusOfNursingVisit, 1, 2);
        var statusEmergencyConsultationList = GetStatusEmergencyConsultations(ref yousiki1InfDetailList, yousiki1Inf.PtId, yousiki1Inf.SinYm, yousiki1Inf.DataType, yousiki1Inf.SeqNo, CodeNo_StatusOfEmergencyConsultation, 1, 2, 3, 4);
        var statusShortTermAdmissionList = GetStatusShortTermAdmissionModels(ref yousiki1InfDetailList, yousiki1Inf.PtId, yousiki1Inf.SinYm, yousiki1Inf.DataType, yousiki1Inf.SeqNo, CodeNo_StatusOfShortTermAdmission, 1, 2, 3);
        var patientSitutationList = GetPatientSitutations(ref yousiki1InfDetailList, yousiki1Inf.PtId, yousiki1Inf.SinYm, yousiki1Inf.DataType, yousiki1Inf.SeqNo, CodeNo_PatientSituation, 0, 1);
        var barthelIndexList = GetBarthelIndexModels(ref yousiki1InfDetailList, yousiki1Inf.PtId, yousiki1Inf.SinYm, yousiki1Inf.DataType, yousiki1Inf.SeqNo, CodeNo_BartherIndex, 0, 1);
        var statusNurtritionList = GetStatusNurtritionModels(ref yousiki1InfDetailList, yousiki1Inf.PtId, yousiki1Inf.SinYm, yousiki1Inf.DataType, yousiki1Inf.SeqNo, CodeNo_PresenceNurtrition, 0, 3);
        var hospitalizationStatusList = GetHospitalizationStatus(yousiki1Inf, ref yousiki1InfDetailList);
        var statusHomeVisitList = GetStatusHomeVisits(yousiki1Inf, ref yousiki1InfDetailList);
        var finalExaminationInf = GetAtHomeEndOfMedicineInf(yousiki1Inf, ref yousiki1InfDetailList);
        var finalExaminationInf2 = GetEndOfMedicineInf2(yousiki1Inf, ref yousiki1InfDetailList);

        #region Set byomei data
        List<string> byomeiCdList = new();
        foreach (var item in hospitalizationStatusList)
        {
            byomeiCdList.AddRange(item.GetByomeiCdList());
        }
        foreach (var item in statusHomeVisitList)
        {
            byomeiCdList.AddRange(item.GetByomeiCdList());
        }
        byomeiCdList.AddRange(finalExaminationInf.GetByomeiCdList());
        byomeiCdList.AddRange(finalExaminationInf2.GetByomeiCdList());
        byomeiCdList = byomeiCdList.Distinct().ToList();

        if (byomeiCdList.Any())
        {
            var byomeiDictionary = _ptDiseaseRepository.GetByomeiMst(yousiki1Inf.HpId, byomeiCdList);
            foreach (var item in hospitalizationStatusList)
            {
                item.GetCommonImageInf(byomeiDictionary);
            }
            foreach (var item in statusHomeVisitList)
            {
                item.GetCommonImageInf(byomeiDictionary);
            }
            finalExaminationInf.GetCommonImageInf(byomeiDictionary);
            finalExaminationInf2.GetCommonImageInf(byomeiDictionary);
        }
        #endregion

        var result = new AtHomeModel(
                         yousiki1InfDetailList,
                         statusVisitList,
                         statusVisitNursingList,
                         statusEmergencyConsultationList,
                         statusShortTermAdmissionList,
                         patientSitutationList,
                         barthelIndexList,
                         statusNurtritionList,
                         hospitalizationStatusList,
                         statusHomeVisitList,
                         finalExaminationInf,
                         finalExaminationInf2);
        return result;
    }

    /// <summary>
    /// GetStatusVisitModels
    /// </summary>
    /// <param name="yousiki1InfDetailList"></param>
    /// <param name="ptId"></param>
    /// <param name="sinYm"></param>
    /// <param name="dataType"></param>
    /// <param name="seqNo"></param>
    /// <param name="codeNo"></param>
    /// <param name="payload1"></param>
    /// <param name="payload2"></param>
    /// <returns></returns>
    private List<StatusVisitModel> GetStatusVisitModels(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList, long ptId, int sinYm, int dataType, int seqNo, string codeNo, int payload1, int payload2)
    {
        var sinDateList = yousiki1InfDetailList.Where(item => item.CodeNo == codeNo && item.Payload == payload1).ToList();
        RemoveRange(ref yousiki1InfDetailList, sinDateList);

        var medicalInstitutionList = yousiki1InfDetailList.Where(item => item.CodeNo == codeNo && item.Payload == payload2).ToList();
        RemoveRange(ref yousiki1InfDetailList, medicalInstitutionList);

        var result = sinDateList.Select(item => new StatusVisitModel(
                                                    item,
                                                    medicalInstitutionList.FirstOrDefault(medical => medical.RowNo == item.RowNo)
                                                                          ?? new Yousiki1InfDetailModel(
                                                                                 ptId,
                                                                                 sinYm,
                                                                                 dataType,
                                                                                 seqNo,
                                                                                 codeNo,
                                                                                 item.RowNo,
                                                                                 payload2,
                                                                                 "1")
                                 )).ToList();
        return result;
    }

    /// <summary>
    /// GetStatusEmergencyConsultations
    /// </summary>
    /// <param name="yousiki1InfDetailList"></param>
    /// <param name="ptId"></param>
    /// <param name="sinYm"></param>
    /// <param name="dataType"></param>
    /// <param name="seqNo"></param>
    /// <param name="codeNo"></param>
    /// <param name="payload1"></param>
    /// <param name="payload2"></param>
    /// <param name="payload3"></param>
    /// <param name="payload4"></param>
    /// <returns></returns>
    private List<StatusEmergencyConsultationModel> GetStatusEmergencyConsultations(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList, long ptId, int sinYm, int dataType, int seqNo, string codeNo, int payload1, int payload2, int payload3, int payload4)
    {
        var emergencyConsultationDayList = yousiki1InfDetailList.Where(item => item.CodeNo == codeNo && item.Payload == payload1).ToList();
        RemoveRange(ref yousiki1InfDetailList, emergencyConsultationDayList);

        var destinationList = yousiki1InfDetailList.Where(item => item.CodeNo == codeNo && item.Payload == payload2).ToList();
        RemoveRange(ref yousiki1InfDetailList, destinationList);

        var consultationRouteList = yousiki1InfDetailList.Where(item => item.CodeNo == codeNo && item.Payload == payload3).ToList();
        RemoveRange(ref yousiki1InfDetailList, consultationRouteList);

        var outComeList = yousiki1InfDetailList.Where(item => item.CodeNo == codeNo && item.Payload == payload4).ToList();
        RemoveRange(ref yousiki1InfDetailList, outComeList);

        return emergencyConsultationDayList.Select(item => new StatusEmergencyConsultationModel(
            item,
            destinationList.FirstOrDefault(r => r.RowNo == item.RowNo)
                           ?? new Yousiki1InfDetailModel(
                                  ptId,
                                  sinYm,
                                  dataType,
                                  seqNo,
                                  codeNo,
                                  item.RowNo,
                                  payload2,
                                  "1"),
            consultationRouteList.FirstOrDefault(r => r.RowNo == item.RowNo)
                                 ?? new Yousiki1InfDetailModel(
                                        ptId,
                                        sinYm,
                                        dataType,
                                        seqNo,
                                        codeNo,
                                        item.RowNo,
                                        payload3,
                                       "1"),
            outComeList.FirstOrDefault(r => r.RowNo == item.RowNo)
                       ?? new Yousiki1InfDetailModel(
                              ptId,
                              sinYm,
                              dataType,
                              seqNo,
                              codeNo,
                              item.RowNo,
                              payload4,
                              "1")
            )).ToList();
    }

    /// <summary>
    /// GetStatusShortTermAdmissionModels
    /// </summary>
    /// <param name="ptId"></param>
    /// <param name="sinYm"></param>
    /// <param name="dataType"></param>
    /// <param name="seqNo"></param>
    /// <param name="codeNo"></param>
    /// <param name="payload1"></param>
    /// <param name="payload2"></param>
    /// <param name="payload3"></param>
    /// <returns></returns>
    private List<StatusShortTermAdmissionModel> GetStatusShortTermAdmissionModels(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList, long ptId, int sinYm, int dataType, int seqNo, string codeNo, int payload1, int payload2, int payload3)
    {
        var admissionDateList = yousiki1InfDetailList.Where(item => item.CodeNo == codeNo && item.Payload == payload1).ToList();
        RemoveRange(ref yousiki1InfDetailList, admissionDateList);

        var dischargeDateList = yousiki1InfDetailList.Where(item => item.CodeNo == codeNo && item.Payload == payload2).ToList();
        RemoveRange(ref yousiki1InfDetailList, dischargeDateList);

        var serviceList = yousiki1InfDetailList.Where(item => item.CodeNo == codeNo && item.Payload == payload3).ToList();
        RemoveRange(ref yousiki1InfDetailList, serviceList);

        var result = admissionDateList.Select(item => new StatusShortTermAdmissionModel(
                                                          item,
                                                          dischargeDateList.FirstOrDefault(r => r.RowNo == item.RowNo)
                                                                           ?? new Yousiki1InfDetailModel(
                                                                                  ptId,
                                                                                  sinYm,
                                                                                  dataType,
                                                                                  seqNo,
                                                                                  codeNo,
                                                                                  item.RowNo,
                                                                                  payload2,
                                                                                  string.Empty),
                                                          serviceList.FirstOrDefault(r => r.RowNo == item.RowNo)
                                                                     ?? new Yousiki1InfDetailModel(
                                                                            ptId,
                                                                            sinYm,
                                                                            dataType,
                                                                            seqNo,
                                                                            codeNo,
                                                                            item.RowNo,
                                                                            payload3,
                                                                            "1")
                                         )).ToList();
        return result;
    }

    /// <summary>
    /// GetPatientSitutations
    /// </summary>
    /// <param name="yousiki1InfDetailList"></param>
    /// <param name="ptId"></param>
    /// <param name="sinYm"></param>
    /// <param name="dataType"></param>
    /// <param name="seqNo"></param>
    /// <param name="codeNo"></param>
    /// <param name="rowNo"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    private List<PatientSitutationModel> GetPatientSitutations(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList, long ptId, int sinYm, int dataType, int seqNo, string codeNo, int rowNo, int payload)
    {
        var yousiki1InfDetail = yousiki1InfDetailList.FirstOrDefault(item => item.CodeNo == codeNo
                                                                             && item.RowNo == rowNo
                                                                             && item.Payload == payload);
        if (yousiki1InfDetail != null)
        {
            yousiki1InfDetailList.Remove(yousiki1InfDetail);
        }

        string[] titles = new string[]
        {
                " ＊ 末期の悪性腫瘍",
                " ＊ 在宅自己連続携行式腹膜灌流を行っている状態",
                " ＊ 在宅血液透析を行っている状態",
                " ＊ 在宅酸素療法を行っている状態",
                " ＊ 在宅中心静脈栄養法を行っている状態",
                " ＊ 在宅成分栄養経管栄養法を行っている状態",
                " ＊ 在宅自己導尿を行っている状態",
                " ＊ 在宅人工呼吸を行っている状態",
                " ＊ 植込型脳・脊髄刺激装置による疼（とう）痛管理を行っている状態",
                " ＊ 肺高血圧症であって、プロスタグランジンI2製剤を投与されている状態",
                " ＊ 気管切開を行っている状態",
                " ＊ 気管カニューレを使用している状態",
                " ＊ ドレーンチューブを使用している状態"
        };
        var result = titles.Select((item, index) => new PatientSitutationModel(
                                                        item,
                                                        (yousiki1InfDetail != null && yousiki1InfDetail.Value.Length > index) ? yousiki1InfDetail.Value[index].AsString() : "0"
                           )).ToList();
        return result;
    }

    /// <summary>
    /// GetBarthelIndexModels
    /// </summary>
    /// <param name="yousiki1InfDetailList"></param>
    /// <param name="ptId"></param>
    /// <param name="sinYm"></param>
    /// <param name="dataType"></param>
    /// <param name="seqNo"></param>
    /// <param name="codeNo"></param>
    /// <param name="rowNo"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    private List<BarthelIndexModel> GetBarthelIndexModels(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList, long ptId, int sinYm, int dataType, int seqNo, string codeNo, int rowNo, int payload)
    {
        var yousiki1InfDetail = yousiki1InfDetailList.FirstOrDefault(item => item.CodeNo == codeNo
                                                                             && item.RowNo == rowNo
                                                                             && item.Payload == payload);
        if (yousiki1InfDetail != null)
        {
            yousiki1InfDetailList.Remove(yousiki1InfDetail);
        }

        string[] titles = new string[]
        {
                " ＊ 食事",
                " ＊ 移乗",
                " ＊ 整容",
                " ＊ トイレ動作",
                " ＊ 入浴",
                " ＊ 平地歩行",
                " ＊ 階段",
                " ＊ 更衣",
                " ＊ 排便管理",
                " ＊ 排尿管理",
        };
        var result = titles.Select((item, index) => new BarthelIndexModel(
                                                        item,
                                                        (yousiki1InfDetail != null && yousiki1InfDetail.Value.Length > index) ? yousiki1InfDetail.Value[index].ToString() : "0"
                           )).ToList();
        return result;
    }

    /// <summary>
    /// GetStatusNurtritionModels
    /// </summary>
    /// <param name="ptId"></param>
    /// <param name="sinYm"></param>
    /// <param name="dataType"></param>
    /// <param name="seqNo"></param>
    /// <param name="codeNo"></param>
    /// <param name="rowNo"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    private List<StatusNurtritionModel> GetStatusNurtritionModels(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList, long ptId, int sinYm, int dataType, int seqNo, string codeNo, int rowNo, int payload)
    {
        var yousiki1InfDetail = yousiki1InfDetailList.FirstOrDefault(item => item.CodeNo == codeNo
                                                                             && item.RowNo == rowNo
                                                                             && item.Payload == payload);

        if (yousiki1InfDetail != null)
        {
            yousiki1InfDetailList.Remove(yousiki1InfDetail);
        }
        string[] titles = new string[]
        {
                " ＊ 経鼻胃管",
                " ＊ 胃瘻・腸瘻",
                " ＊ 末梢静脈栄養",
                " ＊ 中心静脈栄養",
                " ＊ 皮下注射"
        };
        var result = titles.Select((item, index) => new StatusNurtritionModel(
                                                        item,
                                                        (yousiki1InfDetail != null && yousiki1InfDetail.Value.Length > index) ? yousiki1InfDetail.Value[index].ToString() : "0"
                           )).ToList();
        return result;
    }

    /// <summary>
    /// GetHospitalizationStatus
    /// </summary>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private List<CommonForm1Model> GetHospitalizationStatus(Yousiki1InfModel yousiki1Inf, ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var yousikiHospitalizationStatusList = yousiki1InfDetailList.Where(item => item.CodeNo == CodeNo_HospitalizationStatus).ToList();
        RemoveRange(ref yousiki1InfDetailList, yousikiHospitalizationStatusList);

        var result = SetData(CodeNo_HospitalizationStatus, yousiki1Inf, yousikiHospitalizationStatusList, isCanSortRow: false, listType: ByomeiListType.HospitalizationStatus);
        return result;
    }

    /// <summary>
    /// GetStatusHomeVisits
    /// </summary>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private List<CommonForm1Model> GetStatusHomeVisits(Yousiki1InfModel yousiki1Inf, ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var yousikiStatusHomeVisitList = yousiki1InfDetailList.Where(item => item.CodeNo == CodeNo_StatusHomeVisit).ToList();
        RemoveRange(ref yousiki1InfDetailList, yousikiStatusHomeVisitList);

        var result = SetData(CodeNo_StatusHomeVisit, yousiki1Inf, yousikiStatusHomeVisitList, isCanSortRow: false, listType: ByomeiListType.StatusHomeVisit);
        return result;
    }

    /// <summary>
    /// GetAtHomeEndOfMedicineInf
    /// </summary>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private CommonForm1Model GetAtHomeEndOfMedicineInf(Yousiki1InfModel yousiki1Inf, ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var endOfMedicineYousikiList = yousiki1InfDetailList.Where(item => item.CodeNo == CodeNo_MainInjuryOfVisit && item.RowNo == 0 && item.Payload >= 2).ToList();

        var finalExaminationInf = new CommonForm1Model(CodeNo_MainInjuryOfVisit, yousiki1Inf)
        {
            IsEnableICD10Code = true,
            PayLoadInjuryName = 9,
            PayLoadICD10Code = 2,
            PayLoadInjuryNameCode = 3,
            PayLoadModifierCode = 4,
        };

        if (endOfMedicineYousikiList.Any())
        {
            RemoveRange(ref yousiki1InfDetailList, endOfMedicineYousikiList);
            finalExaminationInf.SetData(endOfMedicineYousikiList);
        }

        return finalExaminationInf;
    }

    /// <summary>
    /// GetEndOfMedicineInf2
    /// </summary>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private CommonForm1Model GetEndOfMedicineInf2(Yousiki1InfModel yousiki1Inf, ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var finalExaminationInfYousikiList = yousiki1InfDetailList.Where(x => x.CodeNo == CodeNo_ApprearanceDiagnosis && x.RowNo == 0 && x.Payload >= 2).ToList();
        var finalExaminationInf2 = new CommonForm1Model(CodeNo_ApprearanceDiagnosis, yousiki1Inf)
        {
            IsEnableICD10Code = true,
            PayLoadInjuryName = 9,
            PayLoadICD10Code = 2,
            PayLoadInjuryNameCode = 3,
            PayLoadModifierCode = 4,
        };

        if (finalExaminationInfYousikiList.Any())
        {
            RemoveRange(ref yousiki1InfDetailList, finalExaminationInfYousikiList);
            finalExaminationInf2.SetData(finalExaminationInfYousikiList);
        }
        return finalExaminationInf2;
    }
    #endregion

    #region RenderLivingHabit
    /// <summary>
    /// RenderLivingHabit
    /// </summary>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private LivingHabitModel RenderLivingHabit(List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var acuteAorticHistoryList = GetAcuteAorticHistories(ref yousiki1InfDetailList);
        var acuteCoronaryHistoryList = GetAcuteCoronaryHistories(ref yousiki1InfDetailList);
        var strokeHistoryList = GetStrokeHistories(ref yousiki1InfDetailList);
        var outpatientConsultationInfList = GetOutpatientConsultationInfs(ref yousiki1InfDetailList);
        var livingHabitModel = new LivingHabitModel(
                                   yousiki1InfDetailList,
                                   outpatientConsultationInfList,
                                   strokeHistoryList,
                                   acuteCoronaryHistoryList,
                                   acuteAorticHistoryList);
        return livingHabitModel;
    }

    /// <summary>
    /// GetAcuteAorticHistories
    /// </summary>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private List<AcuteAorticDissectionHistoryModel> GetAcuteAorticHistories(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var yousikiAcuteAorticDissectionHistoryList = yousiki1InfDetailList.Where(item => item.CodeNo == _listAcuteAorticCodeNo[1] && item.Payload == 1).ToList();
        RemoveRange(ref yousiki1InfDetailList, yousikiAcuteAorticDissectionHistoryList);

        return yousikiAcuteAorticDissectionHistoryList.OrderBy(item => item.RowNo)
                                                      .Select(item => new AcuteAorticDissectionHistoryModel(item))
                                                      .ToList();
    }

    /// <summary>
    /// GetAcuteCoronaryHistories
    /// </summary>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private List<AcuteCoronaryHistoryModel> GetAcuteCoronaryHistories(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var typeList = yousiki1InfDetailList.Where(item => item.CodeNo == _listAcuteCoronaryCodeNo[1] && item.Payload == 1).ToList();
        RemoveRange(ref yousiki1InfDetailList, typeList);

        var onsetDateList = yousiki1InfDetailList.Where(item => item.CodeNo == _listAcuteCoronaryCodeNo[1] && item.Payload == 2).ToList();
        RemoveRange(ref yousiki1InfDetailList, onsetDateList);

        var result = typeList.Select(item => new AcuteCoronaryHistoryModel(
                                                 item,
                                                 onsetDateList.FirstOrDefault(onset => onset.RowNo == item.RowNo) ?? new()
                             )).ToList();

        result = result.OrderByDescending(x => x.OnsetDate.Value).ToList();
        int sortNo = 1;
        foreach (var item in result)
        {
            item.UpdateSortNo(sortNo);
            sortNo++;
        }
        return result;
    }

    /// <summary>
    /// GetStrokeHistories
    /// </summary>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private List<StrokeHistoryModel> GetStrokeHistories(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var typeList = yousiki1InfDetailList.Where(x => x.CodeNo == _listStrokeCodeNo[1] && x.Payload == 1).ToList();
        RemoveRange(ref yousiki1InfDetailList, typeList);

        var onsetDateList = yousiki1InfDetailList.Where(x => x.CodeNo == _listStrokeCodeNo[1] && x.Payload == 2).ToList();
        RemoveRange(ref yousiki1InfDetailList, onsetDateList);

        var result = typeList.Select(item => new StrokeHistoryModel(
                                                 item,
                                                 onsetDateList.FirstOrDefault(onset => onset.RowNo == item.RowNo) ?? new()
                             )).ToList();

        result = result.OrderByDescending(x => x.OnsetDate.Value).ToList();
        int sortNo = 1;
        foreach (var item in result)
        {
            item.UpdateSortNo(sortNo);
            sortNo++;
        }
        return result;
    }

    /// <summary>
    /// GetOutpatientConsultationInfs
    /// </summary>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private List<OutpatientConsultationInfModel> GetOutpatientConsultationInfs(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var consultationDateList = yousiki1InfDetailList.Where(item => item.CodeNo == _outpatientConsultationInfCodeNo && item.Payload == 1).ToList();
        RemoveRange(ref yousiki1InfDetailList, consultationDateList);

        var firstVisitList = yousiki1InfDetailList.Where(item => item.CodeNo == _outpatientConsultationInfCodeNo && item.Payload == 2).ToList();
        RemoveRange(ref yousiki1InfDetailList, firstVisitList);

        var appearanceReferralList = yousiki1InfDetailList.Where(item => item.CodeNo == _outpatientConsultationInfCodeNo && item.Payload == 3).ToList();
        RemoveRange(ref yousiki1InfDetailList, appearanceReferralList);

        var departmentCodeList = yousiki1InfDetailList.Where(item => item.CodeNo == _outpatientConsultationInfCodeNo && item.Payload == 4).ToList();
        RemoveRange(ref yousiki1InfDetailList, departmentCodeList);

        return consultationDateList.Select(item => new OutpatientConsultationInfModel(
                                                       item,
                                                       firstVisitList.FirstOrDefault(firstVisit => firstVisit.RowNo == item.RowNo) ?? new(),
                                                       appearanceReferralList.FirstOrDefault(appearanceReferral => appearanceReferral.RowNo == item.RowNo) ?? new(),
                                                       departmentCodeList.FirstOrDefault(departmentCode => departmentCode.RowNo == item.RowNo) ?? new()
                                   )).ToList();
    }
    #endregion

    #region RenderRehabilitation
    /// <summary>
    /// RenderRehabilitation
    /// </summary>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private RehabilitationModel RenderRehabilitation(Yousiki1InfModel yousiki1Inf, List<Yousiki1InfDetailModel> yousiki1InfDetailList, Dictionary<string, string> kacodeYousikiMstDict)
    {
        var outpatientConsultationList = GetOutpatientConsultation(ref yousiki1InfDetailList, kacodeYousikiMstDict);
        var byomeiRehabilitationList = GetByomeiRehabilitation(yousiki1Inf, ref yousiki1InfDetailList);
        var patientStatus = GetPatientStatus(ref yousiki1InfDetailList);
        var barthelIndexList = patientStatus.barthelIndexList;
        var FIMList = patientStatus.FIMList;

        #region set byomei data
        List<string> byomeiCdList = new();
        foreach (var item in byomeiRehabilitationList)
        {
            byomeiCdList.AddRange(item.GetByomeiCdList());
        }
        byomeiCdList = byomeiCdList.Distinct().ToList();
        if (byomeiCdList.Any())
        {
            var byomeiDictionary = _ptDiseaseRepository.GetByomeiMst(yousiki1Inf.HpId, byomeiCdList);
            foreach (var item in byomeiRehabilitationList)
            {
                item.GetCommonImageInf(byomeiDictionary);
            }
        }
        #endregion

        var result = new RehabilitationModel(
                         yousiki1InfDetailList,
                         outpatientConsultationList,
                         byomeiRehabilitationList,
                         barthelIndexList,
                         FIMList);
        return result;
    }

    /// <summary>
    /// GetOutpatientConsultation
    /// </summary>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private List<OutpatientConsultationModel> GetOutpatientConsultation(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList, Dictionary<string, string> kacodeYousikiMstDict)
    {
        List<OutpatientConsultationModel> result = new();
        var yousikiOutpatientConsultationList = yousiki1InfDetailList.Where(item => item.CodeNo == CodeNo_OutpatientConsultate).ToList();
        if (yousikiOutpatientConsultationList.Any())
        {
            RemoveRange(ref yousiki1InfDetailList, yousikiOutpatientConsultationList);
            var listRowNo = yousikiOutpatientConsultationList.Select(item => item.RowNo).Distinct().OrderBy(item => item).ToList();
            foreach (var rowNo in listRowNo)
            {
                var detailList = yousikiOutpatientConsultationList.Where(x => x.RowNo == rowNo).ToList();
                result.Add(new OutpatientConsultationModel(detailList, kacodeYousikiMstDict));
            }
        }
        return result;
    }

    /// <summary>
    /// GetByomeiRehabilitation
    /// </summary>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private List<CommonForm1Model> GetByomeiRehabilitation(Yousiki1InfModel yousiki1Inf, ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var yousikiByomeiRehabilitationList = yousiki1InfDetailList.Where(item => item.CodeNo == CodeNo_ByomeiRehabilitation).ToList();
        RemoveRange(ref yousiki1InfDetailList, yousikiByomeiRehabilitationList);

        var result = SetData(CodeNo_ByomeiRehabilitation, yousiki1Inf, yousikiByomeiRehabilitationList, maxRow: 5, isCanSortRow: false, listType: ByomeiListType.Rehabilitation);
        return result;
    }

    /// <summary>
    /// GetPatientStatus
    /// </summary>
    /// <param name="yousiki1InfDetailList"></param>
    /// <returns></returns>
    private (List<PatientStatusModel> barthelIndexList, List<PatientStatusModel> FIMList) GetPatientStatus(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        var yousikiPatientStatus = yousiki1InfDetailList.Where(item => item.CodeNo == CodeNo_PatientStatus).ToList();
        RemoveRange(ref yousiki1InfDetailList, yousikiPatientStatus);

        var listBarthelIndexLabel = new List<string> { "＊食事", "＊移乗", "＊整容", "＊トイレ動作", "＊入浴", "＊平地歩行", "＊階段", "＊更衣", "＊排便管理", "＊排尿管理" };
        var listFIMLabel = new List<string> { "＊食事", "＊整容", "＊清拭", "＊更衣（上半身）", "＊更衣（下半身）", "＊トイレ", "＊排尿コントロール", "＊排便コントロール",
                                                  "＊ベッド・車椅子移乗", "＊トイレ移乗", "＊浴槽・シャワー移乗", "＊歩行・車椅子移動", "＊階段移動", "＊理解", "＊表出", "＊社会的交流", "＊問題解決", "＊記憶"};

        string valueBarthelIndex = "0000000000";
        string valueFIM = "111111111111111111";


        var detailDefault = yousiki1InfDetailList.FirstOrDefault(item => item.CodeNo == CodeNo_PatientStatus && item.Payload == 1);
        if (detailDefault != null)
        {
            valueBarthelIndex = detailDefault.Value;
        }

        detailDefault = yousiki1InfDetailList.FirstOrDefault(item => item.CodeNo == CodeNo_PatientStatus && item.Payload == 2);
        if (detailDefault != null)
        {
            valueFIM = detailDefault.Value;
        }

        var barthelIndexList = ConvertPatientStatus(valueBarthelIndex, listBarthelIndexLabel);
        var FIMList = ConvertPatientStatus(valueFIM, listFIMLabel);

        return (barthelIndexList, FIMList);
    }

    /// <summary>
    /// ConvertPatientStatus
    /// </summary>
    /// <param name="value"></param>
    /// <param name="listLabel"></param>
    /// <returns></returns>
    private List<PatientStatusModel> ConvertPatientStatus(string value, List<string> listLabel)
    {
        List<PatientStatusModel> patientStatusList = new();

        if (string.IsNullOrEmpty(value) || listLabel == null || value.Length != listLabel.Count)
        {
            return patientStatusList;
        }

        for (int i = 0; i < value.Length; i++)
        {
            patientStatusList.Add(new PatientStatusModel(listLabel[i], value[i].AsInteger()));
        }

        return patientStatusList;
    }
    #endregion

    #region common function
    private void RemoveRange(ref List<Yousiki1InfDetailModel> yousiki1InfDetailList, List<Yousiki1InfDetailModel> removeYousiki1InfDetailList)
    {
        foreach (var item in removeYousiki1InfDetailList)
        {
            yousiki1InfDetailList.Remove(item);
        }
    }
    #endregion
}
