using Domain.Models.PtGroupMst;

namespace EmrCloudApi.Responses.PtGroupMst
{
    public class GroupItemDtoResponse
    {
        public GroupItemDtoResponse(GroupItemModel model)
        {
            GrpId = model.GrpId;
            GrpCode = model.GrpCode;
            SeqNo = model.SeqNo;
            GrpCodeName = model.GrpCodeName;
            SortNo = model.SortNo;
        }

        public int GrpId { get; private set; }

        public string GrpCode { get; private set; } = string.Empty;

        public long SeqNo { get; private set; }

        public string GrpCodeName { get; private set; } = string.Empty;

        public int SortNo { get; private set; }
    }
}
