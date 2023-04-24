using Entity.Tenant;

namespace Reporting.Statistics.Sta1001.Models
{
    public class CoJihiSbtMstModel
    {
        public JihiSbtMst JihiSbtMst { get; private set; }

        public CoJihiSbtMstModel(JihiSbtMst jihiSbtMst)
        {
            JihiSbtMst = jihiSbtMst;
        }

        public int JihiSbt
        {
            get { return JihiSbtMst.JihiSbt; }
        }

        public int SortNo
        {
            get { return JihiSbtMst.SortNo; }
        }

        public string Name
        {
            get { return JihiSbtMst.Name ?? string.Empty; }
        }
    }
}
