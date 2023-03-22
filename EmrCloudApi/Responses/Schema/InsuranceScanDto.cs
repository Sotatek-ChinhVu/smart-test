using Domain.Models.Insurance;

namespace EmrCloudApi.Responses.Schema
{
    public class InsuranceScanDto
    {
        public InsuranceScanDto(InsuranceScanModel model)
        {
            HpId = model.HpId;
            PtId = model.PtId;
            SeqNo = model.SeqNo;
            HokenGrp = model.HokenGrp;
            HokenId = model.HokenId;
            FileName = model.FileName;
            FileNameDisplay = model.FileNameDisplay;
            IsDeleted = model.IsDeleted;
            UpdateTime = model.UpdateTime;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public string FileName { get; private set; }

        public string FileNameDisplay { get; private set; }

        public int IsDeleted { get; private set; }

        public string UpdateTime { get; private set; }
    }
}
