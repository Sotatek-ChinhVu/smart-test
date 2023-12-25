using Domain.SuperAdminModels.Tenant;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Interfaces;
using Interactor.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using SuperAdmin.Responses;
using SuperAdminAPI.Presenters.Tenant;
using SuperAdminAPI.Reponse.Tenant;
using SuperAdminAPI.Request.Tennant;
using System.Text;
using UseCase.Core.Sync;
using UseCase.SuperAdmin.GetTenant;
using UseCase.SuperAdmin.GetTenantDetail;
using UseCase.SuperAdmin.RestoreObjectS3Tenant;
using UseCase.SuperAdmin.RestoreTenant;
using UseCase.SuperAdmin.StopedTenant;
using UseCase.SuperAdmin.TenantOnboard;
using UseCase.SuperAdmin.TerminateTenant;
using UseCase.SuperAdmin.UpdateDataTenant;
using UseCase.SuperAdmin.UpgradePremium;

namespace SuperAdminAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TenantController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IWebSocketService _webSocketService;
        private readonly IMessenger _messenger;
        private readonly IConfiguration _configuration;
        private string uniqueKey;
        private CancellationToken? _cancellationToken;
        private bool stopCalculate = false;
        public TenantController(UseCaseBus bus, IWebSocketService webSocketService, IMessenger messenger)
        {
            _bus = bus;
            _webSocketService = webSocketService;
            _messenger = messenger;
        }

        [HttpPost("UpdateTenant")]
        public ActionResult<Response<UpdateTenantResponse>> UpdateTenant([FromBody] UpdateTenantRequest request)
        {
            var input = new UpdateTenantInputData(request.TenantId, request.Size, request.SizeType, request.SubDomain, request.Type, request.Hospital, request.AdminId, request.Password, _webSocketService);
            var output = _bus.Handle(input);
            var presenter = new UpdateTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdateTenantResponse>>(presenter.Result);
        }

        [HttpPost("GetTenant")]
        public ActionResult<Response<GetTenantResponse>> GetTenant([FromBody] GetTenantRequest request)
        {
            var input = new GetTenantInputData(GetSearchTenantModel(request.SearchModel), request.SortDictionary, request.Skip, request.Take);
            var output = _bus.Handle(input);
            var presenter = new GetTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTenantResponse>>(presenter.Result);
        }

        [HttpGet("GetTenantDetail")]
        public ActionResult<Response<GetTenantDetailResponse>> GetTenantDetail([FromQuery] GetTenantDetailRequest request)
        {
            var input = new GetTenantDetailInputData(request.TenantId);
            var output = _bus.Handle(input);
            var presenter = new GetTenantDetailPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTenantDetailResponse>>(presenter.Result);
        }

        [HttpPost("TenantOnboard")]
        public ActionResult<Response<TenantOnboardResponse>> TenantOnboardAsync([FromBody] TenantOnboardRequest request)
        {
            var input = new TenantOnboardInputData(
                request.TenantId,
                request.Hospital,
                request.AdminId,
                request.Password,
                request.SubDomain,
                request.Size,
                request.SizeType,
                request.ClusterMode,
                _webSocketService);
            var output = _bus.Handle(input);
            var presenter = new TenantOnboardPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<TenantOnboardResponse>>(presenter.Result);
        }

        [HttpPost("TerminateTenant")]
        public ActionResult<Response<TerminateTenantResponse>> TerminateTenant([FromBody] TerminateTenantRequest request)
        {
            var input = new TerminateTenantInputData(request.TenantId, _webSocketService, request.Type);
            var output = _bus.Handle(input);
            var presenter = new TerminateTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<TerminateTenantResponse>>(presenter.Result);
        }

        #region 
        private SearchTenantModel GetSearchTenantModel(SearchTenantRequestItem requestItem)
        {
            return new SearchTenantModel(
                       requestItem.KeyWord,
                       requestItem.FromDate,
                       requestItem.ToDate,
                       requestItem.Type,
                       requestItem.StatusTenant,
                       requestItem.StorageFull);
        }
        #endregion

        [HttpPost("ToggleTenant")]
        public ActionResult<Response<ToggleTenantResponse>> ToggleTenant([FromBody] ToggleTenantRequest request)
        {
            var input = new ToggleTenantInputData(request.TenantId, _webSocketService, request.Type);
            var output = _bus.Handle(input);
            var presenter = new ToggleTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<ToggleTenantResponse>>(presenter.Result);
        }

        [HttpPost("RestoreTenant")]
        public ActionResult<Response<RestoreTenantResponse>> RestoreTenant([FromBody] RestoreTenantRequest request)
        {
            var input = new RestoreTenantInputData(request.TenantId, _webSocketService);
            var output = _bus.Handle(input);
            var presenter = new RestoreTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<RestoreTenantResponse>>(presenter.Result);
        }

        [HttpPost("RestoreObjectS3Tenant")]
        public ActionResult<Response<RestoreObjectS3TenantResponse>> RestoreObjectS3Tenant([FromBody] RestoreObjectS3TenantRequest request)
        {
            var input = new RestoreObjectS3TenantInputData(request.ObjectName, _webSocketService, request.Type, request.IsPrefixDelete);
            var output = _bus.Handle(input);
            var presenter = new RestoreObjectS3TenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<RestoreObjectS3TenantResponse>>(presenter.Result);
        }

        [HttpPost("UpdateDataTenant")]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
        public void UpdateDataTenant([FromForm] UpdateDataTenantRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _messenger.Register<UpdateDataTenantResult>(this, UpdateRecalculationStatus);
                _messenger.Register<StopUpdateDataTenantStatus>(this, StopCalculation);
                uniqueKey = Guid.NewGuid().ToString();
                _cancellationToken = cancellationToken;
                var input = new UpdateDataTenantInputData(request.TenantId, _webSocketService, request.FileUpdateData, cancellationToken, _messenger);
                var output = _bus.Handle(input);
            }
            catch (Exception ex)
            {
                stopCalculate = true;
                Console.WriteLine("Exception Cloud:" + ex.Message);
                SendMessage(new UpdateDataTenantResult(true, string.Empty, 0, 0, "", string.Empty));
            }
            finally
            {
                stopCalculate = true;
                _messenger.Deregister<UpdateDataTenantResult>(this, UpdateRecalculationStatus);
                _messenger.Deregister<StopUpdateDataTenantStatus>(this, StopCalculation);
                HttpContext.Response.Body.Close();
            }

            //var presenter = new UpdateDataTenantPresenter();
            //presenter.Complete(output);
            //return new ActionResult<Response<UpdateDataTenantResponse>>(presenter.Result);
        }

        private void StopCalculation(StopUpdateDataTenantStatus stopCalcStatus)
        {
            if (stopCalculate)
            {
                stopCalcStatus.CallFailCallback(stopCalculate);
            }
            else if (!_cancellationToken.HasValue)
            {
                stopCalcStatus.CallFailCallback(false);
            }
            else
            {
                stopCalcStatus.CallSuccessCallback(_cancellationToken!.Value.IsCancellationRequested);
            }
        }

        private void UpdateRecalculationStatus(UpdateDataTenantResult status)
        {
            try
            {
                stopCalculate = status.Done;
                SendMessage(status);

            }
            catch (Exception)
            {
                stopCalculate = true;
                SendMessage(new UpdateDataTenantResult(true, string.Empty, 0, 0, "", string.Empty));
                throw;
            }

        }


        private void SendMessage(UpdateDataTenantResult status)
        {
            //var dto = new RecalculationDto(status);
            string result = "\n" + "";
            var resultForFrontEnd = Encoding.UTF8.GetBytes(result.ToString());
            HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
            HttpContext.Response.Body.FlushAsync();
        }
    }
}
