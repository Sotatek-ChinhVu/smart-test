using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.MstItem;
using EmrCloudApi.Tenant.Requests.MstItem;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MstItem;
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
        public async Task<ActionResult<Response<GetDosageDrugListResponse>>> GetDosageDrugList([FromBody] GetDosageDrugListRequest request)
        {
            var input = new GetDosageDrugListInputData(request.YjCds);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetDosageDrugListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDosageDrugListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetFoodAlrgy)]
        public async Task<ActionResult<Response<GetFoodAlrgyMasterDataResponse>>> GetFoodAlrgy()
        {
            var input = new GetFoodAlrgyInputData();
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new FoodAlrgyMasterDataPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetFoodAlrgyMasterDataResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchOTC)]
        public async Task<ActionResult<Response<SearchOTCResponse>>> SearchOTC([FromBody] SearchOTCRequest request)
        {
            var input = new SearchOTCInputData(request.SearchValue, request.PageIndex, request.PageSize);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SearchOTCPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchOTCResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchSupplement)]
        public async Task<ActionResult<Response<SearchSupplementResponse>>> SearchSupplement([FromBody] SearchSupplementRequest request)
        {
            var input = new SearchSupplementInputData(request.SearchValue, request.PageIndex, request.PageSize);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SearchSupplementPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchSupplementResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchTenItem)]
        public async Task<ActionResult<Response<SearchTenItemResponse>>> SearchTenItem([FromBody] SearchTenItemRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SearchTenItemResponse>>(new Response<SearchTenItemResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SearchTenItemInputData(request.Keyword, request.KouiKbn, request.SinDate, request.PageIndex, request.PageCount, request.GenericOrSameItem, request.YJCd, hpId, request.PointFrom, request.PointTo, request.IsRosai, request.IsMirai, request.IsExpired, request.ItemCodeStartWith);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new SearchTenItemPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateAdoptedInputItem)]
        public async Task<ActionResult<Response<UpdateAdoptedTenItemResponse>>> UpdateAdoptedInputItem([FromBody] UpdateAdoptedTenItemRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<UpdateAdoptedTenItemResponse>>(new Response<UpdateAdoptedTenItemResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<UpdateAdoptedTenItemResponse>>(new Response<UpdateAdoptedTenItemResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new UpdateAdoptedTenItemInputData(request.ValueAdopted, request.ItemCdInputItem, request.StartDateInputItem, hpId, userId);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new UpdateAdoptedTenItemPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.DiseaseSearch)]
        public async Task<ActionResult<Response<DiseaseSearchResponse>>> DiseaseSearch([FromQuery] DiseaseSearchRequest request)
        {
            var input = new DiseaseSearchInputData(request.IsPrefix, request.IsByomei, request.IsSuffix, request.Keyword, request.PageIndex, request.PageCount);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new DiseaseSearchPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<DiseaseSearchResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateAdoptedByomei)]
        public async Task<ActionResult<Response<UpdateAdoptedTenItemResponse>>> UpdateAdoptedByomei([FromBody] UpdateAdoptedByomeiRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<UpdateAdoptedTenItemResponse>>(new Response<UpdateAdoptedTenItemResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<UpdateAdoptedTenItemResponse>>(new Response<UpdateAdoptedTenItemResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new UpdateAdoptedByomeiInputData(hpId, request.ByomeiCd, userId);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new UpdateAdoptedByomeiPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.SearchPostCode)]
        public async Task<ActionResult<Response<SearchPostCodeRespone>>> GetList([FromQuery] SearchPostCodeRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SearchPostCodeRespone>>(new Response<SearchPostCodeRespone> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SearchPostCodeInputData(hpId, request.PostCode1, request.PostCode2, request.Address, request.PageIndex, request.PageSize);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new SearchPostCodePresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.FindTenMst)]
        public async Task<ActionResult<Response<FindtenMstResponse>>> FindTenMst([FromQuery] FindTenMstRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<FindtenMstResponse>>(new Response<FindtenMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new FindTenMstInputData(hpId, request.SinDate, request.ItemCd);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new FindTenMstPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
