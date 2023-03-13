using EmrCalculateApi.Interface;
using EmrCalculateApi.Receipt.Constants;
using EmrCalculateApi.Receipt.ViewModels;
using EmrCalculateApi.Requests;
using EmrCalculateApi.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmrCalculateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SinMeiController : ControllerBase
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        public SinMeiController(ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _tenantProvider = tenantProvider;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        [HttpPost("GetSinMeiList")]
        public ActionResult<GetSinMeiListResponse> GetSinMeiList([FromBody] GetSinMeiListRequest getSinMeiListRequest)
        {
            int hpId = getSinMeiListRequest.HpId;
            long ptId = getSinMeiListRequest.PtId;
            int sinDate = getSinMeiListRequest.SinDate;
            List<long> raiinNoList = getSinMeiListRequest.RaiinNoList;

            using (SinMeiViewModel sinMeiVM = new SinMeiViewModel(SinMeiMode.Kaikei, false, hpId, ptId, sinDate, raiinNoList, _tenantProvider, _systemConfigProvider, _emrLogger))
            {
                return new ActionResult<GetSinMeiListResponse>(new GetSinMeiListResponse(sinMeiVM.SinMei));
            }
        }

        [HttpPost("AccountingCard/GetSinMeiList")]
        public ActionResult<GetSinMeiListResponse> AccountingCardGetSinMeiList([FromBody] GetSinMeiAccountingCardRequest request)
        {
            using (SinMeiViewModel sinMeiVM = new SinMeiViewModel(SinMeiMode.AccountingCard, true, request.HpId, request.PtId, request.SinYm, request.HokenId, _tenantProvider, _systemConfigProvider, _emrLogger))
            {
                return new ActionResult<GetSinMeiListResponse>(new GetSinMeiListResponse(sinMeiVM.SinMei));
            }
        }
    }
}
