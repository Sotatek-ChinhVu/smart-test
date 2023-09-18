using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MstItem.GetDiseaseList;

namespace UseCase.MstItem.DiseaseNameMstSearch
{
    public class DiseaseNameMstSearchOutputData : IOutputData
    {
        public List<ByomeiMstModel> ListData { get; private set; }

        public DiseaseNameMstSearchStatus Status { get; private set; }

        public DiseaseNameMstSearchOutputData(List<ByomeiMstModel> listData, DiseaseNameMstSearchStatus status)
        {
            ListData = listData;
            Status = status;
        }
    }
}
