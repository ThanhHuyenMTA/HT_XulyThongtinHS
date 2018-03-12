using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoAnTN.Models;

namespace WebDoAnTN.Controllers
{
    public class QuanlyhocsinhController : Controller
    {
       dbXulyTThsEntities  db = new dbXulyTThsEntities();
        // GET: Quanlyhocsinh
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ThemHocsinh()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemHocsinh(HOCSINH hocsinh)
        {
            if (ModelState.IsValid)
            {
                db.HOCSINHs.Add(hocsinh);
                db.SaveChanges();
                Session["hocsinh"] = hocsinh.TenHS;
                Session["id_hs"] = hocsinh.id;
                return RedirectToAction("Index");

            }
            return View(hocsinh);
        }
        #region //Chứng minh thư
        [HttpGet]
        public ActionResult CMT()
        {
            return View();
        }
        public JsonResult LoadFormCMT()
        {
            return Json("", JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult themCMT(CMT cmt)
        {
            if (ModelState.IsValid)
            {
                int id_hs =(int)Session["id_hs"];
                db.CMTs.Add(cmt);
                //Tự động add thêm thông tin cmt vào bảng HOCSINH
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id == id_hs);
                hs.SoCMT = cmt.SoCMT;

                db.SaveChanges();
                return Json(cmt, JsonRequestBehavior.AllowGet);

            }
            return Json(cmt, JsonRequestBehavior.AllowGet);

        }
    #endregion //End Chứng minh thư

        #region Giấy khai sinh
        [HttpGet]
        public ActionResult ThemGiaykhaisinh()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemGiaykhaisinh(GIAYKHAISINH gks)
        {
            if(ModelState.IsValid)
            {
                db.GIAYKHAISINHs.Add(gks);
                //Tự động add thêm thông tin GKS vào bảng HOCSINH
                int id_hs = (int)Session["id_hs"];
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id == id_hs);
                hs.id_GKS = gks.id;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gks);
        }
        #endregion end Giay khai sinh

        #region Bằng tốt nghiệp

        [HttpGet]
        public ActionResult ThemBangtotnghiep()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemBangtotnghiep(BANGTOTNGHIEP bangTN)
        {
            if(ModelState.IsValid)
            {
                db.BANGTOTNGHIEPs.Add(bangTN);
                //Tự động add thêm thông tin bangtotnghiep vào bảng HOCSINH
                int id_hs = (int)Session["id_hs"];
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id == id_hs);
                hs.id_BTN = bangTN.id;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bangTN);
        }

        #endregion end Bằng tốt nghiệp

        #region Học bạ, năm học, kì học
        [HttpGet]
        public ActionResult ThemHocba()
        {
            return View();

        }
        [HttpPost]
        public ActionResult ThemHocba(HOCBA hocba)
        {
            if(ModelState.IsValid)
            {
                 db.HOCBAs.Add(hocba);
                //Tự động add thêm thông tin hocba vào bảng HOCSINH
                int id_hs = (int)Session["id_hs"];
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id == id_hs);
                hs.id_HB = hocba.id;
                db.SaveChanges();
                return RedirectToAction("Themnamhoc", "Quanlyhocsinh");
            }
            return View(hocba);
        }
        [HttpPost]
        public JsonResult ThemNamhoc(NAMHOC namhoc)
        {
            if (ModelState.IsValid)
            {
                db.NAMHOCs.Add(namhoc);
                db.SaveChanges();
                return Json(namhoc, JsonRequestBehavior.AllowGet);
            }
            return Json(namhoc, JsonRequestBehavior.AllowGet);
        }
        //Thông tin kì học (Nhập điểm)
        [HttpGet]
        public JsonResult ViewNhapDiem(int id) //id: id_namhoc tuong ung
        {
            Session["id_namhoc"] = id;
            return Json(id,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult NhapDiem(KIHOC kihoc)
        {
            if(ModelState.IsValid)
            {
                kihoc.id_NAMHOC =(int)Session["id_namhoc"];
                db.KIHOCs.Add(kihoc);
                db.SaveChanges();
                return Json(kihoc, JsonRequestBehavior.AllowGet);
            }
            return Json(kihoc, JsonRequestBehavior.AllowGet);
        }
        #endregion End Học bạ

        #region  Người giám hộ
        [HttpGet]
        public ActionResult ThemNguoigiamho()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemNguoigiamho(NGUOIGIAMHO ngh)
        {
            if(ModelState.IsValid)
            {
                ngh.id_HSinh = (int)Session["id_hs"];
                db.NGUOIGIAMHOes.Add(ngh);
                //Sau khi có thông tin học sinh và người giám hộ thì tự tạo hồ sơ HS
                HOSOH hosoHS = new HOSOH();
                hosoHS.id_HS = (int)Session["id_hs"];
                hosoHS.SoCMT_NGH = ngh.SoCMT;
                db.HOSOHS.Add(hosoHS);
                db.SaveChanges();

            }
            return View(ngh);
        }
         
        #endregion

    }
}