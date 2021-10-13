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
    public class CustomerController : Controller
    {
        private IBL _bl;
        public CustomerController(IBL bl)
        {
            _bl = bl;
        }
        // GET: CustomerController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Customer cust)
        {
            cust = _bl.GetCustomerByPhone(cust.PhoneNum);
            if (cust.Id == 0)
            {
                return View();
            }
            else
            {
                Response.Cookies.Append("CurrentUserId", cust.Id.ToString());
                return RedirectToAction("Index", "Order");
            }
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer cust)
        {
            Customer temp = _bl.GetCustomerByPhone(cust.PhoneNum);
            if(temp.Id == 0)
            {
                _bl.AddNewCustomer(cust);
                cust = _bl.GetCustomerByPhone(cust.PhoneNum);
                Response.Cookies.Append("CurrentUserId", cust.Id.ToString());
                return RedirectToAction("Index", "Order");
            }
            else
            {
                return View();
            }
        }

        
    }
}
