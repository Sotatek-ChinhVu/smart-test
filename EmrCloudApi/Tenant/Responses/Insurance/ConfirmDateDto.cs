using Domain.Models.Insurance;

namespace EmrCloudApi.Tenant.Responses.Insurance
{
    public class ConfirmDateDto
    {
        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int CheckId { get; private set; }

        public string CheckName { get; private set; }

        public string CheckComment { get; private set; }

        public int ConfirmDate { get; private set; }

        public ConfirmDateDto(ConfirmDateModel confirmDateModel)
        {
            HokenGrp = confirmDateModel.HokenGrp;
            HokenId = confirmDateModel.HokenId;
            SeqNo = confirmDateModel.SeqNo;
            CheckId = confirmDateModel.CheckId;
            CheckName = confirmDateModel.CheckName;
            CheckComment = confirmDateModel.CheckComment;
            ConfirmDate = confirmDateModel.ConfirmDate;
        }
    }
}
