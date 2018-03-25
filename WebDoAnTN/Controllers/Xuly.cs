using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDoAnTN.Models;

namespace WebDoAnTN.Controllers
{
    public class Xuly
    {
        dbXulyTThsEntities db = new dbXulyTThsEntities();
        //Hàm kiểm tra sai sót trong cơ sở dữ liệu (kiểm tra chuỗi và ktra dateTime)
        public bool CheckString(string name1, string name2)
        {
            if (name1 == name2)
                return true;
            else
                return false;
        }
        public bool CheckDatetime(DateTime time1, DateTime time2)
        {
            if (DateTime.Compare(time1, time2) == 0) //=0 chứng tỏ 2 ngày trùng nhau
                return true;
            else
                return false;
        }
      
    }
}