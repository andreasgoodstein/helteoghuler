using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class IndexModel : PageModel
    {
        private const string BEER_COUNT_KEY = "BeerCount";

        public int BeerCount;

        public void OnGet()
        {
            BeerCount = HttpContext.Session.GetInt32(BEER_COUNT_KEY) ?? 0;
        }

        public IActionResult OnPost()
        {
            BeerCount = HttpContext.Session.GetInt32(BEER_COUNT_KEY) ?? 0;

            BeerCount += 1;

            HttpContext.Session.SetInt32(BEER_COUNT_KEY, BeerCount);

            System.Console.WriteLine("{0} BeerCount: {1}", HttpContext.Session.Id, BeerCount);

            return RedirectToPage("Index");
        }
    }
}