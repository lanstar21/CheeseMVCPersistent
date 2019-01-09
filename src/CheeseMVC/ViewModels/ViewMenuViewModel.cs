
using CheeseMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;

namespace CheeseMVC.ViewModels
{
    public class ViewMenuViewModel
    {
        public Menu Menu { get; set; }
        public IList<CheeseMenu> Items { get; set; }

        public ViewMenuViewModel(List<CheeseMenu> items, Menu newMenu)
        { }

        public ViewMenuViewModel(Menu menu, IList<CheeseMenu> items)
        {
            Menu = menu;
            Items = items;
        }
    }
}