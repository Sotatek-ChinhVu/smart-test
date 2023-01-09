using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Ika.Models
{
    public class RaiinDaysModel
    {
        private int _hpId;
        private long _ptId;
        private int _sinDate;
        
        public RaiinDaysModel(int hpId, long ptId, int sinDate)
        {
            _hpId = hpId;
            _ptId = ptId;
            _sinDate = sinDate;            
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return _hpId; }
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get { return _ptId; }
        }

        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinDate
        {
            get { return _sinDate; }
        }
    }
}
