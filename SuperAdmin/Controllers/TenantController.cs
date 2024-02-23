using Domain.SuperAdminModels.Tenant;
using Helper.Messaging;
using Helper.Messaging.Data;
using Interactor.Realtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Responses;
using SuperAdminAPI.Presenters.Tenant;
using SuperAdminAPI.Reponse.Tenant;
using SuperAdminAPI.Request.Tennant;
using System.Text;
using System.Text.Json;
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
using UseCase.SuperAdmin.UploadDrugImage;

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
        private CancellationToken? _cancellationToken;
        private bool stopUploadDrugImage = false;
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
            var input = new UpdateTenantInputData(request.TenantId, request.SubDomain, request.Hospital, request.AdminId, request.Password, _webSocketService);
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
            var input = new TerminateTenantInputData(request.TenantId, _webSocketService);
            var output = _bus.Handle(input);
            var presenter = new TerminateTenantPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<TerminateTenantResponse>>(presenter.Result);
        }

        #region private function
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
                HttpContext.Response.ContentType = "application/json";
                _cancellationToken = cancellationToken;
                var input = new UpdateDataTenantInputData(request.TenantId, _webSocketService, request.FileUpdateData, cancellationToken, _messenger);
                var output = _bus.Handle(input);
            }
            catch (Exception ex)
            {
                stopCalculate = true;
                Console.WriteLine("Exception Cloud:" + ex.Message);
                SendMessage(new UpdateDataTenantResult(true, string.Empty, 0, 0, "", 0));
            }
            finally
            {
                stopCalculate = true;
                _messenger.Deregister<UpdateDataTenantResult>(this, UpdateRecalculationStatus);
                _messenger.Deregister<StopUpdateDataTenantStatus>(this, StopCalculation);
                HttpContext.Response.Body.Close();
            }
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
                SendMessage(status);

            }
            catch (Exception)
            {
                SendMessage(new UpdateDataTenantResult(true, string.Empty, 0, 0, "", 0));
                throw;
            }

        }

        private void SendMessage(UpdateDataTenantResult status)
        {
            string result = "\n" + JsonSerializer.Serialize(status);
            var resultForFrontEnd = Encoding.UTF8.GetBytes(result.ToString());
            HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
            HttpContext.Response.Body.FlushAsync();
        }

        #region UploadDrugImageAndRelease
        [HttpPost("UploadDrugImageAndRelease")]
        public void UploadDrugImageAndRelease([FromForm] UploadDrugImageAndReleaseRequest request, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            try
            {
                _messenger.Register<UploadDrugImageAndReleaseStatus>(this, ReturnUploadDrugImageAndReleaseStatus);
                _messenger.Register<StopUploadDrugImageAndRelease>(this, StopUploadDrugImageAndRelease);
                HttpContext.Response.ContentType = "application/json";

                var input = new UploadDrugImageAndReleaseInputData(request.FileUpdateData, _messenger, _webSocketService);
                _bus.Handle(input);
            }
            catch
            {
                stopUploadDrugImage = true;
            }
            finally
            {
                stopUploadDrugImage = true;
                _messenger.Deregister<UploadDrugImageAndReleaseStatus>(this, ReturnUploadDrugImageAndReleaseStatus);
                _messenger.Deregister<StopUploadDrugImageAndRelease>(this, StopUploadDrugImageAndRelease);
                HttpContext.Response.Body.Close();
            }
        }

        private void ReturnUploadDrugImageAndReleaseStatus(UploadDrugImageAndReleaseStatus status)
        {
            string result = "\n" + JsonSerializer.Serialize(status);
            var resultForFrontEnd = Encoding.UTF8.GetBytes(result.ToString());
            HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
            HttpContext.Response.Body.FlushAsync();
        }

        private void StopUploadDrugImageAndRelease(StopUploadDrugImageAndRelease status)
        {
            if (stopUploadDrugImage)
            {
                status.CallFailCallback(stopUploadDrugImage);
            }
            else if (!_cancellationToken.HasValue)
            {
                status.CallFailCallback(false);
            }
            else
            {
                status.CallSuccessCallback(_cancellationToken!.Value.IsCancellationRequested);
            }
        }
        #endregion UploadDrugImageAndRelease
    }
}
