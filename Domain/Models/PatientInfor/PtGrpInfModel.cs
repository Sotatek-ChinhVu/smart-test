using Helper.Mapping.Attributes;

namespace Domain.Models.PatientInfor
{
    public class PtGrpInfModel
    {
        public PtGrpInfModel(int grpId, string grpCode)
        {
            GrpId = grpId;
            GrpCode = grpCode;
        }

        public int GrpId { get;private set; }
        public string GrpCode { get; private set; }
    }
}
