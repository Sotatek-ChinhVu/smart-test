using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class DrugMenuItemModel
    {
        public DrugMenuItemModel(MenuItemModel menuItem, DrugDetailModel detailInfor)
        {
            MenuItem = menuItem;
            DetailInfor = detailInfor;
        }

        public DrugMenuItemModel()
        {
            MenuItem = new MenuItemModel(new MenuInfModel(), new List<MenuInfModel>());
            DetailInfor = new DrugDetailModel();
        }

        public MenuItemModel MenuItem { get; set; }
        
        public DrugDetailModel DetailInfor { get;  set; }

    }
}
