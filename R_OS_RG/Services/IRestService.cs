using Newtonsoft.Json;
using R_OS.Models;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
namespace R_OS_RG.Services
{
    public interface IRestService
    {
        void GenerateReport();
        Task<List<ContactInformation>> GetReportData(Guid uuid);
    }

    public class RestService : IRestService
    {
        //private readonly IConfigService _config;
        private readonly string _apiUrl = "https://localhost:5281/api/v1/";
        //IConfigService config
        public RestService()
        {
            //_config = config;

            //_apiUrl = _config.GetValue("API_URL");
        }

        public object JsonConvert { get; private set; }

        public async Task<List<ContactInformation>> GetReportData(Guid uuid)
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, errors) => true;

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var model = new ReportQueueModel()
            {
                ReportUUID = uuid
            };

            var options = new RestClientOptions(_apiUrl + "contactInfo")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second. or whatever time you want.
            };
            var client = new RestClient(options);
            var request = new RestRequest(Method.Get.ToString());
            request.AddHeader("Content-Type", "application/json");
            var response = await client.GetAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ContactInformation>>(response.Content);
                return result;
            }
            else
            {
                throw new Exception("An error occured");
            }
        }

        public void GenerateReport()
        {
            throw new NotImplementedException();
        }
    }
}
