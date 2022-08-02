using Domain.Models.IsuranceMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.SearchHokensyaMst.Get
{
    public class SearchHokensyaMstOutputData : IOutputData
    {
        public List<HokensyaMstModel> ListHokensyaMst { get; private set; }

        public SearchHokensyaMstStatus Status { get; private set; }

        public SearchHokensyaMstOutputData(List<HokensyaMstModel> listHokensyaMst, SearchHokensyaMstStatus status)
        {
            ListHokensyaMst = listHokensyaMst;
            Status = status;
        }
    }
}
