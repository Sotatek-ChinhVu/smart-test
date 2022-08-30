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
        public SearchInputItemOutputData(List<InputItemModel> listInputModel, SearchInputItemStatus status)
        {
            ListInputModel = listInputModel;
            Status = status;
        }

        public List<InputItemModel> ListInputModel { get; private set; }

        public SearchInputItemStatus Status { get; private set; }

    }
}
