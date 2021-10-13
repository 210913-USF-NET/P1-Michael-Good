using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBL;
using Models;

namespace WebUI.Controllers
{
    public class OrderController : Controller
    {
        private IBL _bl;
        public OrderController(IBL bl)
        {
            _bl = bl;
        }
        // GET: OrderController
        public ActionResult Index()
        {
            List<StoreFront> allStores = _bl.GetALLStoreFront();
            return View(allStores);
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection orderNum)
        {
            decimal total = 0;
            int storeId = Int32.Parse(Request.Cookies["CurrentStoreId"]);
            StoreFront store = _bl.GetStoreFrontById(storeId);
            int customerId = Int32.Parse(Request.Cookies["CurrentUserId"]);
            Customer cust = _bl.GetCustomerByID(customerId);
            DateTime thisday = DateTime.Today;
            List<int> wants = new List<int>();
            for (int i = 1; i <= 4; i++)
            {
                int left = store.Inventories[i-1].Quantity;
                int want;
                bool parseSuccess = Int32.TryParse(orderNum[i.ToString()], out want);
                if(left < want || want < 0)
                {
                    return RedirectToAction("Index");
                }
                if(parseSuccess)
                {
                    wants.Add(want);
                }
                else
                {
                    wants.Add(0);
                }
            }
            List<OrderLine> orderlines = new List<OrderLine>();
            for (int i = 1; i <= 4; i++)
            {
                int want = wants[i-1];
                OrderLine ol = new OrderLine()
                {
                    Item = store.Inventories[i-1].Item,
                    Quantity = want
                };
                total += (ol.Item.Price * ol.Quantity);
                orderlines.Add(ol);
                store.Inventories[i-1].Quantity = store.Inventories[i-1].Quantity - want;
                _bl.UpdateInventory(store.Inventories[i-1], store.Id);
            }
            Order order = new Order()
            {
                CustId = customerId,
                StoreAddress = store.Address,
                DateOfOrder = thisday.ToString(),
                Cust = cust,
                Total = total,
                OrderItems = orderlines
            };
            _bl.SendOrder(order);

            return RedirectToAction("Index", "Home");
        }

        // GET: OrderController/Select/5
        public ActionResult Select(int id)
        {
            Response.Cookies.Append("CurrentStoreId", id.ToString());
            StoreFront temp = _bl.GetStoreFrontById(id);  
            return View(temp.Inventories);
        }

    }
}