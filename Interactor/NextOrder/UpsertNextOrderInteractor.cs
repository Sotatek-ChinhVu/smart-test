using Domain.Models.NextOrder;
using UseCase.NextOrder.Upsert;
using static Helper.Constants.KarteConst;
using static Helper.Constants.NextOrderConst;
using static Helper.Constants.OrderInfConst;
using static Helper.Constants.RsvkrtByomeiConst;

namespace Interactor.NextOrder
{
    public class UpsertNextOrderInteractor : IUpsertNextOrderInputPort
    {
        private readonly INextOrderRepository _nextOrderRepository;

        public UpsertNextOrderInteractor(INextOrderRepository nextOrderRepository)
        {
            _nextOrderRepository = nextOrderRepository;
        }

        public UpsertNextOrderOutputData Handle(UpsertNextOrderInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new UpsertNextOrderOutputData(UpsertNextOrderStatus.InvalidHpId, new(), new(), new());
                }
                if (inputData.PtId <= 0)
                {
                    return new UpsertNextOrderOutputData(UpsertNextOrderStatus.InvalidPtId, new(), new(), new());
                }
                if (inputData.UserId <= 0)
                {
                    return new UpsertNextOrderOutputData(UpsertNextOrderStatus.InvalidUserId, new(), new(), new());
                }

                var nextOrderModels = inputData.NextOrderItems.Select(n => ConvertNextOrderToModel(inputData.HpId, inputData.PtId, n)).ToList();
                List<(int, KarteValidationStatus)> validationKarteInfs = new();
                Dictionary<int, NextOrderStatus> validationNextOrders = new();
                List<(int, int, RsvkrtByomeiStatus)> validationRsvkrtByomeis = new();
                var validationOrdInf = new List<(int, Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>)>();

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
                    validationOrdInf.Add((i, validationOneOrdInf));
                }
                if (validationKarteInfs.Count > 0 || validationNextOrders.Count > 0 || validationRsvkrtByomeis.Count > 0 || validationOrdInf.Count > 0)
                {
                    return new UpsertNextOrderOutputData(UpsertNextOrderStatus.Failed, validationNextOrders, validationOrdInf, validationKarteInfs);
                }
                _nextOrderRepository.Upsert(inputData.UserId, inputData.HpId, inputData.PtId, nextOrderModels);
                return new UpsertNextOrderOutputData(UpsertNextOrderStatus.Successed, new(), new(), new());
            }
            catch
            {
                return new UpsertNextOrderOutputData(UpsertNextOrderStatus.Failed, new(), new(), new());
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
