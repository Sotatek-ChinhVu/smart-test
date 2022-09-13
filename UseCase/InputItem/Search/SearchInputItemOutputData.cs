using Domain.Models.InputItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.InputItem.Search
{
    public class SearchInputItemOutputData: IOutputData
    {
        public SearchInputItemOutputData(List<InputItemModel> listInputModel, int totalCount, SearchInputItemStatus status)
        {
            ListInputModel = listInputModel;
            TotalCount = totalCount;
            Status = status;
        }

        public List<InputItemModel> ListInputModel { get; private set; }

        public int TotalCount { get; private set; }

        public SearchInputItemStatus Status { get; private set; }

    }
}
