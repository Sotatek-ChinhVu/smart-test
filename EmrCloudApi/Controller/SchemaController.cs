using Domain.Models.Insurance;
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
using UseCase.Schema.GetListInsuranceScan;
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
        public ActionResult<Response<SaveImageResponse>> SaveInsuranceScanImage([FromForm] IList<SaveInsuranceScanRequest> request)
        {
            var input = new SaveInsuranceScanInputData(HpId,
                                                     UserId, 
                                                     request.Select(x=> new InsuranceScanModel(
                                                                    HpId, 
                                                                    x.PtId , 
                                                                    x.SeqNo,
                                                                    x.HokenGrp, 
                                                                    x.HokenId,
                                                                    x.FileName,
                                                                    x.File?.OpenReadStream() ?? new MemoryStream(), 
                                                                    x.IsDeleted)));
            var output = _bus.Handle(input);
            var presenter = new SaveInsuranceScanPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveImageResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UploadListFileKarte)]
        public ActionResult<Response<SaveListFileResponse>> MultiUpload([FromQuery] SaveListFileRequest request, [FromForm] List<IFormFile> files)
        {
            List<FileItem> listFiles = new();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    bool isSchema = false;
                    if (file.FileName.EndsWith(".schema"))
                    {
                        fileName = file.FileName.Replace(".schema", "." + file.ContentType.Replace("image/", string.Empty));
                        isSchema = true;
                    }
                    var streamImage = new MemoryStream();
                    file.CopyTo(streamImage);
                    listFiles.Add(new FileItem(fileName, isSchema, streamImage));
                }
            }
            var input = new SaveListFileTodayOrderInputData(HpId, request.PtId, request.SetCd, request.TypeUpload, listFiles);
            var output = _bus.Handle(input);
            var presenter = new SaveListFilePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveListFileResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListInsuranceScan)]
        public ActionResult<Response<GetListInsuranceScanResponse>> GetListInsuranceScan([FromQuery] long ptId)
        {
            var input = new GetListInsuranceScanInputData(HpId, ptId);
            var output = _bus.Handle(input);
            var presenter = new GetListInsuranceScanPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListInsuranceScanResponse>>(presenter.Result);
        }
    }
}
