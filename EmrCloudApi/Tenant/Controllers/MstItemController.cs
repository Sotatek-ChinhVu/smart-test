using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.MstItem;
using EmrCloudApi.Tenant.Requests.MstItem;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MstItem;
using EmrCloudApi.Tenant.Responses.MstItem.DiseaseSearch;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MstItem.DiseaseSearch;
using UseCase.MstItem.FindTenMst;
using UseCase.MstItem.GetDosageDrugList;
using UseCase.MstItem.GetFoodAlrgy;
using UseCase.MstItem.SearchOTC;
using UseCase.MstItem.SearchPostCode;
using UseCase.MstItem.SearchSupplement;
using UseCase.MstItem.SearchTenItem;
using UseCase.MstItem.UpdateAdopted;
using UseCase.MstItem.UpdateAdoptedByomei;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MstItemController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;

        public MstItemController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpPost(ApiPath.GetDosageDrugList)]
        public ActionResult<Response<GetDosageDrugListResponse>> GetDosageDrugList([FromBody] GetDosageDrugListRequest request)
        {
            var input = new GetDosageDrugListInputData(request.YjCds);
            var output = _bus.Handle(input);

            var presenter = new GetDosageDrugListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDosageDrugListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetFoodAlrgy)]
        public ActionResult<Response<GetFoodAlrgyMasterDataResponse>> GetFoodAlrgy()
        {
            var input = new GetFoodAlrgyInputData();
            var output = _bus.Handle(input);

            var presenter = new FoodAlrgyMasterDataPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetFoodAlrgyMasterDataResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchOTC)]
        public ActionResult<Response<SearchOTCResponse>> SearchOTC([FromBody] SearchOTCRequest request)
        {
            var input = new SearchOTCInputData(request.SearchValue, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new SearchOTCPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchOTCResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchSupplement)]
        public ActionResult<Response<SearchSupplementResponse>> SearchSupplement([FromBody] SearchSupplementRequest request)
        {
            var input = new SearchSupplementInputData(request.SearchValue, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new SearchSupplementPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchSupplementResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchTenItem)]
        public ActionResult<Response<SearchTenItemResponse>> SearchTenItem([FromBody] SearchTenItemRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new SearchTenItemInputData(request.Keyword, request.KouiKbn, request.SinDate, request.PageIndex, request.PageCount, request.GenericOrSameItem, request.YJCd, hpId, request.PointFrom, request.PointTo, request.IsRosai, request.IsMirai, request.IsExpired, request.ItemCodeStartWith);
            var output = _bus.Handle(input);
            var presenter = new SearchTenItemPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateAdoptedInputItem)]
        public ActionResult<Response<UpdateAdoptedTenItemResponse>> UpdateAdoptedInputItem([FromBody] UpdateAdoptedTenItemRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            int userId = _userService.GetLoginUser().UserId;
            var input = new UpdateAdoptedTenItemInputData(request.ValueAdopted, request.ItemCdInputItem, request.StartDateInputItem, hpId, userId);
            var output = _bus.Handle(input);
            var presenter = new UpdateAdoptedTenItemPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.DiseaseSearch)]
        public ActionResult<Response<DiseaseSearchResponse>> DiseaseSearch([FromQuery] DiseaseSearchRequest request)
        {
            var input = new DiseaseSearchInputData(request.IsPrefix, request.IsByomei, request.IsSuffix, request.IsMisaiyou, request.Sindate, request.Keyword, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new DiseaseSearchPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<DiseaseSearchResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateAdoptedByomei)]
        public ActionResult<Response<UpdateAdoptedTenItemResponse>> UpdateAdoptedByomei([FromBody] UpdateAdoptedByomeiRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            int userId = _userService.GetLoginUser().UserId;
            var input = new UpdateAdoptedByomeiInputData(hpId, request.ByomeiCd, userId);
            var output = _bus.Handle(input);
            var presenter = new UpdateAdoptedByomeiPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.SearchPostCode)]
        public ActionResult<Response<SearchPostCodeRespone>> GetList([FromQuery] SearchPostCodeRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new SearchPostCodeInputData(hpId, request.PostCode1, request.PostCode2, request.Address, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);
            var presenter = new SearchPostCodePresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.FindTenMst)]
        public ActionResult<Response<FindtenMstResponse>> FindTenMst([FromQuery] FindTenMstRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new FindTenMstInputData(hpId, request.SinDate, request.ItemCd);
            var output = _bus.Handle(input);
            var presenter = new FindTenMstPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
