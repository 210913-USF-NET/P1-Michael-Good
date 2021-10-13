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
    public class StoreFrontController : Controller
    {
        private IBL _bl;
        public StoreFrontController(IBL bl)
        {
            _bl = bl;
        }
        // GET: StoreFrontController
        public ActionResult Index()
        {
            List<StoreFront> allStores = _bl.GetALLStoreFront();
            return View(allStores);
        }

        // GET: StoreFrontController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoreFrontController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StoreFront store)
        {
                if(ModelState.IsValid)
                {
                    _bl.AddNewStoreFront(store);
                    return RedirectToAction("Menu", "Admin");
            }
                return View();
        }

        // GET: StoreFrontController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StoreFrontController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StoreFrontController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StoreFrontController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
