using Domain.Models.Insurance;

namespace EmrCloudApi.Tenant.Responses.Insurance
{
    public class RousaiTenkiDto
    {
        public RousaiTenkiDto(RousaiTenkiModel rousaiTenkiModel)
        {
            RousaiTenkiSinkei = rousaiTenkiModel.RousaiTenkiSinkei;
            RousaiTenkiTenki = rousaiTenkiModel.RousaiTenkiTenki;
            RousaiTenkiEndDate = rousaiTenkiModel.RousaiTenkiEndDate;
            RousaiTenkiIsDeleted = rousaiTenkiModel.RousaiTenkiIsDeleted;
            SeqNo = rousaiTenkiModel.SeqNo;
        }

        public int RousaiTenkiSinkei { get; private set; }

        public int RousaiTenkiTenki { get; private set; }

        public int RousaiTenkiEndDate { get; private set; }

        public int RousaiTenkiIsDeleted { get; private set; }

        public long SeqNo { get; private set; }
    }
}
