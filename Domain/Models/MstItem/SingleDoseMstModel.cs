
using Helper.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MstItem
{
    public class SingleDoseMstModel
    {
        public SingleDoseMstModel(ModelStatus status, bool isDeleted, long id, int hpId, string unitName)
        {
            Status = status;
            IsDeleted = isDeleted;
            Id = id;
            HpId = hpId;
            UnitName = unitName;
        }
        public ModelStatus Status { get; private set; }

        public bool IsDeleted { get; private set; }

        public string IdString
        {
            get
            {
                if (Id == 0) return string.Empty;
                return Id.ToString().PadLeft(4, '0');
            }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public long Id { get; private set; }
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        public string UnitName { get; private set; }


        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(UnitName);
        }
    }
}
