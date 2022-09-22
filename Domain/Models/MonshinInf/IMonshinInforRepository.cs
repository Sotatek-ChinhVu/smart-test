using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MonshinInf
{
    public interface IMonshinInforRepository
    {
        public List<MonshinInforModel> MonshinInforModels(int hpId, long ptId);

        bool SaveList(List<MonshinInforModel> monshinInforModels);
    }
}
