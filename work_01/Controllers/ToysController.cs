using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using work_01.Models;
using work_01.Models.ViewModels;

namespace work_01.Controllers
{
    public class ToysController : Controller
    {
        private ToyDbContext db = new ToyDbContext();
        // GET: Toys
        public ActionResult Index()
        {
            return View(db.Toys.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.categories = new SelectList(db.Categories, "CId", "CName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ToyVM tvm)
        {
            if (ModelState.IsValid)
            {
                if (tvm.Picture != null)
                {
                    //for Image
                    string filePath = Path.Combine("~/Images", Guid.NewGuid().ToString() + Path.GetExtension(tvm.Picture.FileName));
                    tvm.Picture.SaveAs(Server.MapPath(filePath));

                    //Toys
                    Toy toys = new Toy
                    {
                        ToyName=tvm.ToyName,
                        CId=tvm.CId,
                        Price=tvm.Price,
                        StoreDate=tvm.StoreDate,
                        PicturePath=filePath
                    };
                    db.Toys.Add(toys);
                    db.SaveChanges();
                    return PartialView("_success");
                }
            }
            ViewBag.categories = new SelectList(db.Categories, "CId", "CName");
            return PartialView("_error");
        }
    }
}