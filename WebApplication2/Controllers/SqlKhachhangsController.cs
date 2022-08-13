//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using RestSharp;
//using WebApplication2.APIs;
//using WebApplication2.Model;
//using static WebApplication2.APIs.DuongPhoAPI;

//namespace WebApplication2.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SqlKhachhangsController : ControllerBase
//    {
//        private readonly SqlDbContex _context;

//        public SqlKhachhangsController(SqlDbContex context)
//        {
//            _context = context;
//        }

//        // GET: api/SqlKhachhangs
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<SqlKhachhang>>> GetKhachHangs()
//        {
//          if (_context.KhachHangs == null)
//          {
//              return NotFound();
//          }
//            return await _context.KhachHangs.ToListAsync();
//        }


//        public class MyKhachHang
//        {
//            public int IDKH { get; set; }
//            public string DanhBo { get; set; } = "";
//            public string SDT { get; set; } = "";
//            public string TenKH { get; set; } = "";
//            public string DiaChi { get; set; } = "";
//            public string LoaiGia { get; set; } = "";
//            public string SerialModedule { get; set; } = "";
//            public string SeriaDH { get; set; } = "";
//            public string HieuDH { get; set; } = "";
//            public string KichCoDH { get; set; } = "";
//            public string ViTriDH { get; set; } = "";
//            public string Latitude { get; set; } = "";

//            public int SONK { get; set; }
//        }
//        private class MMsgKhachHang
//        {
//            public bool Error { get; set; } = false;
//            public string Message { get; set; } = "";
//            public string data { get; set; } = "";
//        }


     

//        [HttpGet]
//        [Route("getListKH")]
//        public async Task<IActionResult> getKhachHangAsync()
//        {

//            MKhachHang? mKhachHang = await Program.khachHang.GetKhachHangAsync();
//            MyKhachHang myKhachHang = new MyKhachHang();
//            if (mKhachHang != null)
//            {
//                myKhachHang.IDKH = mKhachHang.IDKH;
//                myKhachHang.DanhBo = mKhachHang.DanhBo;
//                myKhachHang.DiaChi = mKhachHang.DiaChi;
//                myKhachHang.KichCoDH = mKhachHang.KichCoDH;
//                myKhachHang.LoaiGia = mKhachHang.LoaiGia;
//                myKhachHang.SDT = mKhachHang.SDT;
//                myKhachHang.SeriaDH = mKhachHang.SeriaDH;
//                myKhachHang.SerialModedule = mKhachHang.SerialModedule;

//                myKhachHang.HieuDH = mKhachHang.HieuDH;
//                myKhachHang.KichCoDH = mKhachHang.KichCoDH;
//                myKhachHang.LoaiGia = mKhachHang.LoaiGia;
//                myKhachHang.SONK = mKhachHang.SONK;
//                return Ok(JsonConvert.SerializeObject(myKhachHang));
//            }

//            return BadRequest();
//        }
//        [HttpPost]
//        [Route("CreateKhachHang")]
//        public IActionResult khachhang([FromBody] MKhachHang myrequest)
//        {
           
//            var client = new RestClient("http://113.161.210.158:8992/api/values");
//            var request = new RestRequest();
//            request.Method = Method.Post;
//            request.AddHeader("Content-Type", "application/json; charset=utf-8");
//            request.RequestFormat = RestSharp.DataFormat.Json;
//            request.AddBody(myrequest);
//            request.Timeout = -1;

//            //Console.WriteLine(JsonConvert.SerializeObject(myrequest));
//            RestResponse response = client.Execute(request);
//            //Console.WriteLine(response.Content);
//            if (response.StatusCode == System.Net.HttpStatusCode.OK)
//            {
//                try
//                {
//                    MMsgKhachHang m_msg = JsonConvert.DeserializeObject<MMsgKhachHang>(response.Content);
//                    if (m_msg.Error == false)
//                    {
//                        string[] lst_text = m_msg.data.Split(':');
//                        if (lst_text.Count() > 1)
//                        {
//                            //Program.managerWareHouse.createDonHangFromLoadcell(lst_text[1].Trim(), myrequest.TenSanPham, myrequest.TenLoaiBao, myrequest.SoBao, myrequest.TenDVVC, myrequest.Cmnd, myrequest.SoXe, myrequest.SoroMooc, myrequest.Tenlaixe);
//                            return Ok(lst_text[1].Trim());
//                        }
//                        else
//                        {
//                            return BadRequest();
//                        }
//                    }
//                    else
//                    {
//                        return BadRequest();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    return BadRequest();
//                }
//            }
//            else
//            {
//                return BadRequest();
//            }
//        }


//        // DELETE: api/SqlKhachhangs/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteSqlKhachhang(int id)
//        {
//            if (_context.KhachHangs == null)
//            {
//                return NotFound();
//            }
//            var sqlKhachhang = await _context.KhachHangs.FindAsync(id);
//            if (sqlKhachhang == null)
//            {
//                return NotFound();
//            }

//            _context.KhachHangs.Remove(sqlKhachhang);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool SqlKhachhangExists(int id)
//        {
//            return (_context.KhachHangs?.Any(e => e.IDKH == id)).GetValueOrDefault();
//        }
//    }
//}
