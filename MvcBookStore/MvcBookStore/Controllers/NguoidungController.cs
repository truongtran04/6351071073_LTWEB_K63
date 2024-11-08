using MvcBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBookStore.Controllers
{
    public class NguoidungController : Controller
    {
        private QLBANSACHEntities db = new QLBANSACHEntities();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(KHACHHANG kh, FormCollection collection)
        {
            var hoTen = collection["HoTenKH"];
            var tenDN = collection["TenDN"];
            var matKhau = collection["MatKhau"];
            var nhapLaiMK = collection["NhapLaiMK"];
            var email = collection["Email"];
            var diaChi = collection["DiaChi"];
            var dienThoai = collection["DienThoai"];
            var ngaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);

            if (String.IsNullOrEmpty(hoTen))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (String.IsNullOrEmpty(tenDN))
            {
                ViewData["Loi2"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(matKhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(nhapLaiMK))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(diaChi))
            {
                ViewData["Loi6"] = "Địa chỉ không được bỏ trống";
            }
            else if (String.IsNullOrEmpty(dienThoai))
            {
                ViewData["Loi7"] = "Điện thoại không được bỏ trống";
            }
            else
            {
                kh.HoTen = hoTen;
                kh.Taikhoan = tenDN;
                kh.Matkhau = matKhau;
                kh.Email = email;
                kh.DiachiKH = diaChi;
                kh.DienthoaiKH = dienThoai;
                kh.Ngaysinh = DateTime.Parse(ngaySinh);
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        public ActionResult DangNhap(FormCollection collection)
        {
            var tenDN = collection["TenDN"];
            var matKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tenDN))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(matKhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tenDN && n.Matkhau == matKhau);
                if (kh != null)
                {
                    //ViewBag.ThongBao = "Đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "BookStore");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
    }
}