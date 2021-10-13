using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using StoreBL;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IBL _bl;
        public AdminController(IBL bl)
        {
            _bl = bl;
        }
        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminController/Menu
        public ActionResult Menu(IFormCollection collection)
        {
            string password = collection["password"];
            if (password == "Iloveshrek<3")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        // GET: AdminController/Order
        public ActionResult Order()
        {
            return View();
        }


        // GET: AdminController/Refill
        public ActionResult Refill()
        {
            List<StoreFront> allStores = _bl.GetALLStoreFront();
            return View(allStores);
        }

        // GET: AdminController/Select/5
        public ActionResult Select(int id)
        {
            Response.Cookies.Append("CurrentStoreId", id.ToString());
            StoreFront temp = _bl.GetStoreFrontById(id);
            return View(temp.Inventories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // GET: AdminController/Update
        public ActionResult Update(IFormCollection orderNum)
        {

            int storeId = Int32.Parse(Request.Cookies["CurrentStoreId"]);
            StoreFront store = _bl.GetStoreFrontById(storeId);
            List<int> adds = new List<int>();
            for (int i = 1; i <= 4; i++)
            {
                int left = store.Inventories[i - 1].Quantity;
                int add;
                bool parseSuccess = Int32.TryParse(orderNum[i.ToString()], out add);
                if (add < 0)
                {
                    return RedirectToAction("Refill");
                }
                if (parseSuccess)
                {
                    adds.Add(add);
                }
                else
                {
                    adds.Add(0);
                }
            }
            for (int i = 1; i <= 4; i++)
            {
                int add = adds[i - 1];
                store.Inventories[i - 1].Quantity += add;
                _bl.UpdateInventory(store.Inventories[i - 1], store.Id);
            }
            Dictionary<string, string> password = new Dictionary<string, string>();
            password.Add("password", "IloveShrek<3");
            return RedirectToAction("Menu", password);
        }

        // GET: AdminController/Select
        public ActionResult Search(IFormCollection orderTypeAndId)
        {
            int id;
            if(orderTypeAndId["cp"] != "")
            {
                bool parseSuccess = Int32.TryParse(orderTypeAndId["cp"], out id);
                if (parseSuccess)
                {
                    List<Order> temp = _bl.GetAllOrdersByCustomerByCost(id);
                    return View(temp);
                }
            }
            if(orderTypeAndId["cd"] != "")
            {
                bool parseSuccess = Int32.TryParse(orderTypeAndId["cd"], out id);
                if (parseSuccess)
                {
                    List<Order> temp = _bl.GetAllOrdersByCustomerByDate(id);
                    return View(temp);
                }
            }
            if(orderTypeAndId["sp"] != "")
            {
                List<Order> temp = _bl.GetAllOrdersByStoreByCost(orderTypeAndId["sp"]);
                return View(temp);
            }
            if(orderTypeAndId["sd"] != "")
            {
                List<Order> temp = _bl.GetAllOrdersByStoreByDate(orderTypeAndId["sd"]);
                return View(temp);
            }
            return RedirectToAction("Order");
        }

    }
}
