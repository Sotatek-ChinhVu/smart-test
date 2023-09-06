
using Helper.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MstItem
{
    public class SingleDoseMstModel //: ObservableObject //, IEmrDataGridModel
    {
        public SingleDoseMstModel(ModelStatus status, bool isDeleted, long id, int hpId, string unitName,
        DateTime createDate, int createId, string createMachine, DateTime updateDate,
        int updateId, string updateMachine)
        {
            Status = status;
            IsDeleted = isDeleted;
            Id = id;
            HpId = hpId;
            UnitName = unitName;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
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
            private set { }
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

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate { get; private set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId { get; private set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine { get; private set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate { get; private set; }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId { get; private set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine { get; private set; }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(UnitName);
        }
    }
}
