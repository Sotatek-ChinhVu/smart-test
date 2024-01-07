using Entity.Tenant;

namespace Reporting.Yakutai.Model
{
    public class CoSingleDoseMstModel
    {
        public SingleDoseMst SingleDoseMst { get; } = null;

        public CoSingleDoseMstModel(SingleDoseMst singleDoseMst)
        {
            SingleDoseMst = singleDoseMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return SingleDoseMst.HpId; }
        }

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        public string UnitName
        {
            get { return SingleDoseMst.UnitName; }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public long Id
        {
            get { return SingleDoseMst.Id; }
        }


    }
}
