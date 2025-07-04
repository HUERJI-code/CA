using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace CA.Controllers
{
    public class URLController : Controller
    {
        [HttpGet("/URL/getImageUrls")]

        //testUrl:"https://localhost:7262/home/getUrls?url=https://stocksnap.io/"
        public List<string> getUrgetImageUrls(string url)
        {
            List<string> imageUrls = new List<string>();
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/114.0.0.0 Safari/537.36");

                var response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string htmlContent = response.Content.ReadAsStringAsync().Result;

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);
                var imgNodes = htmlDocument.DocumentNode.SelectNodes("//img");

                if (imgNodes != null)
                {
                    foreach (var imgNode in imgNodes)
                    {
                        var srcAttribute = imgNode.GetAttributeValue("src", null);
                        if (!string.IsNullOrEmpty(srcAttribute))
                        {
                            var absoluteUrl = new Uri(new Uri(url), srcAttribute).ToString();
                            imageUrls.Add(absoluteUrl);
                        }
                    }
                }
                if (imageUrls.Count >= 20)
                {
                    return imageUrls.Take(20).ToList();
                }
                else
                {
                    return new List<string> { "Found only " + imageUrls.Count + " image(s); fewer than 20." };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting image URLs: {ex.Message}");
                return new List<string> { "Error occurred: " + ex.Message };
            }
        }

    }
}
