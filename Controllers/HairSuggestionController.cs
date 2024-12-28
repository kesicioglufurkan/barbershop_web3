using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace barbershop_web3.Controllers
{
    public class HairSuggestionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HairSuggestionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.ErrorMessage = "Lütfen bir dosya seçin!";
                return View("Index");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                using var memoryStream = new MemoryStream();

                // Dosyayı belleğe yükle
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // Akışın başına dön

                var fileContent = new StreamContent(memoryStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                var requestContent = new MultipartFormDataContent();
                requestContent.Add(fileContent, "file", file.FileName);

                // Flask API'ye POST isteği gönder
                var response = await client.PostAsync("http://127.0.0.1:5000/analyze", requestContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);
                    ViewBag.FaceShape = data.face_shape;
                    ViewBag.SuggestionImage = data.suggestion;  // /static/saclar/oval.png gibi bir değer alıyoruz.
                    return View("Result");
                }
                else
                {
                    ViewBag.ErrorMessage = $"API isteği başarısız oldu! Durum Kodu: {response.StatusCode}";
                    return View("Index");
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Hata oluştu: {ex.Message}";
                return View("Index");
            }
        }



    }
}
