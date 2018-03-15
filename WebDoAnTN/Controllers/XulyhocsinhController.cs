using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDoAnTN.Models;

namespace WebDoAnTN.Controllers
{
    
    public class XulyhocsinhController : Controller
    {
        dbXulyTThsEntities db = new dbXulyTThsEntities();
        // GET: Xulyhocsinh
        #region Chi tiết chứng minh thư
        [HttpGet]  
        public ActionResult ChitietCMT(int id_hs)
        {
            
            HOCSINH hocsinh = db.HOCSINHs.SingleOrDefault(n => n.id == id_hs);
            Session["cthocsinh"] = hocsinh.TenHS;
            CMT cmt = db.CMTs.SingleOrDefault(n => n.SoCMT == hocsinh.SoCMT);
            return View(cmt);
        }
        #endregion

        #region Danh sách lỗi được thông báo
        [HttpGet]
        public ActionResult Danhsachloi()
        {
            return View();
        }
        #endregion

    }
}