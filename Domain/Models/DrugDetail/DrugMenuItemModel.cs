using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class DrugMenuItemModel
    {
        public DrugMenuItemModel(MenuItemModel menuItem, int indexOfChildrens, int indexOfLevel0)
        {
            MenuItem = menuItem;
            IndexOfChildrens = indexOfChildrens;
            IndexOfLevel0 = indexOfLevel0;
        }

        public DrugMenuItemModel()
        {
            MenuItem = new MenuItemModel(new MenuInfModel(), new List<MenuInfModel>());
        }

        public MenuItemModel MenuItem { get; set; }

        public int IndexOfChildrens { get; set; }

        public int IndexOfLevel0 { get; set; }
    }
}
