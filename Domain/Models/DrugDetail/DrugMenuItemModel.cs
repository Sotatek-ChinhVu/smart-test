using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class DrugMenuItemModel
    {
        public DrugMenuItemModel(MenuItemModel menuItem, int indexOfChildren, int indexOfLevel0, string yjCode)
        {
            MenuItem = menuItem;
            IndexOfChildren = indexOfChildren;
            IndexOfLevel0 = indexOfLevel0;
            YjCode = yjCode;
        }

        public DrugMenuItemModel()
        {
            MenuItem = new MenuItemModel(new MenuInfModel(), new List<MenuInfModel>());
            YjCode = string.Empty;
        }

        public MenuItemModel MenuItem { get; private set; }

        public int IndexOfChildren { get; set; }

        public int IndexOfLevel0 { get; set; }

        public string YjCode { get; private set; }
    }
}
