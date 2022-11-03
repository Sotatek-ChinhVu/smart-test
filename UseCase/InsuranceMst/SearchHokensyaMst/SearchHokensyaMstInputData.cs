using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.SearchHokensyaMst.Get
{
    public class SearchHokensyaMstInputData : IInputData<SearchHokensyaMstOutputData>
    {
        public SearchHokensyaMstInputData(int hpId, int sinDate, string? keyword)
        {
            HpId = hpId;
            SinDate = sinDate;
            Keyword = keyword;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public string? Keyword { get; private set; }
    }
}
