using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class MenuItemModel
    {
        public MenuItemModel(MenuInfModel menu, List<MenuInfModel> childrens)
        {
            Menu = menu;
            Childrens = childrens;
        }

        public MenuInfModel  Menu { get; private set; }

        public List<MenuInfModel>  Childrens { get; private set; }
    }
}
