using Bookstore.Auth.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bookstore.Auth.Controllers
{
    public class BooksMVCController : Controller
    {
        public async Task<IActionResult> Index()
        {
            IEnumerable<BookViewModel> books = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7263/api/");
                //HTTP GET
                var responseTask = client.GetAsync("Books");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var content = await responseTask.Result.Content.ReadAsStringAsync();


                    books = JsonConvert.DeserializeObject<IEnumerable<BookViewModel>>(content);

                }
            }
            return View(books);
        }
    }
}
