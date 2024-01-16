using Domain.Models.Diseases;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Diseases;
using EmrCloudApi.Presenters.MstItem;
using EmrCloudApi.Requests.Diseases;
using EmrCloudApi.Requests.ListSetMst;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Diseases;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.ByomeiSetMst.UpdateByomeiSetMst;
using UseCase.Core.Sync;
using UseCase.Diseases.GetAllByomeiByPtId;
using UseCase.Diseases.GetDiseaseList;
using UseCase.Diseases.GetSetByomeiTree;
using UseCase.Diseases.IsHokenInfInUsed;
using UseCase.Diseases.Upsert;
using UseCase.Diseases.Validation;
using UseCase.ListSetMst.UpdateListSetMst;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class DiseasesController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public DiseasesController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetPtDiseaseListResponse>> GetDiseaseListMedicalExamination([FromQuery] GetPtDiseaseListRequest request)
        {
            var input = new GetPtDiseaseListInputData(HpId, request.PtId, request.SinDate, request.HokenId, request.RequestFrom, request.IsContiFiltered, request.IsInMonthFiltered);
            var output = _bus.Handle(input);

            var presenter = new GetPtDiseaseListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPtDiseaseListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetAllByomeiByPtId)]
        public ActionResult<Response<GetAllByomeiByPtIdResponse>> GetAllByomeiByPtId([FromQuery] GetAllByomeiByPtIdRequest request)
        {
            var input = new GetAllByomeiByPtIdInputData(HpId, request.PtId, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new GetAllByomeiByPtIdPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetAllByomeiByPtIdResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Upsert)]
        public ActionResult<Response<UpsertPtDiseaseListResponse>> Upsert([FromBody] UpsertPtDiseaseListRequest request)
        {

            var input = new UpsertPtDiseaseListInputData(request.PtDiseases.Select(r => new UpsertPtDiseaseListInputItem(
                                                            r.Id,
                                                            r.PtId,
                                                            r.SortNo,
                                                            r.PrefixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                                                            r.SuffixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                                                            r.Byomei,
                                                            r.StartDate,
                                                            r.TenkiKbn,
                                                            r.TenkiDate,
                                                            r.SyubyoKbn,
                                                            r.SikkanKbn,
                                                            r.NanByoCd,
                                                            r.HosokuCmt,
                                                            r.HokenPid,
                                                            r.IsNodspRece,
                                                            r.IsNodspKarte,
                                                            r.SeqNo,
                                                            r.IsImportant,
                                                            r.IsDeleted,
                                                            r.ByomeiCd,
                                                            HpId
                                                        )).ToList(),
                                                        HpId,
                                                        UserId
                                                        );
            var output = _bus.Handle(input);

            var presenter = new UpsertPtDiseaseListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertPtDiseaseListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSetByomeiTree)]
        public ActionResult<Response<GetSetByomeiTreeResponse>> GetSetByomeiTree([FromQuery] GetSetByomeiTreeRequest request)
        {
            var input = new GetSetByomeiTreeInputData(HpId, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetSetByomeiTreePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetSetByomeiTreeResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Validate)]
        public ActionResult<Response<ValidationPtDiseaseListResponse>> Validate([FromBody] UpsertPtDiseaseListRequest request)
        {
            var input = new ValidationPtDiseaseListInputData(request.PtDiseases.Select(r => new ValidationPtDiseaseListInputItem(
                                                            r.Id,
                                                            r.PtId,
                                                            r.SortNo,
                                                            r.PrefixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                                                            r.SuffixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                                                            r.Byomei,
                                                            r.StartDate,
                                                            r.TenkiKbn,
                                                            r.TenkiDate,
                                                            r.SyubyoKbn,
                                                            r.SikkanKbn,
                                                            r.NanByoCd,
                                                            r.HosokuCmt,
                                                            r.HokenPid,
                                                            r.IsNodspRece,
                                                            r.IsNodspKarte,
                                                            r.SeqNo,
                                                            r.IsImportant,
                                                            r.IsDeleted,
                                                            r.ByomeiCd,
                                                            HpId
                                                        )).ToList(),
                                                        HpId,
                                                        UserId
                                                        );
            var output = _bus.Handle(input);

            var presenter = new ValidationPtDiseaseListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidationPtDiseaseListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateByomeiSetMst)]
        public ActionResult<Response<UpdateByomeiSetMstResponse>> UpdateByomeiSetMst([FromBody] UpdateByomeiSetMstRequest request)
        {
            var input = new UpdateByomeiSetMstInputData(UserId, HpId, request.ByomeiSetMsts);
            var output = _bus.Handle(input);
            var presenter = new UpdateByomeiSetMstPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }


        [HttpGet(ApiPath.IsHokenInfInUsed)]
        public ActionResult<Response<UpdateByomeiSetMstResponse>> IsHokenInfInUsed([FromQuery] IsHokenInfInUsedRequest request)
        {
            var input = new IsHokenInfInUsedInputData(HpId, request.PtId, request.HokenPId);
            var output = _bus.Handle(input);
            var presenter = new IsHokenInfInUsedPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

    }
}
