using Domain.Models.Insurance;
using Domain.Models.NextOrder;
using UseCase.NextOrder;
using static Helper.Constants.OrderInfConst;

namespace Interactor.NextOrder
{
    public static class NextOrderCommon
    {
        public static Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> CheckValidateOrderInf(int hpId, long ptId, List<string> itemCds, List<string> ipnCds, NextOrderModel nextOrderModel, List<RsvkrtOrderInfModel> checkOderInfs, List<HokenInfModel> checkHokens)
        {
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

            for (int index = 0; index < nextOrderModel.RsvkrtOrderInfs.Count; index++)
            {
                var item = nextOrderModel.RsvkrtOrderInfs[index];

                if (item.Id > 0)
                {
                    var check = checkOderInfs.Any(c => c.RsvkrtNo == item.RsvkrtNo && c.RsvDate == item.RsvDate && c.RpNo == item.RpNo && c.RpEdaNo == item.RpEdaNo);
                    if (!check)
                    {
                        AddErrorStatus(validationOneOrdInf, index.ToString(), new("-1", OrdInfValidationStatus.InvalidTodayOrdUpdatedNoExist));
                        break;
                    }
                }

                var checkObjs = nextOrderModel.RsvkrtOrderInfs.Where(o => item.Id > 0 && o.RpNo == item.RpNo).ToList();
                var positionOrd = nextOrderModel.RsvkrtOrderInfs.FindIndex(o => o == checkObjs.LastOrDefault());
                if (checkObjs.Count >= 2 && positionOrd == index)
                {
                    AddErrorStatus(validationOneOrdInf, positionOrd.ToString(), new("-1", OrdInfValidationStatus.DuplicateTodayOrd));
                    break;
                }

                var checkHokenPid = checkHokens.Any(h => h.HokenId == item.HokenPid);
                if (!checkHokenPid)
                {
                    AddErrorStatus(validationOneOrdInf, index.ToString(), new("-1", OrdInfValidationStatus.HokenPidNoExist));

                    break;
                }

                var odrDetail = item.OrdInfDetails.FirstOrDefault(itemOd => item.RpNo != itemOd.RpNo || item.RpEdaNo != itemOd.RpEdaNo || item.RsvDate != itemOd.RsvDate || item.RsvkrtNo != itemOd.RsvkrtNo);
                if (odrDetail != null)
                {
                    var indexOdrDetail = item.OrdInfDetails.IndexOf(odrDetail);
                    AddErrorStatus(validationOneOrdInf, index.ToString(), new(indexOdrDetail.ToString(), OrdInfValidationStatus.OdrNoMapOdrDetail));
                    break;
                }

                odrDetail = item.OrdInfDetails.FirstOrDefault(od => !itemCds.Contains(od.ItemCd));
                if (odrDetail != null)
                {
                    var indexOdrDetail = item.OrdInfDetails.IndexOf(odrDetail);
                    AddErrorStatus(validationOneOrdInf, index.ToString(), new(indexOdrDetail.ToString(), OrdInfValidationStatus.InvalidItemCd));
                    break;
                }

                odrDetail = item.OrdInfDetails.FirstOrDefault(od => !ipnCds.Contains(od.IpnCd));
                if (odrDetail != null)
                {
                    var indexOdrDetail = item.OrdInfDetails.IndexOf(odrDetail);
                    AddErrorStatus(validationOneOrdInf, index.ToString(), new(indexOdrDetail.ToString(), OrdInfValidationStatus.InvalidIpnCd));
                    break;
                }
            }

            return validationOneOrdInf;
        }

        public static NextOrderModel ConvertNextOrderToModel(int hpId, long ptId, List<Tuple<string, string>> ipnCds, NextOrderItem nextOrderItem)
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
                    nextOrderItem.rsvKrtOrderInfItems.Select(o => ConvertRsvkrtOrderInfToModel(hpId, ptId, ipnCds, o)).ToList()
                );
        }

        public static RsvkrtByomeiModel ConvertRsvkrtByomeiToModel(int hpId, long ptId, RsvKrtByomeiItem rsvkrtByomeiItem)

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

        public static RsvkrtKarteInfModel ConvertRsvkrtKarteInfToModel(int hpId, long ptId, RsvKrtKarteInfItem rsvkrtKarteInfItem)

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

        public static RsvkrtOrderInfModel ConvertRsvkrtOrderInfToModel(int hpId, long ptId, List<Tuple<string, string>> ipnCds, RsvKrtOrderInfItem rsvkrtOrderInfItem)
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
                    rsvkrtOrderInfItem.RsvKrtOrderInfDetailItems.Select(od => ConvertRsvkrtOrderDetailToModel(hpId, ptId, ipnCds, od)).ToList()
                );
        }

        public static RsvKrtOrderInfDetailModel ConvertRsvkrtOrderDetailToModel(int hpId, long ptId, List<Tuple<string, string>> ipnCds, RsvKrtOrderInfDetailItem rsvkrtOrderInfDetailItem)

        {
            return new RsvKrtOrderInfDetailModel(
                    hpId,
                    ptId,
                    rsvkrtOrderInfDetailItem.RsvkrtNo,
                    rsvkrtOrderInfDetailItem.RpNo,
                    rsvkrtOrderInfDetailItem.RpEdaNo,
                    rsvkrtOrderInfDetailItem.RowNo,
                    rsvkrtOrderInfDetailItem.RsvDate,
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
                    ipnCds.FirstOrDefault(ipn => ipn.Item1 == rsvkrtOrderInfDetailItem.IpnCd)?.Item2 ?? string.Empty,
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

        private static void AddErrorStatus(Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> dicValidation, string key, KeyValuePair<string, OrdInfValidationStatus> status)
        {
            dicValidation.Add(key, status);
        }
    }
}
