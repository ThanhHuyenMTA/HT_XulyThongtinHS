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
            ViewBag.ctHocSinh = hocsinh.TenHS;
            CMT cmt = db.CMTs.SingleOrDefault(n => n.SoCMT == hocsinh.SoCMT);
            return View(cmt);
        }
        #endregion

        #region Danh sách lỗi được thông báo
        [HttpGet]
        public ActionResult Danhsachloi()
        {
            //xu li lỗi sai khác trong thông tin học sinh
            Hoten();
            return View(db.TABLE_LOI.ToList());
        }
        #endregion

        #region hàm kiem tra loi
        public void Hoten()
        {
            List<HOCSINH> ListHs = db.HOCSINHs.ToList();
            List<CMT> listCMT = db.CMTs.ToList();
            List<HOCBA> listHB = db.HOCBAs.ToList();
            List<BANGTOTNGHIEP> listBTN = db.BANGTOTNGHIEPs.ToList();
            List<GIAYKHAISINH> listGKS = db.GIAYKHAISINHs.ToList();
            var model = from HS in ListHs
                        join cmt in listCMT on HS.SoCMT equals cmt.SoCMT
                        join hb in listHB on HS.id_HB equals hb.id
                        join btn in listBTN on HS.id_BTN equals btn.id
                        join gks in listGKS on HS.id_GKS equals gks.id
                        select new
                        {
                            id_hs = HS.id,
                            tenHs = HS.TenHS,
                            soCmt = HS.SoCMT,
                            tenCmt = cmt.HoTen,
                            id_hb = HS.id_HB,
                            tenHb = hb.HoTen,
                            id_btn = HS.id_BTN,
                            tenBtn = btn.HoTen,
                            id_gks = HS.id_GKS,
                            tenGks = gks.HoTen
                        };
            foreach (var i in model.ToList())
            {
                TABLE_LOI tb_loi = new TABLE_LOI();
                if (i.tenHs != i.tenCmt)
                {
                    tb_loi.id_HS = i.id_hs;
                    tb_loi.id_CMT = i.soCmt;
                }
                if (i.tenHs != i.tenHb)
                {
                    tb_loi.id_HS = i.id_hs;
                    tb_loi.id_HB = i.id_hb;
                }
                if (i.tenHs != i.tenBtn)
                {
                    tb_loi.id_HS = i.id_hs;
                    tb_loi.id_BTN = i.id_btn;
                }
                if (i.tenGks != i.tenHs)
                {
                    tb_loi.id_HS = i.id_hs;
                    tb_loi.id_GKS = i.id_gks;
                }
                if (tb_loi.id_HS != null)
                {
                    tb_loi.TyPe = "HoTen";
                    tb_loi.TrangThai = 1;
                    tb_loi.TimeStart = DateTime.Now;
                    DateTime start = (DateTime)tb_loi.TimeStart;
                    DateTime end = start.AddDays(5);
                    tb_loi.TimeEnd = end;
                    db.TABLE_LOI.Add(tb_loi);
                    db.SaveChanges();
                }
            }
        }
        #endregion
    }
}