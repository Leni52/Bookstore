using Bookstore.Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Bookstore.Auth.Controllers
{
    public class BooksMVCController : Controller
    {
        public async Task<IActionResult> Index()
        {
            IEnumerable<BookViewModel> books = Enumerable.Empty<BookViewModel>();

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            if (accessToken != null)
            {
                // You have obtained the access token, and you can use it in your HttpClient requests.
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7263/api/");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                    var response = await client.GetAsync("Books");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        books = JsonConvert.DeserializeObject<IEnumerable<BookViewModel>>(content);

                    }
                }
            }
            else
            {

            }

            return View(books);
        }


        [Route("/booksMVC/bookId")]
        public async Task<IActionResult> DeleteBook(Guid bookId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            if (accessToken != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7263/api/");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                    var response = await client.GetAsync("Books/bookId");

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("BooksMVC/Index");

                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Error deleting the book.");
            return View();
        }

    }
}
