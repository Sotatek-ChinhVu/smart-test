using Domain.Models.PatientInfor;

namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class SavePatientInfoRequest
    {
        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public SavePatientInfoModel PtInformation { get; set; }
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
