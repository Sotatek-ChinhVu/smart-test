using Domain.Models.Diseases;
using Domain.Models.MedicalExamination;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Helper.Constants;
using UseCase.MedicalExamination.GetCheckedOrder;

namespace Interactor.MedicalExamination
{
    public class GetCheckedOrderInteractor : IGetCheckedOrderInputPort
    {
        private readonly IMedicalExaminationRepository _medicalExaminationRepository;
        public GetCheckedOrderInteractor(IMedicalExaminationRepository medicalExaminationRepository)
        {
            _medicalExaminationRepository = medicalExaminationRepository;
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
                if (inputData.PrimaryDoctor < 0)
                {
                    return new GetCheckedOrderOutputData(GetCheckedOrderStatus.InvalidPrimaryDoctor, new());
                }

                var ordInfs = inputData.OdrInfItemInputDatas.Select(o => new OrdInfModel(
                        o.HpId,
                        o.RaiinNo,
                        o.RpNo,
                        o.RpEdaNo,
                        o.PtId,
                        o.SinDate,
                        o.HokenPid,
                        o.OdrKouiKbn,
                        o.RpName,
                        o.InoutKbn,
                        o.SikyuKbn,
                        o.SyohoSbt,
                        o.SanteiKbn,
                        o.TosekiKbn,
                        o.DaysCnt,
                        o.SortNo,
                        o.IsDeleted,
                        o.Id,
                        o.OdrDetails.Select(od => new OrdInfDetailModel(
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
                                od.UnitSbt,
                                od.TermVal,
                                od.KohatuKbn,
                                od.SyohoKbn,
                                od.SyohoLimitKbn,
                                od.DrugKbn,
                                od.YohoKbn,
                                od.Kokuji1,
                                od.Kokuji2,
                                od.IsNodspRece,
                                od.IpnCd,
                                string.Empty,
                                od.JissiKbn,
                                od.JissiDate,
                                od.JissiId,
                                od.JissiMachine,
                                od.ReqCd,
                                od.Bunkatu,
                                od.CmtName,
                                od.CmtOpt,
                                od.FontColor,
                                od.CommentNewline,
                                string.Empty,
                                0,
                                0,
                                false,
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
                        ""
                    )).ToList();

                var diseases = inputData.PtDiseaseListInputItems.Select(i => new PtDiseaseModel(
                        i.HpId,
                        i.PtId,
                        i.SeqNo,
                        i.ByomeiCd,
                        i.SortNo,
                        i.PrefixList,
                        i.SuffixList,
                        i.Byomei,
                        i.StartDate,
                        i.TenkiKbn,
                        i.TenkiDate,
                        i.SyubyoKbn,
                        i.SikkanKbn,
                        i.NanByoCd,
                        i.IsNodspRece,
                        i.IsNodspKarte,
                        i.IsDeleted,
                        i.Id,
                        i.IsImportant,
                        0,
                        "",
                        "",
                        "",
                        "",
                        i.HokenPid,
                        i.HosokuCmt
                    )).ToList();

                var checkedOrderModelList = new List<CheckedOrderModel>();
                var allOdrInfDetail = new List<OrdInfDetailModel>();
                foreach (var odr in ordInfs)
                {
                    allOdrInfDetail.AddRange(odr.OrdInfDetails);
                }
                if (inputData.SyosaisinKbn != SyosaiConst.Jihi)
                {
                    bool isJouhou = allOdrInfDetail.Any(d => d.ItemCd == ItemCdConst.Con_Jouhou);
                    List<CheckedOrderModel> checkingOrders = _medicalExaminationRepository.IgakuTokusitu(inputData.HpId, inputData.SinDate, inputData.HokenId, inputData.SyosaisinKbn, diseases, allOdrInfDetail, isJouhou);
                    _medicalExaminationRepository.IgakuTokusituIsChecked(inputData.HpId, inputData.SinDate, inputData.SyosaisinKbn, ref checkingOrders, allOdrInfDetail);
                    checkedOrderModelList.AddRange(checkingOrders);
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.SihifuToku1(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.HokenId, inputData.SyosaisinKbn, inputData.RaiinNo, inputData.OyaRaiinNo, diseases, allOdrInfDetail, isJouhou));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.SihifuToku2(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.HokenId, inputData.IBirthDay, inputData.RaiinNo, inputData.SyosaisinKbn, inputData.OyaRaiinNo, diseases, allOdrInfDetail, ordInfs.Select(x => x.OdrKouiKbn).ToList(), isJouhou));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.IgakuTenkan(inputData.HpId, inputData.SinDate, inputData.HokenId, inputData.SyosaisinKbn, diseases, allOdrInfDetail, isJouhou));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.IgakuNanbyo(inputData.HpId, inputData.SinDate, inputData.HokenId, inputData.SyosaisinKbn, diseases, allOdrInfDetail, isJouhou));
                    checkedOrderModelList = _medicalExaminationRepository.InitPriorityCheckDetail(checkedOrderModelList);
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.TouyakuTokusyoSyoho(inputData.HpId, inputData.SinDate, inputData.HokenId, diseases, allOdrInfDetail, ordInfs));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.ChikiHokatu(inputData.HpId, inputData.PtId, inputData.UserId, inputData.SinDate, inputData.PrimaryDoctor, inputData.TantoId, allOdrInfDetail, inputData.SyosaisinKbn));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.YakkuZai(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.IBirthDay, allOdrInfDetail, ordInfs));
                    checkedOrderModelList.AddRange(_medicalExaminationRepository.SiIkuji(inputData.HpId, inputData.SinDate, inputData.IBirthDay, allOdrInfDetail, isJouhou, inputData.SyosaisinKbn));

                    //checkingOrderModelList.AddRange(OnlineIgaku(allOdrInfDetail));
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
    }
}
