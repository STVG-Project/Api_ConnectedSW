using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Model
{

        [Table("tb_khachhang")]
        public class SqlKhachhang
        {
            [Key]
            public long IDKH { get; set; }
            public string DanhBo { get; set; } = "";
            public string SDT { get; set; } = "";
            public string TenKH { get; set; } = "";
            public string DiaChi { get; set; } = "";
            public string LoaiGia { get; set; } = "";
            public string SerialModedule { get; set; } = "";
            public string SeriaDH { get; set; } = "";
            public string HieuDH { get; set; } = "";
            public string KichCoDH { get; set; } = "";
            public string ViTriDH { get; set; } = "";
            public string Latitude { get; set; } = "";

            public int SONK { get; set; }
           public SqlTuyenDuong? TuyenDuong { get; set; }
           
        }

}
