using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.ReceSeikyu;
using EmrCloudApi.Requests.ReceSeikyu;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReceSeikyu;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.ReceSeikyu.GetList;
using UseCase.ReceSeikyu.SearchReceInf;
using UseCase.ReceSeikyu.Save;
using System.Text;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class ReceSeikyuController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly ICalcultateCustomerService _calcultateCustomerService;

        public ReceSeikyuController(UseCaseBus bus, IUserService userService, ICalcultateCustomerService calcultateCustomerService) : base(userService)
        {
            _bus = bus;
            _calcultateCustomerService = calcultateCustomerService;
        }

        [HttpGet(ApiPath.GetListReceSeikyu)]
        public ActionResult<Response<GetListReceSeikyuResponse>> GetListReceSeikyu([FromQuery] GetListReceSeikyuRequest request)
        {
            var input = new GetListReceSeikyuInputData(HpId, 
                                                       request.SinDate,
                                                       request.SinYm,
                                                       request.IsIncludingUnConfirmed,
                                                       request.PtNumSearch,
                                                       request.NoFilter,
                                                       request.IsFilterMonthlyDelay,
                                                       request.IsFilterReturn,
                                                       request.IsFilterOnlineReturn);
            var output = _bus.Handle(input);
            var presenter = new GetListReceSeikyuPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListReceSeikyuResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.SearchReceInf)]
        public ActionResult<Response<SearchReceInfResponse>> SearchReceInf([FromQuery] SearchReceInfRequest request)
        {
            var input = new SearchReceInfInputData(HpId, request.PtNum, request.SinYm);
            var output = _bus.Handle(input);
            var presenter = new SearchReceInfPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SearchReceInfResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveReceSeikyu)]
        public ActionResult<Response<SaveReceSeiKyuResponse>> SaveReceSeikyu([FromBody] SaveReceSeiKyuRequest request)
        {
            var input = new SaveReceSeiKyuInputData(request.Data, request.SinYm, HpId, UserId);
            var output = _bus.Handle(input);
            if (output.Status == SaveReceSeiKyuStatus.Successful && output.PtIds.Any() && output.SeikyuYm != 0)
            {
                //Call httpClient 
                _calcultateCustomerService.RunCaculationPostAsync<string>(TypeCalculate.ReceFutanCalculateMain, new
                {
                    PtIds = output.PtIds,
                    SeikyuYm = output.SeikyuYm
                }).Wait();
            }

            
            var presenter = new SaveReceSeiKyuPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveReceSeiKyuResponse>>(presenter.Result);
        }


        private void AddMessageCheckErrorInMonth(string displayText , int percent)
        {
            StringBuilder titleProgressbar = new();
            titleProgressbar.Append("\n{ displayText: \"");
            titleProgressbar.Append(displayText);
            titleProgressbar.Append("\", percent: ");
            titleProgressbar.Append(percent);
            titleProgressbar.Append("\" }");
            var resultForFrontEnd = Encoding.UTF8.GetBytes(titleProgressbar.ToString());
            HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
            HttpContext.Response.Body.FlushAsync();
        }
    }
}
