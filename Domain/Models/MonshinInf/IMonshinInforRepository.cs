using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MonshinInf
{
    public interface IMonshinInforRepository
    {
        bool SaveList(List<MonshinInforModel> monshinInforModels);

        bool CheckExistMonshinInf(int hpId,long ptId, int sinDate, long raiinNo);
        
        public List<MonshinInforModel> MonshinInforModels(int hpId, long ptId, int sinDate, bool isDeleted);
    }
}
