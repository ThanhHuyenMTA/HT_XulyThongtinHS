﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoAnTN.Models;

namespace WebDoAnTN.Controllers
{
    public class QuanlyhocsinhController : Controller
    {
       dbXulyTThsEntities  db = new dbXulyTThsEntities();
       Xuly xulyLoi = new Xuly();
        // GET: Quanlyhocsinh
        public ActionResult Index()
        {
            return View();
        }
        #region Thêm học sinh 
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
                if(hocsinh.TenHS!=null)
                {
                    db.HOCSINHs.Add(hocsinh);
                    db.SaveChanges();
                    Session["hocsinh"] = hocsinh.TenHS;
                    Session["id_hs"] = hocsinh.id;
                    return RedirectToAction("Index");
                }
                return View(hocsinh);
            }
            return View(hocsinh);
        }

        #endregion

        #region Chứng minh thư
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
                if(xulyLoi.CheckString(hs.TenHS,cmt.HoTen)==false){
                    Session["Loi"] = "Họ tên giữa Học sinh và chứng minh thư có sự khác nhau";
                }
                else { Session["Loi"] = "Đúng"; }
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
                db.SaveChanges();
                //Tự động add thêm thông tin GKS vào bảng HOCSINH
                int id_hs = (int)Session["id_hs"];
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id == id_hs);  
                //dua ra giay khai sinh vua them vào (nam ơ vi tri cuoi cung)
                GIAYKHAISINH newG = db.GIAYKHAISINHs.ToList().Last();
                hs.id_GKS = newG.id;
                //Lưu lại dữ liệu bảng Học Sinh
                db.Entry(hs).State = System.Data.Entity.EntityState.Modified;
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
                db.SaveChanges();
                //Tự động add thêm thông tin bangtotnghiep vào bảng HOCSINH
                int id_hs = (int)Session["id_hs"];
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id == id_hs);                
                //dua ra giay khai sinh vua them vào (nam ơ vi tri cuoi cung)
                BANGTOTNGHIEP newB = db.BANGTOTNGHIEPs.ToList().Last();
                hs.id_BTN = newB.id;
                //Lưu lại dữ liệu bảng Học Sinh
                db.Entry(hs).State = System.Data.Entity.EntityState.Modified;
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
        public JsonResult XulyHocba(HOCBA hocba)
        {
            if(ModelState.IsValid)
            {
                 db.HOCBAs.Add(hocba);
                 db.SaveChanges();
                //Tự động add thêm thông tin hocba vào bảng HOCSINH
                int id_hs = (int)Session["id_hs"];
                HOCSINH hs = db.HOCSINHs.SingleOrDefault(n => n.id == id_hs);
                HOCBA NewH = db.HOCBAs.ToList().Last();
                hs.id_HB = NewH.id;
                //Lưu lại dữ liệu bảng Học Sinh
                db.Entry(hs).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(hocba,JsonRequestBehavior.AllowGet);
            }
            return  Json(hocba,JsonRequestBehavior.AllowGet);
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


        #region Uplooad_Image ... cần sửa lỗi
        [HttpPost]
        public JsonResult UploadImage()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {             
                var fileImg = Request.Files["HelpSectionImages"];
            
                //lưu tên file
                var fileName = Path.GetFileName(fileImg.FileName);
                //lưu đường dẫn
                var path = Path.Combine(Server.MapPath("~/Content/img/profile"), fileName);
                // file is uploaded
                var type = fileImg.ContentType;
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                }else{
                    if (type == "image/jpeg" || type == "image/jpg" || type == "image/png")
                        fileImg.SaveAs(path);                  
                }
                //dua ra hoc sinh can upload anh
                HOCSINH hs = db.HOCSINHs.ToList().Last();
                hs.anh = fileName;
                db.Entry(hs).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                //end
                return Json(fileName, JsonRequestBehavior.AllowGet);
           
            }
            return Json("Khong", JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Danh sách học sinh
        public ActionResult DanhsachHS()
        {           
            return View(db.HOCSINHs.ToList());
        }
        #endregion

        #region Search - học sinh
        [HttpPost]
        public ActionResult Search(string searchname)
        {
            List<HOCSINH> list_hs = new List<HOCSINH>();
            list_hs = db.HOCSINHs.Where(n => n.TenHS == searchname).ToList();
            return View(list_hs);
        }
        #endregion

      
    }
}