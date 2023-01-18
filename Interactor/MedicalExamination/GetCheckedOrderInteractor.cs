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

                var ordInfs = inputData.OdrInfItems.Select(o => new OrdInfModel(
                        o.InOutKbn,
                        o.OdrKouiKbn,
                        o.OdrInfDetailItems.Select(od => new OrdInfDetailModel(
                                od.ItemCd,
                                od.SinKouiKbn
                            )).ToList()
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
                if (inputData.SyosaisinKbn != SyosaiConst.Jihi)
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
