using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MstItem
{
    public class MedicineUnitModel
    {
        public MedicineUnitModel(string odrUnitName, bool isRegister)
        {
            OdrUnitName = odrUnitName;
            IsRegister = isRegister;
        }
        /// <summary>
        /// オーダー単位名称
        ///     オーダー時に使用する単位
        /// </summary>
        /// 
        public string OdrUnitName { get; private set; }

        public bool IsRegister { get; private set; }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(OdrUnitName);
        }
    }
}
