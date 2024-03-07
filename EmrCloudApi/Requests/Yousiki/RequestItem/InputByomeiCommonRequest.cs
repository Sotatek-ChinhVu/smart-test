namespace EmrCloudApi.Requests.Yousiki.RequestItem
{
    public class InputByomeiCommonRequest
    {
        public CommonForm1Request ByomeiInf { get; set; } = new();

        public Yousiki1InfDetailRequest? DuringMonthMedicineInfModel { get; set; } 

        public Yousiki1InfDetailRequest? FinalMedicineDateModel { get; set; } 
    }
}
