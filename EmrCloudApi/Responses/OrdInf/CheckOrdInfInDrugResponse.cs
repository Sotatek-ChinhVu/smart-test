namespace EmrCloudApi.Responses.OrdInfs
{
    public class CheckOrdInfInDrugResponse
    {
        public CheckOrdInfInDrugResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
