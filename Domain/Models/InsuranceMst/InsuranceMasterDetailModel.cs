namespace Domain.Models.InsuranceMst
{
    public class InsuranceMasterDetailModel
    {
        public HokenMstModel Master { get; private set; }

        public IEnumerable<HokenMstModel> Details { get; private set; }

        public InsuranceMasterDetailModel(HokenMstModel master, IEnumerable<HokenMstModel> details)
        {
            Master = master;
            Details = details;
        }
    }
}
