using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapLon
{
    public class NguoiChoi
    {
        public string HoTen { get; set; }
        public Image HinhAnh { get; set; }
        public int Diem { get; set; }
        public NguoiChoi(string hoten,Image hinhanh)
        {
            this.HoTen = hoten;
            this.HinhAnh = hinhanh;
            this.Diem = 0;
        }     
    }
}
