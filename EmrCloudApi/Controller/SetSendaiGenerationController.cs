using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SetSendaiGeneration;
using EmrCloudApi.Requests.SetSendaiGeneration;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.GetSendaiGeneration;
using EmrCloudApi.Responses.SetSendaiGeneration;
using EmrCloudApi.Services;
using Helper.Messaging;
using Helper.Messaging.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading;
using UseCase.Core.Sync;
using UseCase.SetSendaiGeneration.Add;
using UseCase.SetSendaiGeneration.Delete;
using UseCase.SetSendaiGeneration.GetList;
using UseCase.SetSendaiGeneration.Restore;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SetSendaiGenerationController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IMessenger _messenger;
        private CancellationToken? _cancellationToken;
        public SetSendaiGenerationController(UseCaseBus bus, IUserService userService, IMessenger messenger) : base(userService)
        {
            _bus = bus;
            _messenger = messenger;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<SetSendaiGenerationGetListResponse>> GetList()
        {
            var input = new SetSendaiGenerationInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new SetSendaiGenerationGetListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SetSendaiGenerationGetListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Delete)]
        public ActionResult<Response<DeleteSetSendaiGenerationResponse>> Delete([FromBody] DeleteSetSendaiGenerationRequest request)
        {
            var input = new DeleteSendaiGenerationInputData(HpId, request.GenerationId, request.RowIndex, UserId);
            var output = _bus.Handle(input);

            var presenter = new DeleteSetSendaiGenerationPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<DeleteSetSendaiGenerationResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Insert)]
        public void AddSetSensaiGeneration([FromBody] AddSetSendaiGenerationRequest request, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            try
            {
                _messenger.Register<ProcessSetSendaiGenerationStatus>(this, UpdateProcessSave);
                _messenger.Register<ProcessSetSendaiGenerationStop>(this, StopProcess);
                HttpContext.Response.ContentType = "application/json";
                var input = new AddSetSendaiGenerationInputData(request.StartDate, HpId, UserId, _messenger);
                var output = _bus.Handle(input);
                if (output.Status == AddSetSendaiGenerationStatus.Success)
                {
                    UpdateProcessSave(new ProcessSetSendaiGenerationStatus(string.Empty, 100, true, true));
                }
                else
                {
                    UpdateProcessSave(new ProcessSetSendaiGenerationStatus(string.Empty, 100, true, false));
                }    
            }
            finally
            {
                _messenger.Deregister<ProcessSetSendaiGenerationStatus>(this, UpdateProcessSave);
                _messenger.Deregister<ProcessSetSendaiGenerationStop>(this, StopProcess);
            }
        }


        private void StopProcess(ProcessSetSendaiGenerationStop stopCalcStatus)
        {
            if (!_cancellationToken.HasValue)
            {
                stopCalcStatus.CallFailCallback(false);
            }
            else
            {
                stopCalcStatus.CallSuccessCallback(_cancellationToken!.Value.IsCancellationRequested);
            }
        }

        private void UpdateProcessSave(ProcessSetSendaiGenerationStatus status)
        {
            StringBuilder titleProgressbar = new();
            titleProgressbar.Append("\n{ displayText: \"");
            titleProgressbar.Append(status.DisplayText);
            titleProgressbar.Append("\", percent: ");
            titleProgressbar.Append(status.Percent);
            titleProgressbar.Append(", complete: ");
            titleProgressbar.Append(status.Complete.ToString().ToLower());
            titleProgressbar.Append(", completeSuccess: ");
            titleProgressbar.Append(status.CompleteSuccess.ToString().ToLower());
            titleProgressbar.Append(" }");
            var resultForFrontEnd = Encoding.UTF8.GetBytes(titleProgressbar.ToString());
            HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
            HttpContext.Response.Body.FlushAsync();
        }

        [HttpPost(ApiPath.Restore)]
        public void Restore([FromBody] RestoreSetSendaiGenerationRequest request, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            try
            {
                _messenger.Register<ProcessSetSendaiGenerationStatus>(this, UpdateProcessSave);
                _messenger.Register<ProcessSetSendaiGenerationStop>(this, StopProcess);
                HttpContext.Response.ContentType = "application/json";
                var input = new RestoreSetSendaiGenerationInputData(request.RestoreGenerationId, HpId, UserId, _messenger);
                var output = _bus.Handle(input);
                if (output.Status == RestoreSetSendaiGenerationStatus.Success)
                {
                    UpdateProcessSave(new ProcessSetSendaiGenerationStatus(string.Empty, 100, true, true));
                }
                else
                {
                    UpdateProcessSave(new ProcessSetSendaiGenerationStatus(string.Empty, 100, true, false));
                }
            }
            finally
            {
                _messenger.Deregister<ProcessSetSendaiGenerationStatus>(this, UpdateProcessSave);
                _messenger.Deregister<ProcessSetSendaiGenerationStop>(this, StopProcess);
            }
        }
    }
}
