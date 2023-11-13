﻿using Domain.Models.SuperSetDetail;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Text;
using UseCase.SuperSetDetail.GetSuperSetDetail;
using UseCase.SuperSetDetail.SuperSetDetail;

namespace Interactor.SuperSetDetail;

public class GetSuperSetDetailInteractor : IGetSuperSetDetailInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;
    private readonly IAmazonS3Service _amazonS3Service;
    private readonly AmazonS3Options _options;

    public GetSuperSetDetailInteractor(IOptions<AmazonS3Options> optionsAccessor, ISuperSetDetailRepository superSetDetailRepository, IAmazonS3Service amazonS3Service)
    {
        _superSetDetailRepository = superSetDetailRepository;
        _amazonS3Service = amazonS3Service;
        _options = optionsAccessor.Value;
    }

    public GetSuperSetDetailOutputData Handle(GetSuperSetDetailInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.InvalidHpId);
            }
            if (inputData.UserId <= 0)
            {
                return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.InvalidUserId);
            }
            else if (inputData.SetCd <= 0)
            {
                return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.InvalidSetCd);
            }
            else if (inputData.Sindate <= 0)
            {
                return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.InvalidSindate);
            }
            var result = _superSetDetailRepository.GetSuperSetDetail(inputData.HpId, inputData.UserId, inputData.SetCd, inputData.Sindate);
            return new GetSuperSetDetailOutputData(ConvertSuperSetDetailToItem(inputData.SetCd, result), GetSuperSetDetailListStatus.Successed);
        }
        catch
        {
            return new GetSuperSetDetailOutputData(GetSuperSetDetailListStatus.Failed);
        }
        finally
        {
            _superSetDetailRepository.ReleaseResource();
        }
    }

    private List<SetByomeiItem> ConvertSetByomeiToItem(List<SetByomeiModel> setByomeiModels)
    {
        return setByomeiModels.Select(s => new SetByomeiItem(
                s.Id,
                s.IsSyobyoKbn,
                s.SikkanKbn,
                s.NanByoCd,
                s.FullByomei,
                s.IsSuspected,
                s.IsDspRece,
                s.IsDspKarte,
                s.ByomeiCmt,
                s.ByomeiCd,
                s.PrefixSuffixList,
                s.Icd10,
                s.Icd102013,
                s.Icd1012013,
                s.Icd1022013
            )).ToList();
    }

    private List<SetOrderInfItem> ConvertSetOrderInfToItem(List<SetOrderInfModel> setOrderInfModels)
    {
        return setOrderInfModels.Select(o => new SetOrderInfItem(
                o.Id,
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
                o.SortNo,
                o.IsDeleted,
                o.CreateDate,
                o.CreateId,
                o.CreateName,
                o.GroupKoui.Value,
                o.IsSelfInjection,
                o.IsDrug,
                o.IsInjection,
                o.IsKensa,
                o.IsShohoComment,
                o.IsShohoBiko,
                o.IsShohosenComment,
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
                         od.Price,
                         od.IsSpecialItem,
                         od.IsYoho,
                         od.IsKensa,
                         od.Is820Cmt,
                         od.Is830Cmt,
                         od.Is831Cmt,
                         od.Is850Cmt,
                         od.Is851Cmt,
                         od.Is852Cmt,
                         od.Is840Cmt,
                         od.Is842Cmt,
                         od.IsShohoComment,
                         od.IsShohoBiko,
                         od.IsDrug,
                         od.IsInjection,
                         od.IsDrugUsage,
                         od.IsStandardUsage,
                         od.IsSuppUsage,
                         od.IsInjectionUsage,
                         od.IsNormalComment,
                         od.IsComment,
                         od.IsUsage,
                         od.DisplayedQuantity,
                         od.EditingQuantity,
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
                    )
                ).ToList()
            )).ToList();
    }

    private List<SetGroupOrderInfItem> ConvertSetGroupOrderInfToItem(List<SetGroupOrderInfModel> setGroupOrderInfModels)
    {
        return setGroupOrderInfModels.Select(s => new SetGroupOrderInfItem(
             s.GUID,
             s.KouiCode,
             s.GroupKouiCode.Value,
             s.GroupName,
             s.IsShowInOut,
             s.InOutKbn,
             s.InOutName,
             s.IsShowSikyu,
             s.SikyuKbn,
             s.TosekiKbn,
             s.SikyuName,
             s.IsShowSantei,
             s.SanteiKbn,
             s.SanteiName,
             s.SyohoSbt,
             s.IsDrug,
             s.IsKensa,
             ConvertSetOrderInfToItem(s.ListSetOrdInfModels)
         )).ToList();
    }

    private List<SetFileInfModel> ConvertToListSetKarteFileItem(int setCd, List<SetFileInfModel> listModel)
    {
        List<SetFileInfModel> result = new();
        if (listModel.Any())
        {
            List<string> listFolders = new()
            {
                CommonConstants.Store,
                CommonConstants.Karte,
                CommonConstants.SetPic,
                setCd.ToString()
            };

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

    private SuperSetDetailItem ConvertSuperSetDetailToItem(int setCd, SuperSetDetailModel superSetDetailModel)
    {
        return new SuperSetDetailItem(
                                        ConvertSetByomeiToItem(superSetDetailModel.SetByomeiList),
                                        superSetDetailModel.SetKarteInf,
                                        ConvertSetGroupOrderInfToItem(superSetDetailModel.SetGroupOrderInfList),
                                        ConvertToListSetKarteFileItem(setCd, superSetDetailModel.SetKarteFileModelList));
    }
}
