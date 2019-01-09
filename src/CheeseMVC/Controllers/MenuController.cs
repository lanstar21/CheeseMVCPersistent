using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IList<Menu> menus = context.Menus.ToList();
            return View(menus);
        }


        [HttpGet]
        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(addMenuViewModel);
            }
            else
            {
                Menu newMenu = new Menu();
                newMenu.Name = addMenuViewModel.Name;
                context.Menus.Add(newMenu);
                context.SaveChanges();
                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }
        }

        [HttpGet]
        public IActionResult ViewMenu(int id)
        {
            Menu newMenu = new Menu();
            newMenu = context.Menus.Single(m => m.ID == id);

            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel(items, newMenu);
            return View(viewMenuViewModel);
        }

        [HttpGet]
        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Single(m => m.ID == id);
            List<Cheese> cheeses = context.Cheeses.ToList();
            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, cheeses);
            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(addMenuItemViewModel);
            }
            else
            {
                IList<CheeseMenu> existingItems = context.CheeseMenus
        .Where(cm => cm.CheeseID == addMenuItemViewModel.CheeseID)
        .Where(cm => cm.MenuID == addMenuItemViewModel.MenuID).ToList();
                if (existingItems.Count == 0)
                {
                    CheeseMenu cheeseMenu = new CheeseMenu();
                    cheeseMenu.CheeseID = addMenuItemViewModel.CheeseID;
                    cheeseMenu.MenuID = addMenuItemViewModel.MenuID;
                    context.CheeseMenus.Add(cheeseMenu);
                    context.SaveChanges();

                    return Redirect("/Menu/ViewMenu/" + cheeseMenu.MenuID);
                }
                else
                {
                    return View(addMenuItemViewModel);
                }
            }
        }
    }
}
