using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.SaveCompareTenMst
{
    public class SaveCompareTenMstInputData: IInputData<SaveCompareTenMstOutputData>
    {
        public SaveCompareTenMstInputData(List<SaveCompareTenMstModel> listData, ComparisonSearchModel comparison, int userId)
        {
            ListData = listData;
            Comparison = comparison;
            UserId = userId;
        }

        public List<SaveCompareTenMstModel> ListData { get; private set; }
        public ComparisonSearchModel Comparison { get; private set; }
        public int UserId { get; private set; }
    }
}
