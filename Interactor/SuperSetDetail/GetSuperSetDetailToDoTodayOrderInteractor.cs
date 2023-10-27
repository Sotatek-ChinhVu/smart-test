using Domain.Models.SuperSetDetail;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text;
using UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;

namespace Interactor.SuperSetDetail;

public class GetSuperSetDetailToDoTodayOrderInteractor : IGetSuperSetDetailToDoTodayOrderInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;
    private readonly AmazonS3Options _options;

    public GetSuperSetDetailToDoTodayOrderInteractor(IOptions<AmazonS3Options> optionsAccessor, ITenantProvider tenantProvider, ISuperSetDetailRepository superSetDetailRepository, IAmazonS3Service amazonS3Service)
    {
        _superSetDetailRepository = superSetDetailRepository;
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public GetSuperSetDetailToDoTodayOrderOutputData Handle(GetSuperSetDetailToDoTodayOrderInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.InvalidHpId);
            }
            if (inputData.UserId <= 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.InvalidUserId);
            }
            else if (inputData.SetCd <= 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.InvalidSetCd);
            }
            else if (inputData.SinDate <= 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.InvalidSinDate);
            }

            var result = _superSetDetailRepository.GetSuperSetDetailForTodayOrder(inputData.HpId, inputData.UserId, inputData.SetCd, inputData.SinDate);
            if (result.Item1.Count == 0 && result.Item2.Count == 0 && result.Item3.Count == 0)
            {
                return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.NoData);
            }

            var setFiles = new List<SetFileInfModel>();
            foreach (var setFileInfModel in result.setFileInfModels)
            {
                setFiles.AddRange(ConvertToListSetKarteFileItem(setFileInfModel.setCd, setFileInfModel.setFiles).ToList());
            }

            return new GetSuperSetDetailToDoTodayOrderOutputData(ConvertSetByomeiToItem(result.byomeis ?? new()), result.karteInfs, ConvertSetOrderInfToItem(result.orderInfModels), setFiles, GetSuperSetDetailToDoTodayOrderStatus.Successed);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            return new GetSuperSetDetailToDoTodayOrderOutputData(new(), new(), new(), new(), GetSuperSetDetailToDoTodayOrderStatus.Failed);
        }
        finally
        {
            _superSetDetailRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }

    private List<SetByomeiItem> ConvertSetByomeiToItem(List<SetByomeiModel> setByomeiModels)
    {
        return setByomeiModels.Select(s => new SetByomeiItem(
                s.IsSyobyoKbn,
                s.SikkanKbn,
                s.NanByoCd,
                s.FullByomei,
                s.IsSuspected,
                s.IsDspRece,
                s.IsDspKarte,
                s.ByomeiCmt,
                s.ByomeiCd,
                s.Icd10,
                s.Icd102013,
                s.Icd1012013,
                s.Icd1022013,
                s.PrefixSuffixList
            )).ToList();
    }

    private List<SetOrderInfItem> ConvertSetOrderInfToItem(List<SetOrderInfModel> setOrderInfModels)
    {
        return setOrderInfModels.Select(o => new SetOrderInfItem(
                o.HpId,
                o.SetCd,
                o.RpNo,
                o.RpEdaNo,
                o.OdrKouiKbn,
                o.RpName,
                o.InoutKbn,
                o.SikyuKbn,
                o.SyohoSbt,
                o.SanteiKbn,
                o.TosekiKbn,
                o.DaysCnt,
                o.GroupKoui.Value,
                o.OrdInfDetails.Select(
                    od => new SetOrderInfDetailItem(
                         od.HpId,
                         od.SetCd,
                         od.RpNo,
                         od.RpEdaNo,
                         od.RowNo,
                         od.SinKouiKbn,
                         od.ItemCd,
                         od.ItemName,
                         od.DisplayItemName,
                         od.Suryo,
                         od.UnitName,
                         od.UnitSBT,
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
                         od.IpnName,
                         od.Bunkatu,
                         od.CmtName,
                         od.CmtOpt,
                         od.FontColor,
                         od.CommentNewline,
                         od.MasterSbt,
                         od.InOutKbn,
                         od.Yakka,
                         od.IsGetPriceInYakka,
                         od.Ten,
                         od.BunkatuKoui,
                         od.KensaGaichu,
                         od.OdrTermVal,
                         od.CnvTermVal,
                         od.YjCd,
                         od.CenterItemCd1,
                         od.CenterItemCd2,
                         od.Kasan1,
                         od.Kasan2,
                         od.YohoSets,
                         od.CmtColKeta1,
                         od.CmtColKeta2,
                         od.CmtColKeta3,
                         od.CmtColKeta4,
                         od.CmtCol1,
                         od.CmtCol2,
                         od.CmtCol3,
                         od.CmtCol4,
                         od.HandanGrpKbn,
                         od.IsKensaMstEmpty
                )).ToList(),
                o.CreateDate,
                o.CreateName
            )).ToList();
    }

    private List<SetFileInfModel> ConvertToListSetKarteFileItem(int setCd, List<SetFileInfModel> listModel)
    {
        List<SetFileInfModel> result = new();
        if (listModel.Any())
        {
            List<string> listFolders = new();
            listFolders.Add(CommonConstants.Store);
            listFolders.Add(CommonConstants.Karte);
            listFolders.Add(CommonConstants.SetPic);
            listFolders.Add(setCd.ToString());

            string path = _amazonS3Service.GetFolderUploadOther(listFolders);
            foreach (var model in listModel)
            {
                var fileName = new StringBuilder();
                fileName.Append(_options.BaseAccessUrl);
                fileName.Append("/");
                fileName.Append(path);
                fileName.Append(model.LinkFile);
                result.Add(new SetFileInfModel(model.IsSchema, fileName.ToString()));
            }
        }
        return result;
    }
}
