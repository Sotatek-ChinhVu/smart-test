namespace Domain.Models.Insurance
{
    public class HokensyaMstModel
    {
        public HokensyaMstModel(int isKigoNa)
        {
            IsKigoNa = isKigoNa;
        }

        public HokensyaMstModel()
        {
        }

        public int IsKigoNa { get; private set; }
    }
}
