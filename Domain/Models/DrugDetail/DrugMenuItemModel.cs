using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class DrugMenuItemModel
    {
        public DrugMenuItemModel(MenuItemModel menuItem, int indexOfChildrens, int indexOfLevel0, string yjCode)
        {
            MenuItem = menuItem;
            IndexOfChildrens = indexOfChildrens;
            IndexOfLevel0 = indexOfLevel0;
            YjCode = yjCode;
        }

        public DrugMenuItemModel()
        {
            MenuItem = new MenuItemModel(new MenuInfModel(), new List<MenuInfModel>());
            YjCode = string.Empty;
        }

        public MenuItemModel MenuItem { get; set; }

        public int IndexOfChildrens { get; set; }

        public int IndexOfLevel0 { get; set; }

        public string YjCode { get; set; }
    }
}
