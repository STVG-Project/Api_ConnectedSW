using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;
using WebApplication2.Model;

namespace WebApplication2.APIs
{
    public class KhachHangAPI
    {

        public class MKhachHang
        {
            public int IDKH { get; set; }
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
        }
        public class ListKhachHang
        {
            public List<MKhachHang> khachHangs { get; set; } = new List<MKhachHang>();
        }
        public class MMsgKhachHang
        {
            public int resultCode { get; set; } = 0;
            public string resultMessage { get; set; } = "";
            public ListKhachHang data { get; set; } = new ListKhachHang();
        }

        public class KhachHangRequest
        {
            public string MADP { get; set; } = "";
            public string checkSum { get; set; } = "";
        }

        public class Identity
        {
            public string userName { get; set; } = "";
            public string passWord { get; set; } = "";
            public string checkSum { get; set; } = "";
        }


        public class LoginInfo
        {
            public Identity serviceClientIdentity = new Identity();
            public Identity userIdentity = new Identity();
        }

        public class RequestObject
        {
            public LoginInfo loginInfo = new LoginInfo();
            public string requestType { get; set; } = "";

            public KhachHangRequest data { get; set; } = new KhachHangRequest();
        }

        private RequestObject my_requestkh;
        public string createMD5(string value)
        {
            MD5 mh = MD5.Create();


            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes($"{value}");
            byte[] hash = mh.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public KhachHangAPI()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            string usermd5;
            string usernamemd5;
            string MADPmd5;
            usermd5 = string.Format("{0}:{1:00}:{2:0000}", "123", month, year);
            usernamemd5 = String.Format("{0}:{1}:{2:00}:{3:0000}", "DucHung", "dh.123", month, year);
            MADPmd5 = string.Format("{0}:{1:00}:{2:0000}", "070", month, year);

            my_requestkh = new RequestObject();

            //requestType
            my_requestkh.requestType = "getKhachHang";
            my_requestkh.loginInfo.userIdentity.userName = "User1";
            my_requestkh.loginInfo.userIdentity.passWord = "123";
            my_requestkh.loginInfo.userIdentity.checkSum = $"{createMD5(usermd5)}";

            //LoginInfo
            my_requestkh.loginInfo.serviceClientIdentity.userName = "DucHung";
            my_requestkh.loginInfo.serviceClientIdentity.passWord = "dh.123";
            my_requestkh.loginInfo.serviceClientIdentity.checkSum = $"{createMD5(usernamemd5)}";
            //data
            my_requestkh.data.MADP = "070";
            my_requestkh.data.checkSum = $"{createMD5(MADPmd5)}";

        }

        public async Task<ListKhachHang?> GetKhachHangAsync()
        {
            var client = new RestClient("http://113.161.210.158:8992/api/values");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(JsonConvert.SerializeObject(my_requestkh));

            request.Timeout = -1;
            RestResponse response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    if (response.Content == null)
                    {
                        return null;
                    }
                    MMsgKhachHang? m_msg = JsonConvert.DeserializeObject<MMsgKhachHang>(response.Content!);
                    if (m_msg == null)
                    {
                        return null;
                    }
                    if (m_msg.resultCode == 0)
                    {
                        using (SqlDbContex contex = new SqlDbContex())
                        {
                            foreach (var item in m_msg.data.khachHangs)
                            {
                                SqlKhachhang? tmp = contex.KhachHangs!.Where(s => s.IDKH.CompareTo(item.IDKH) == 0).FirstOrDefault();

                                if (tmp == null)
                                {
                                    SqlKhachhang new_item = new SqlKhachhang();
                                    new_item.IDKH = item.IDKH;
                                    new_item.SDT = item.SDT;
                                    new_item.TenKH = item.TenKH;
                                    new_item.DanhBo = item.DanhBo;
                                    new_item.DiaChi = item.DiaChi;
                                    new_item.Latitude = item.Latitude;
                                    new_item.LoaiGia = item.LoaiGia;
                                    new_item.SeriaDH = item.SeriaDH;
                                    new_item.ViTriDH = item.ViTriDH;
                                    new_item.SerialModedule = item.SerialModedule;
                                    new_item.SONK = item.SONK;
                                    new_item.KichCoDH = item.KichCoDH;
                                    new_item.HieuDH = item.HieuDH;
                                    SqlTuyenDuong? sqlTuyenDuong = contex.Tuyenduongs!.Where(s => s.madp.CompareTo("070") == 0).FirstOrDefault();
                                    if (sqlTuyenDuong != null)
                                    {

                                        if (sqlTuyenDuong.KhachHangs == null)
                                        {
                                            sqlTuyenDuong.KhachHangs = new List<SqlKhachhang>();
                                        }
                                        sqlTuyenDuong.KhachHangs.Add(new_item);
                                    }
                                    contex.KhachHangs!.Add(new_item);
                                }
                                else
                                {
                                    tmp.SDT = item.SDT;
                                    tmp.TenKH = item.TenKH;
                                    tmp.DanhBo = item.DanhBo;
                                    tmp.DiaChi = item.DiaChi;
                                    tmp.Latitude = item.Latitude;
                                    tmp.LoaiGia = item.LoaiGia;
                                    tmp.SeriaDH = item.SeriaDH;
                                    tmp.ViTriDH = item.ViTriDH;
                                    tmp.SerialModedule = item.SerialModedule;
                                    tmp.SONK = item.SONK;
                                    tmp.KichCoDH = item.KichCoDH;
                                    tmp.HieuDH = item.HieuDH;
                                }
                                await contex.SaveChangesAsync();
                            }
                        }
                        return m_msg.data;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

    }
}
