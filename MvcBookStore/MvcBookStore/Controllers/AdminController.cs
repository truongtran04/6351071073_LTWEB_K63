using MvcBookStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MvcBookStore.Controllers
{
    public class AdminController : Controller
    {
        QLBANSACHEntities db = new QLBANSACHEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tenDN = collection["username"];
            var matKhau = collection["password"];

            if (String.IsNullOrEmpty(tenDN))
            {
                ViewData["Loi1"] = "Vui lòng nhập tên tài khoản";
            }
            else if (String.IsNullOrEmpty(matKhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            }
            else
            {
                Admin admin = db.Admins.SingleOrDefault(ad => ad.UserAdmin == tenDN && ad.PassAdmin == matKhau);

                if (admin != null)
                {
                    Session["TaiKhoanAdmin"] = admin;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        public ActionResult ChuDe()
        {
            return View(db.CHUDEs.ToList());
        }

        public ActionResult NhaXB()
        {
            return View(db.NHAXUATBANs.ToList());
        }

        public ActionResult Sach(int? page)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);
            return View(db.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoiSach()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(cd => cd.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(cd => cd.TenNXB), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiSach(SACH s, HttpPostedFileBase fileUpload)
        {

            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(cd => cd.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(cd => cd.TenNXB), "MaNXB", "TenNXB");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    s.Anhbia = fileName;
                    db.SACHes.Add(s);
                    db.SaveChanges();
                }
                return RedirectToAction("Sach");
            }
        }

        public ActionResult ChiTietSach(int? id)
        {
            SACH sach = db.SACHes.SingleOrDefault(s => s.Masach == id);
            ViewBag.MaSach = sach.Masach;

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        public ActionResult XoaSach(int? id)
        {
            SACH sach = db.SACHes.SingleOrDefault(s => s.Masach == id);
            ViewBag.MaSach = sach.Masach;

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("XoaSach")]
        public ActionResult XacNhanXoa(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(s => s.Masach == id);
            ViewBag.MaSach = sach.Masach;

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACHes.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Sach");
        }

        [HttpGet]
        public ActionResult SuaSach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(s => s.Masach == id);

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(cd => cd.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(cd => cd.TenNXB), "MaNXB", "TenNXB");
            return View(sach);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSach(SACH s, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(cd => cd.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(cd => cd.TenNXB), "MaNXB", "TenNXB");
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View(s);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                        return View(s);
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    s.Anhbia = fileName;
                    db.Entry(s).State = (System.Data.Entity.EntityState)EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Sach");
            }
        }

        public ActionResult ThongKe()
        {
            var booksByCategory = db.SACHes
            .GroupBy(s => s.CHUDE.TenChuDe)  // Assuming `Tenchude` is the category name in CHUDE
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .ToList();

            ViewBag.ChartLabels = booksByCategory.Select(b => b.Category).ToArray();
            ViewBag.ChartData = booksByCategory.Select(b => b.Count).ToArray();

            return View();
        }
    }
}