using Domain.Models.HpMst;
using Domain.Models.NextOrder;
using Domain.Models.PatientInfor;
using Domain.Models.User;
using UseCase.NextOrder.Upsert;
using static Helper.Constants.KarteConst;
using static Helper.Constants.NextOrderConst;
using static Helper.Constants.OrderInfConst;
using static Helper.Constants.RsvkrtByomeiConst;

namespace Interactor.NextOrder
{
    public class UpsertNextOrderListInteractor : IUpsertNextOrderListInputPort
    {
        private readonly INextOrderRepository _nextOrderRepository;
        private readonly IHpInfRepository _hpInfRepository;
        private readonly IPatientInforRepository _patientInfRepository;
        private readonly IUserRepository _userRepository;

        public UpsertNextOrderListInteractor(INextOrderRepository nextOrderRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInfRepository, IUserRepository userRepository)
        {
            _nextOrderRepository = nextOrderRepository;
            _hpInfRepository = hpInfRepository;
            _patientInfRepository = patientInfRepository;
            _userRepository = userRepository;
        }

        public UpsertNextOrderListOutputData Handle(UpsertNextOrderListInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0 || !_hpInfRepository.CheckHpId(inputData.HpId))
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidHpId, new(), new(), new(), new());
                }
                if (inputData.PtId <= 0 || !_patientInfRepository.CheckExistListId(new List<long> { inputData.PtId }))
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidPtId, new(), new(), new(), new());
                }
                if (inputData.UserId <= 0 || !_userRepository.CheckExistedUserId(inputData.UserId))
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidUserId, new(), new(), new(), new());
                }

                var nextOrderModels = inputData.NextOrderItems.Select(n => ConvertNextOrderToModel(inputData.HpId, inputData.PtId, n)).ToList();
                List<(int, KarteValidationStatus)> validationKarteInfs = new();
                Dictionary<int, NextOrderStatus> validationNextOrders = new();
                List<(int, int, RsvkrtByomeiStatus)> validationRsvkrtByomeis = new();
                var validationOrdInfs = new List<(int, Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>)>();

                for (int i = 0; i < nextOrderModels.Count; i++)
                {
                    var nextOrderModel = nextOrderModels[i];

                    var validationNextOrder = nextOrderModel.Validation();
                    if (validationNextOrder != NextOrderStatus.Valid)
                    {
                        validationNextOrders.Add(i, validationNextOrder);
                    }

                    var validationKarteInf = nextOrderModel.RsvkrtKarteInf.Validation();
                    if (validationKarteInf != KarteValidationStatus.Valid)
                    {
                        validationKarteInfs.Add((i, validationKarteInf));
                    }

                    for (int i1 = 0; i1 < nextOrderModel.RsvkrtByomeis.Count; i1++)
                    {
                        var byomei = nextOrderModel.RsvkrtByomeis[i1];
                        var validationByomei = byomei.Validation();
                        if (validationKarteInf != KarteValidationStatus.Valid)
                        {
                            validationRsvkrtByomeis.Add((i, i1, validationByomei));
                        }
                    }

                    Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> validationOneOrdInf = new();
                    for (int i2 = 0; i2 < nextOrderModel.RsvkrtOrderInfs.Count; i2++)
                    {
                        var order = nextOrderModel.RsvkrtOrderInfs[i2];
                        var validationOrderInf = order.Validation(0);
                        if (validationOrderInf.Value != OrdInfValidationStatus.Valid)
                        {
                            validationOneOrdInf.Add(i2.ToString(), validationOrderInf);
                        }
                    }
                    if (validationOneOrdInf.Any())
                        validationOrdInfs.Add((i, validationOneOrdInf));
                }
                if (validationKarteInfs.Count > 0 || validationNextOrders.Count > 0 || validationRsvkrtByomeis.Count > 0 || validationOrdInfs.Count > 0)
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Failed, validationNextOrders, validationOrdInfs, validationKarteInfs, validationRsvkrtByomeis);
                }
                _nextOrderRepository.Upsert(inputData.UserId, inputData.HpId, inputData.PtId, nextOrderModels);
                return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Successed, new(), new(), new(), new());
            }
            catch
            {
                return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Failed, new(), new(), new(), new());
            }
        }

        private NextOrderModel ConvertNextOrderToModel(int hpId, long ptId, NextOrderItem nextOrderItem)
        {
            return new NextOrderModel(
                    hpId,
                    ptId,
                    nextOrderItem.RsvkrtNo,
                    nextOrderItem.RsvkrtKbn,
                    nextOrderItem.RsvDate,
                    nextOrderItem.RsvName,
                    nextOrderItem.IsDeleted,
                    nextOrderItem.SortNo,
                    nextOrderItem.rsvKrtByomeiItems.Select(b => ConvertRsvkrtByomeiToModel(hpId, ptId, b)).ToList(),
                    ConvertRsvkrtKarteInfToModel(hpId, ptId, nextOrderItem.rsvkrtKarteInf),
                    nextOrderItem.rsvKrtOrderInfItems.Select(o => ConvertRsvkrtOrderInfToModel(hpId, ptId, o)).ToList()
                );
        }

        private RsvkrtByomeiModel ConvertRsvkrtByomeiToModel(int hpId, long ptId, RsvKrtByomeiItem rsvkrtByomeiItem)

        {
            return new RsvkrtByomeiModel(
                    rsvkrtByomeiItem.Id,
                    hpId,
                    ptId,
                    rsvkrtByomeiItem.RsvkrtNo,
                    rsvkrtByomeiItem.SeqNo,
                    rsvkrtByomeiItem.ByomeiCd,
                    rsvkrtByomeiItem.Byomei,
                    rsvkrtByomeiItem.SyobyoKbn,
                    rsvkrtByomeiItem.SikkanKbn,
                    rsvkrtByomeiItem.NanbyoCd,
                    rsvkrtByomeiItem.HosokuCmt,
                    rsvkrtByomeiItem.IsNodspRece,
                    rsvkrtByomeiItem.IsNodspKarte,
                    rsvkrtByomeiItem.IsDeleted,
                    rsvkrtByomeiItem.PrefixSuffixList,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                );
        }

        private RsvkrtKarteInfModel ConvertRsvkrtKarteInfToModel(int hpId, long ptId, RsvKrtKarteInfItem rsvkrtKarteInfItem)

        {
            return new RsvkrtKarteInfModel(
                    hpId,
                    ptId,
                    rsvkrtKarteInfItem.RsvDate,
                    rsvkrtKarteInfItem.RsvkrtNo,
                    rsvkrtKarteInfItem.SeqNo,
                    rsvkrtKarteInfItem.Text,
                    rsvkrtKarteInfItem.RichText,
                    rsvkrtKarteInfItem.IsDeleted
                );
        }

        private RsvkrtOrderInfModel ConvertRsvkrtOrderInfToModel(int hpId, long ptId, RsvKrtOrderInfItem rsvkrtOrderInfItem)

        {
            return new RsvkrtOrderInfModel(
                    hpId,
                    ptId,
                    rsvkrtOrderInfItem.RsvDate,
                    rsvkrtOrderInfItem.RsvkrtNo,
                    rsvkrtOrderInfItem.RpNo,
                    rsvkrtOrderInfItem.RpEdaNo,
                    rsvkrtOrderInfItem.Id,
                    rsvkrtOrderInfItem.HokenPid,
                    rsvkrtOrderInfItem.OdrKouiKbn,
                    rsvkrtOrderInfItem.RpName,
                    rsvkrtOrderInfItem.InoutKbn,
                    rsvkrtOrderInfItem.SikyuKbn,
                    rsvkrtOrderInfItem.SyohoSbt,
                    rsvkrtOrderInfItem.SanteiKbn,
                    rsvkrtOrderInfItem.TosekiKbn,
                    rsvkrtOrderInfItem.DaysCnt,
                    rsvkrtOrderInfItem.IsDeleted,
                    rsvkrtOrderInfItem.SortNo,
                    DateTime.MinValue,
                    0,
                    string.Empty,
                    rsvkrtOrderInfItem.RsvKrtOrderInfDetailItems.Select(od => ConvertRsvkrtOrderDetailToModel(hpId, ptId, od)).ToList()
                );
        }

        private RsvKrtOrderInfDetailModel ConvertRsvkrtOrderDetailToModel(int hpId, long ptId, RsvKrtOrderInfDetailItem rsvkrtOrderInfDetailItem)

        {
            return new RsvKrtOrderInfDetailModel(
                    hpId,
                    ptId,
                    rsvkrtOrderInfDetailItem.RsvkrtNo,
                    rsvkrtOrderInfDetailItem.RpNo,
                    rsvkrtOrderInfDetailItem.RpEdaNo,
                    rsvkrtOrderInfDetailItem.RowNo,
                    rsvkrtOrderInfDetailItem.SinKouiKbn,
                    rsvkrtOrderInfDetailItem.SinKouiKbn,
                    rsvkrtOrderInfDetailItem.ItemCd,
                    rsvkrtOrderInfDetailItem.ItemName,
                    rsvkrtOrderInfDetailItem.Suryo,
                    rsvkrtOrderInfDetailItem.UnitName,
                    rsvkrtOrderInfDetailItem.UnitSbt,
                    rsvkrtOrderInfDetailItem.TermVal,
                    rsvkrtOrderInfDetailItem.KohatuKbn,
                    rsvkrtOrderInfDetailItem.SyohoKbn,
                    rsvkrtOrderInfDetailItem.SyohoLimitKbn,
                    rsvkrtOrderInfDetailItem.DrugKbn,
                    rsvkrtOrderInfDetailItem.YohoKbn,
                    rsvkrtOrderInfDetailItem.Kokuji1,
                    rsvkrtOrderInfDetailItem.Kokuji2,
                    rsvkrtOrderInfDetailItem.IsNodspRece,
                    rsvkrtOrderInfDetailItem.IpnCd,
                    rsvkrtOrderInfDetailItem.IpnName,
                    rsvkrtOrderInfDetailItem.Bunkatu,
                    rsvkrtOrderInfDetailItem.CmtName,
                    rsvkrtOrderInfDetailItem.CmtOpt,
                    rsvkrtOrderInfDetailItem.FontColor,
                    rsvkrtOrderInfDetailItem.CommentNewline,
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
                    string.Empty,
                    new(),
                    0,
                    0
                );
        }
    }
}
