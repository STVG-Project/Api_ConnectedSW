using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;
using WebApplication2.Model;

namespace WebApplication2.APIs
{
    public class DuongPhoAPI
    {




        public class ItemDuongPho
        {
            public string MADP { get; set; } = "";
            public string TENDP { get; set; } = "";
        }
        public class ListDuongPho
        {
            public List<ItemDuongPho> duongPhos { get; set; } = new List<ItemDuongPho>();
        }
        public class MMsgDuongPho
        {
            public int resultCode { get; set; } = 0;
            public string resultMessage { get; set; } = "";
            public ListDuongPho data { get; set; } = new ListDuongPho();
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
        }

        private RequestObject my_requestdp;
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
        public DuongPhoAPI()
        {
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            string usermd5;
            string usernamemd5;

            usermd5 = string.Format("{0}:{1:00}:{2:0000}", "123", month, year);
            usernamemd5 = String.Format("{0}:{1}:{2:00}:{3:0000}", "DucHung", "dh.123", month, year);

            my_requestdp = new RequestObject();
            //requestType
            my_requestdp.requestType = "getDuongPho";
            //LoginInfo
            my_requestdp.loginInfo.userIdentity.userName = "User1";
            my_requestdp.loginInfo.userIdentity.passWord = "123";
            my_requestdp.loginInfo.userIdentity.checkSum = $"{createMD5(usermd5)}";

            my_requestdp.loginInfo.serviceClientIdentity.userName = "DucHung";
            my_requestdp.loginInfo.serviceClientIdentity.passWord = "dh.123";
            my_requestdp.loginInfo.serviceClientIdentity.checkSum = $"{createMD5(usernamemd5)}";
        }
        public async Task<ListDuongPho?> GetDuongPhoAsync()
        {
            var client = new RestClient("http://113.161.210.158:8992/api/values");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(JsonConvert.SerializeObject(my_requestdp));

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
                    MMsgDuongPho? m_msg = JsonConvert.DeserializeObject<MMsgDuongPho>(response.Content!);
                    if (m_msg == null)
                    {
                        return null;
                    }
                    if (m_msg.resultCode == 0)
                    {
                        using (SqlDbContex context = new SqlDbContex())
                        {
                            foreach (ItemDuongPho item in m_msg.data.duongPhos)
                            {
                                SqlTuyenDuong? tmp = context.Tuyenduongs!.Where(s => s.madp.CompareTo(item.MADP) == 0).FirstOrDefault();
                                if (tmp == null)
                                {
                                    SqlTuyenDuong new_item = new SqlTuyenDuong();
                                    new_item.ID = DateTime.Now.Ticks;
                                    new_item.madp = item.MADP;
                                    new_item.tendp = item.TENDP;
                                    context.Tuyenduongs!.Add(new_item);
                                }
                                else
                                {
                                    tmp.tendp = item.TENDP;
                                }
                                await context.SaveChangesAsync();
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

