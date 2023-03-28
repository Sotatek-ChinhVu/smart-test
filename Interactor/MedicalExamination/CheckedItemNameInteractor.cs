using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.CheckedItemName;

namespace Interactor.MedicalExamination
{
    public class CheckedItemNameInteractor : ICheckedItemNameInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;

        public CheckedItemNameInteractor(ITodayOdrRepository todayOdrRepository)
        {
            _todayOdrRepository = todayOdrRepository;
        }

        public CheckedItemNameOutputData Handle(CheckedItemNameInputData inputData)
        {
            try
            {
                if (inputData.OdrInfs.Count == 0)
                {
                    return new CheckedItemNameOutputData(CheckedItemNameStatus.InputNotData, new());
                }
                var checkedName = _todayOdrRepository.CheckNameChanged(inputData.OdrInfs.Select(item =>
                    new OrdInfModel(
                        item.HpId,
                        item.RaiinNo,
                        item.RpNo,
                        item.RpEdaNo,
                        item.PtId,
                        item.SinDate,
                        item.HokenPid,
                        item.OdrKouiKbn,
                        item.RpName,
                        item.InoutKbn,
                        item.SikyuKbn,
                        item.SyohoSbt,
                        item.SanteiKbn,
                        item.TosekiKbn,
                        item.DaysCnt,
                        item.SortNo,
                        item.IsDeleted,
                        item.Id,
                        item.OdrDetails.Select(itemDetail => new OrdInfDetailModel(
                                itemDetail.HpId,
                                itemDetail.RaiinNo,
                                itemDetail.RpNo,
                                itemDetail.RpEdaNo,
                                itemDetail.RowNo,
                                itemDetail.PtId,
                                itemDetail.SinDate,
                                itemDetail.SinKouiKbn,
                                itemDetail.ItemCd,
                                itemDetail.ItemName,
                                itemDetail.Suryo,
                                itemDetail.UnitName,
                                itemDetail.UnitSbt,
                                itemDetail.TermVal,
                                itemDetail.KohatuKbn,
                                itemDetail.SyohoKbn,
                                itemDetail.SyohoLimitKbn,
                                itemDetail.DrugKbn,
                                itemDetail.YohoKbn,
                                itemDetail.Kokuji1,
                                itemDetail.Kokuji2,
                                itemDetail.IsNodspRece,
                                itemDetail.IpnCd,
                                string.Empty,
                                itemDetail.JissiKbn,
                                itemDetail.JissiDate,
                                itemDetail.JissiId,
                                itemDetail.JissiMachine,
                                itemDetail.ReqCd,
                                itemDetail.Bunkatu,
                                itemDetail.CmtName,
                                itemDetail.CmtOpt,
                                itemDetail.FontColor,
                                itemDetail.CommentNewline,
                                string.Empty,
                                item?.InoutKbn ?? 0,
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
                        "",
                        false
                    )).ToList());

                return new CheckedItemNameOutputData(CheckedItemNameStatus.Successed, checkedName);
            }
            catch
            {
                return new CheckedItemNameOutputData(CheckedItemNameStatus.Failed, new());
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
            }
        }
    }
}
