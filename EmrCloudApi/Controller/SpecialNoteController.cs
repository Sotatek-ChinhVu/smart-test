﻿using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SpecialNote;
using EmrCloudApi.Requests.SpecialNote;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SpecialNote;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SpecialNote.AddAlrgyDrugList;
using UseCase.SpecialNote.Get;
using UseCase.SpecialNote.GetPtWeight;
using UseCase.SpecialNote.Save;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SpecialNoteController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public SpecialNoteController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.Get)]
        public ActionResult<Response<GetSpecialNoteResponse>> Get([FromQuery] SpecialNoteRequest request)
        {
            var input = new GetSpecialNoteInputData(HpId, request.PtId, request.Sex);
            var output = _bus.Handle(input);

            var presenter = new GetSpecialNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSpecialNoteResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.AddAlrgyDrugList)]
        public ActionResult<Response<AddAlrgyDrugListResponse>> AddAlrgyDrugList([FromBody] AddAlrgyDrugListRequest request)
        {
            var input = new AddAlrgyDrugListInputData(request.AlgrgyDrugs.Select(
                                                        a => new AddAlrgyDrugListItemInputData(
                                                                a.PtId,
                                                                a.ItemCd,
                                                                a.DrugName,
                                                                a.StartDate,
                                                                a.EndDate,
                                                                a.Cmt
                                                            )
                                                        ).ToList(),
                                                        HpId,
                                                        UserId
                                                        );
            var output = _bus.Handle(input);

            var presenter = new AddAlrgyDrugListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<AddAlrgyDrugListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Save)]
        public ActionResult<Response<SaveSpecialNoteResponse>> Save([FromBody] SpecialNoteSaveRequest request)
        {
            var input = new SaveSpecialNoteInputData(HpId, request.PtId, request.SinDate, request.SummaryTab.Map(), request.ImportantNoteTab.Map(), request.PatientInfoTab.Map(), UserId);
            var output = _bus.Handle(input);

            var presenter = new SaveSpecialNotePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveSpecialNoteResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetPtWeight)]
        public ActionResult<Response<GetPtWeightResponse>> Save([FromQuery] GetPtWeightRequest request)
        {
            var input = new GetPtWeightInputData(request.SinDate, request.PtId);
            var output = _bus.Handle(input);

            var presenter = new GetPtWeightPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPtWeightResponse>>(presenter.Result);
        }
    }
}