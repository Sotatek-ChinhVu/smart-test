using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class SummaryTabItem
    {
        public SummaryTabItem(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}
