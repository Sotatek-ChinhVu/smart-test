using Domain.Models.DrugInfor;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.DrugInfor;
using EmrCloudApi.Requests.DrugInfor;
using EmrCloudApi.Requests.DrugInfor.SaveSinrekiFilterMstListRequestItem;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.DrugInfor;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.DrugInfor.GetContentDrugUsageHistory;
using UseCase.DrugInfor.GetSinrekiFilterMstList;
using UseCase.DrugInfor.SaveSinrekiFilterMstList;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class PrescriptionHistoryController : AuthorizeControllerBase
{

    private readonly UseCaseBus _bus;
    public PrescriptionHistoryController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetSinrekiFilterMstList)]
    public ActionResult<Response<GetSinrekiFilterMstListResponse>> GetList()
    {
        var input = new GetSinrekiFilterMstListInputData(HpId);
        var output = _bus.Handle(input);

        var presenter = new GetSinrekiFilterMstListPresenter();
        presenter.Complete(output);

        var result = Ok(presenter.Result);
        return result;
    }

    [HttpGet(ApiPath.GetContentDrugUsageHistory)]
    public ActionResult<Response<GetContentDrugUsageHistoryResponse>> GetContentDrugUsageHistory([FromQuery] GetContentDrugUsageHistoryRequest request)
    {
        var input = new GetContentDrugUsageHistoryInputData(HpId, UserId, request.PtId, request.GrpId, request.StartDate, request.EndDate);
        var output = _bus.Handle(input);

        var presenter = new GetContentDrugUsageHistoryPresenter();
        presenter.Complete(output);

        var result = Ok(presenter.Result);
        return result;
    }

    [HttpPost(ApiPath.SaveSinrekiFilterMstList)]
    public ActionResult<Response<SaveSinrekiFilterMstListResponse>> SaveSinrekiFilterMstList([FromBody] SaveSinrekiFilterMstListRequest request)
    {
        var input = new SaveSinrekiFilterMstListInputData(HpId, UserId, ConvertToSinrekiFilterMstList(request.SinrekiFilterMstList));
        var output = _bus.Handle(input);

        var presenter = new SaveSinrekiFilterMstListPresenter();
        presenter.Complete(output);

        var result = Ok(presenter.Result);
        return result;
    }

    #region private function
    private List<SinrekiFilterMstModel> ConvertToSinrekiFilterMstList(List<SinrekiFilterMstRequestItem> sinrekiFilterMstList)
    {
        List<SinrekiFilterMstModel> result = new();
        foreach (var mstRequest in sinrekiFilterMstList)
        {
            List<SinrekiFilterMstDetailModel> detailModelList = mstRequest.SinrekiFilterMstDetailList
                                                                          .Select(item => new SinrekiFilterMstDetailModel(
                                                                                              item.Id,
                                                                                              mstRequest.GrpCd,
                                                                                              item.ItemCd,
                                                                                              string.Empty,
                                                                                              item.SortNo,
                                                                                              item.IsExclude,
                                                                                              item.IsDeleted
                                                                           )).ToList();
            List<SinrekiFilterMstKouiModel> kouiModelList = mstRequest.SinrekiFilterMstKouiList
                                                                      .Select(item => new SinrekiFilterMstKouiModel(
                                                                                          mstRequest.GrpCd,
                                                                                          item.SeqNo,
                                                                                          item.KouiKbnId,
                                                                                          item.IsChecked
                                                                      )).ToList();
            var mstModel = new SinrekiFilterMstModel(
                               mstRequest.GrpCd,
                               mstRequest.Name,
                               mstRequest.SortNo,
                               mstRequest.IsDeleted,
                               kouiModelList,
                               detailModelList);
            result.Add(mstModel);
        }
        return result;
    }

    #endregion
}
