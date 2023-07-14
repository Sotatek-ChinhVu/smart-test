﻿using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models.Diseases;
using Domain.Models.MedicalExamination;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.Reception;
using Domain.Models.TodayOdr;
using Entity.Tenant;
using Helper.Constants;
using Interactor.CalculateService;
using UseCase.MedicalExamination.GetCheckedOrder;

namespace Interactor.MedicalExamination
{
    public class GetCheckedOrderInteractor : IGetCheckedOrderInputPort
    {
        private readonly IMedicalExaminationRepository _medicalExaminationRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly ICalculateService _calculateRepository;
        private readonly ITodayOdrRepository _todayOdrRepository;
        private readonly IMstItemRepository _mstItemRepository;

        public GetCheckedOrderInteractor(IMedicalExaminationRepository medicalExaminationRepository, IReceptionRepository receptionRepository, ICalculateService calculateRepository, ITodayOdrRepository todayOdrRepository, IMstItemRepository mstItemRepository)
        {
            _medicalExaminationRepository = medicalExaminationRepository;
            _receptionRepository = receptionRepository;
            _calculateRepository = calculateRepository;
            _todayOdrRepository = todayOdrRepository;
            _mstItemRepository = mstItemRepository;
        }

        public GetCheckedOrderOutputData Handle(GetCheckedOrderInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidHpId, new());
                }
                if (inputData.UserId <= 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidUserId, new());
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidSinDate, new());
                }
                if (inputData.HokenId <= 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidHokenId, new());
                }
                if (inputData.PtId <= 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidPtId, new());
                }
                if (inputData.IBirthDay <= 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidIBirthDay, new());
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidRaiinNo, new());
                }
                if (inputData.SyosaisinKbn < 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidSyosaisinKbn, new());
                }
                if (inputData.OyaRaiinNo <= 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidOyaRaiinNo, new());
                }
                if (inputData.TantoId <= 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidTantoId, new());
                }
                //if (inputData.HokenPid <= 0)
                //{
                //    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidHokenPid, new());
                //}

                if (inputData.PrimaryDoctor < 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidPrimaryDoctor, new());
                }

                var ordInfs = inputData.OdrInfItems.Select(o => new OrdInfModel(
                        o.HpId,
                        o.RaiinNo,
                        o.RpNo,
                        o.RpEdaNo,
                        o.PtId,
                        o.SinDate,
                        o.HokenPid,
                        o.OdrKouiKbn,
                        string.Empty,
                        o.InoutKbn,
                        o.SikyuKbn,
                        o.SyohoSbt,
                        o.SanteiKbn,
                        0,
                        o.DaysCnt,
                        o.SortNo,
                        o.IsDeleted,
                        0,
                        o.DetailInfoList.Select(od => new OrdInfDetailModel(
                                od.HpId,
                                od.RaiinNo,
                                od.RpNo,
                                od.RpEdaNo,
                                od.RowNo,
                                od.PtId,
                                od.SinDate,
                                od.SinKouiKbn,
                                od.ItemCd,
                                od.ItemName,
                                od.Suryo,
                                od.UnitName,
                                0,
                                od.TermVal,
                                0,
                                od.SyohoKbn,
                                0,
                                od.DrugKbn,
                                od.YohoKbn,
                                od.Kokuji1,
                                od.Kokuji2,
                                od.IsNodspRece,
                        od.IpnCd,
                                string.Empty,
                                0,
                                DateTime.MinValue,
                                0,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                od.CmtOpt,
                                string.Empty,
                                0,
                                string.Empty,
                                o?.InoutKbn ?? 0,
                                0,
                                true,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                0,
                                "",
                                new List<YohoSetMstModel>(),
                                0,
                                0,
                                "",
                                "",
                                "",
                                ""
                            )).ToList(),
                        DateTime.MinValue,
                        0,
                        "",
                        DateTime.MinValue,
                        0,
                        "",
                        string.Empty,
                        string.Empty
                    )).ToList();

                var diseases = inputData.DiseaseItems.Select(i => new PtDiseaseModel(
                        i.SikkanKbn,
                        i.HokenPid,
                        i.StartDate,
                        i.TenkiKbn,
                        i.TenkiDate,
                        i.SyubyoKbn
                    )).ToList();

                var checkedOrderModelList = new List<CheckedOrderModel>();
                var allOdrInfDetail = new List<OrdInfDetailModel>();
                foreach (var odr in ordInfs)
                {
                    allOdrInfDetail.AddRange(odr.OrdInfDetails);
                }
                if (inputData.SyosaisinKbn != SyosaiConst.Jihi && inputData.EnabledSanteiCheck)
                {
                    bool isJouhou = allOdrInfDetail.Any(d => d.ItemCd == ItemCdConst.Con_Jouhou);
                    List<CheckedOrderModel> checkingOrders = _medicalExaminationRepository.IgakuTokusitu(inputData.HpId, inputData.SinDate, inputData.HokenId, inputData.SyosaisinKbn, diseases, allOdrInfDetail, isJouhou);
                    checkingOrders = _medicalExaminationRepository.IgakuTokusituIsChecked(inputData.HpId, inputData.SinDate, inputData.SyosaisinKbn, checkingOrders, allOdrInfDetail);
                    checkedOrderModelList.AddRange(checkingOrders);
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.SihifuToku1(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.HokenId, inputData.SyosaisinKbn, inputData.RaiinNo, inputData.OyaRaiinNo, diseases, allOdrInfDetail, isJouhou));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.SihifuToku2(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.HokenId, inputData.IBirthDay, inputData.RaiinNo, inputData.SyosaisinKbn, inputData.OyaRaiinNo, diseases, allOdrInfDetail, ordInfs.Select(x => x.OdrKouiKbn).ToList(), isJouhou));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.IgakuTenkan(inputData.HpId, inputData.SinDate, inputData.HokenId, inputData.SyosaisinKbn, diseases, allOdrInfDetail, isJouhou));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.IgakuNanbyo(inputData.HpId, inputData.SinDate, inputData.HokenId, inputData.SyosaisinKbn, diseases, allOdrInfDetail, isJouhou));
                    checkedOrderModelList = _medicalExaminationRepository.InitPriorityCheckDetail(checkedOrderModelList);
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.TouyakuTokusyoSyoho(inputData.HpId, inputData.SinDate, inputData.HokenId, diseases, allOdrInfDetail, ordInfs).Where(c => c.CheckingType > 0));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.ChikiHokatu(inputData.HpId, inputData.PtId, inputData.UserId, inputData.SinDate, inputData.PrimaryDoctor, inputData.TantoId, allOdrInfDetail, inputData.SyosaisinKbn));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.YakkuZai(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.IBirthDay, allOdrInfDetail, ordInfs));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.SiIkuji(inputData.HpId, inputData.SinDate, inputData.IBirthDay, allOdrInfDetail, isJouhou, inputData.SyosaisinKbn));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.TrialIryoJyohoKibanCalculation(allOdrInfDetail));

                    var odrItems = ordInfs.Select(o => new OdrInfItem(
                            o.HpId,
                            o.PtId,
                            o.SinDate,
                            o.RaiinNo,
                            o.RpNo,
                            o.RpEdaNo,
                            o.HokenPid,
                            o.OdrKouiKbn,
                            o.InoutKbn,
                            o.SikyuKbn,
                            o.SyohoSbt,
                            o.SanteiKbn,
                            o.DaysCnt,
                            o.SortNo,
                            o.IsDeleted,
                            o.OrdInfDetails.Select(od =>
                                    new OdrInfDetailItem(
                                            od.HpId,
                                            od.PtId,
                                            od.SinDate,
                                            od.RaiinNo,
                                            od.RpNo,
                                            od.RpEdaNo,
                                            od.RowNo,
                                            od.SinKouiKbn,
                                            od.ItemCd,
                                            od.Suryo,
                                            od.UnitName,
                                            od.TermVal,
                                            od.SyohoKbn,
                                            od.DrugKbn,
                                            od.YohoKbn,
                                            od.Kokuji1,
                                            od.Kokuji2,
                                            od.IsNodspRece,
                                            od.IpnCd,
                                            od.IpnName,
                                            od.CmtOpt,
                                            od.ItemName,
                                            od.IsDummy
                                        )
                                ).ToList()
                        )).ToList();

                    long maxRpNoOnDB = _todayOdrRepository.GetMaxRpNo(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate);
                    long maxRpNo = Math.Max(maxRpNoOnDB, 1);

                    foreach (var itemCd in checkedOrderModelList.Select(c => c.ItemCd))
                    {
                        odrItems.Add(CreateIkaTodayOdrInfModel(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.HokenPid, itemCd, maxRpNo));

                        // 追加した項目のDummyフラグをセット
                        foreach (var detail in odrItems.Last().DetailInfoList)
                        {
                            detail.IsDummy = true;
                        }

                        maxRpNo++;
                    }

                    var raiinInf = _receptionRepository.Get(inputData.RaiinNo);
                    var requestRaiinInf = new ReceptionItem(raiinInf);
                    var runTraialCalculateRequest = new RunTraialCalculateRequest(
                            inputData.HpId,
                            inputData.PtId,
                            inputData.SinDate,
                            inputData.RaiinNo,
                            odrItems,
                            requestRaiinInf,
                            false
                        );

                    var runTrialCalculate = _calculateRepository.RunTrialCalculate(runTraialCalculateRequest);

                    var itemCds = runTrialCalculate.SinMeiList.Select(x => x.ItemCd).Distinct().ToList();

                    checkedOrderModelList = checkedOrderModelList.Where(c => itemCds.Contains(c.ItemCd)).ToList();

                    checkedOrderModelList.AddRange(_medicalExaminationRepository.Zanyaku(inputData.HpId, inputData.SinDate, allOdrInfDetail, ordInfs));
                }

                if (checkedOrderModelList.Any(c => c.ItemCd == ItemCdConst.YakuzaiJohoTeiyo)
                    && !checkedOrderModelList.Any(c => c.ItemCd == ItemCdConst.YakuzaiJoho))
                {
                    checkedOrderModelList.RemoveAll(c => c.ItemCd == ItemCdConst.YakuzaiJohoTeiyo);
                }

                return new GetCheckedOrderOutputData(GetCheckedOrderStatus.Successed, checkedOrderModelList);
            }
            finally
            {
                _medicalExaminationRepository.ReleaseResource();
            }
        }

        private OdrInfItem CreateIkaTodayOdrInfModel(int hpId, long ptId, int sinDate, long raiinNo, int hokenPid, string itemCd, long maxRpNo)
        {
            var tenMst = _mstItemRepository.FindTenMst(hpId, itemCd, sinDate);
            List<OdrInfDetailItem> odrInfDetails = new List<OdrInfDetailItem>();

            OdrInfDetailItem detail = new OdrInfDetailItem(
                hpId,
                ptId,
                sinDate,
                raiinNo,
                maxRpNo + 1,
                1,
                1,
                tenMst.SinKouiKbn,
                itemCd,
                0,
                string.Empty,
                0,
                0,
                0,
                0,
                String.Empty,
                String.Empty,
                0,
                String.Empty,
                String.Empty,
                String.Empty,
                String.Empty,
                false
            );

            odrInfDetails.Add(detail);
            

            OdrInfItem odrInf = new OdrInfItem(
                    hpId,
                    ptId,
                    sinDate,
                    raiinNo,
                    maxRpNo + 1,
                    1,
                    hokenPid,
                    tenMst.SinKouiKbn,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    odrInfDetails
                );
       

            return odrInf;
        }
    }
}
