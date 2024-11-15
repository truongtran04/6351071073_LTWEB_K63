using MvcBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using PagedList.Mvc;

namespace MvcBookStore.Controllers
{
    public class BookStoreController : Controller
    {
        private QLBANSACHEntities db = new QLBANSACHEntities();
        // GET: BookStore

        private List<SACH> LaySacMoi(int count)
        {
            return db.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);
            var sachmoi = LaySacMoi(10);
            return View(sachmoi.ToPagedList(pageNum,pageSize));
        }

        public ActionResult ChuDe()
        {
            var chude = db.CHUDEs.ToList();
            return View(chude);
        }

        public ActionResult NhaXuatBan()
        {
            var chude = db.NHAXUATBANs.ToList();
            return View(chude);
        }

        public ActionResult SPTheoChuDe(int id)
        {
            var sach = db.SACHes.Where(c => c.MaCD == id).ToList();
            return View(sach);
        }
        public ActionResult SPTheoNXB(int id)
        {
            var sach = db.SACHes.Where(c => c.MaNXB == id).ToList();
            return View(sach);
        }

        public ActionResult Details(int id)
        {
            var sach = db.SACHes.Where(c => c.Masach == id).ToList();
            return View(sach.Single());
        }
    }
}