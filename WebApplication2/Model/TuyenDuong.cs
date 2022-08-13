//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Model
{

    [Table("tb_tuyenduong")]

    public class SqlTuyenDuong
    {
        [Key]
        public long ID { get; set; }
        public string madp { get; set; } = "";
        public string tendp { get; set; } = "";
        public List<SqlKhachhang>? KhachHangs { get; set; }

    }

}
