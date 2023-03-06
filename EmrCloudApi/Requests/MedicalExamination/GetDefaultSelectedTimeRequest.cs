namespace EmrCloudApi.Requests.MedicalExamination
{
    public class GetDefaultSelectedTimeRequest
    {
        public int SinDate { get; set; }

        public int DayOfWeek { get; set; }

        public int UketukeTime { get; set; }

        public int BirthDay { get; set; }

        public bool FromOutOfSystem { get; set; }
    }
}
