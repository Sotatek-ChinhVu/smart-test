using Entity.Tenant;

namespace Reporting.Calculate.Ika.Models
{
    public class TenDataModel
    {
        public TenMst tenMst { get; set; }

        public IpnKasanMst kasanMst { get; set; }

        public IpnMinYakkaMst minYakkaMst { get; set; }

        public TenDataModel() { }


        public int kasan1
        {
            get
            {
                return kasanMst != null ? kasanMst.Kasan1 : 0;
            }
        }

        public int kasan2
        {
            get
            {
                return kasanMst != null ? kasanMst.Kasan2 : 0;
            }
        }

        public double minYakka
        {
            get
            {
                return minYakkaMst != null ? minYakkaMst.Yakka : 0;
            }
        }
    }
}
