using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InputItem
{
    public interface IInputItemRepository
    {
        public IEnumerable<InputItemModel> SearchDataInputItem(string keyword, int KouiKbn, int STDDate, string ItemCodeStartWith, int pageIndex, int pageCount);
    }
}
