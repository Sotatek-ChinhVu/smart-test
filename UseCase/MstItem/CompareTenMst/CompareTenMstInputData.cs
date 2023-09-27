using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.CompareTenMst
{
    public class CompareTenMstInputData: IInputData<CompareTenMstOutputData>
    {
        public CompareTenMstInputData(List<ActionCompareSearchModel> actions, ComparisonSearchModel comparions, int sinDate, int hpId)
        {
            Actions = actions;
            Comparions = comparions;
            SinDate = sinDate;
            HpId = hpId;
        }

        public List<ActionCompareSearchModel> Actions { get; private set; }

        public ComparisonSearchModel Comparions { get; private set; }

        public int SinDate { get; private set; }

        public int HpId { get; private set; }
    }
}
