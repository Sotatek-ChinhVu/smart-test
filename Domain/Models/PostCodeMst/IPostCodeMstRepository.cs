using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PostCodeMst
{
    public interface IPostCodeMstRepository
    {
        public List<PostCodeMstModel> PostCodeMstModels(int hpId, string postCode1, string postCode2, string address);
    }
}
