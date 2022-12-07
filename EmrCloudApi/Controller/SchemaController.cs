using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Schema;
using EmrCloudApi.Requests.Schema;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Schema;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using Schema.Insurance.SaveInsuranceScan;
using UseCase.Core.Sync;
using UseCase.Schema.GetListImageTemplates;
using UseCase.Schema.SaveListFileTodayOrder;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SchemaController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public SchemaController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetListImageTemplatesResponse>> GetList()
        {
            var input = new GetListImageTemplatesInputData();
            var output = _bus.Handle(input);

            var presenter = new GetListImageTemplatesPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListImageTemplatesResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveInsuranceScanImage)]
        public ActionResult<Response<SaveImageResponse>> SaveInsuranceScanImage([FromQuery] SaveInsuranceScanRequest request)
        {
            var input = new SaveInsuranceScanInputData(HpId, request.PtId, request.HokenGrp, request.HokenId, request.UrlOldImage, UserId, Request.Body);
            var output = _bus.Handle(input);

            var presenter = new SaveInsuranceScanPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveImageResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveListFile)]
        public ActionResult<Response<SaveListFileTodayOrderResponse>> MultiUpload([FromQuery] SaveListFileTodayOrderRequest request, [FromForm] List<IFormFile> files, [FromForm] string listFileDeletes)
        {
            List<FileItem> listFiles = new();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var streamImage = new MemoryStream();
                    file.CopyTo(streamImage);
                    listFiles.Add(new FileItem(file.FileName, streamImage));
                }
            }
            List<long> listIdSplits = new();
            if (listFileDeletes.Length > 0)
            {
                foreach (var item in listFileDeletes.Trim().Split(',').ToList())
                {
                    if (long.Parse(item) > 0)
                    {
                        listIdSplits.Add(long.Parse(item));
                    }
                }
            }
            var input = new SaveListFileTodayOrderInputData(HpId, request.PtId, request.RaiinNo, listFiles, listIdSplits);
            var output = _bus.Handle(input);
            var presenter = new SaveListFileTodayOrderPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveListFileTodayOrderResponse>>(presenter.Result);
        }
    }
}
