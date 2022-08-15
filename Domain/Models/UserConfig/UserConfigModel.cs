using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.UserConfig
{
    public class UserConfigModel
    {
        public UserConfigModel(int hpId, int userId, int grpCd, int grpItemCd, int grpItemEdaNo, int val, string param)
        {
            HpId = hpId;
            UserId = userId;
            GrpCd = grpCd;
            GrpItemCd = grpItemCd;
            GrpItemEdaNo = grpItemEdaNo;
            Val = val;
            Param = param;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public int GrpCd { get; private set; }
        public int GrpItemCd { get; private set; }
        public int GrpItemEdaNo { get; private set; }
        public int Val { get; private set; }
        public string Param { get; private set; }
    }
}
