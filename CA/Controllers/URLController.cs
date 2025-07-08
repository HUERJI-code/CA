using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CA.Controllers
{
    public class URLController : Controller
    {
        [HttpGet("/URL/getImageUrls")]
        public List<string> getImageUrls(string url)  // 修正了方法名（原方法名有笔误）
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
                        if (string.IsNullOrEmpty(srcAttribute)) continue;

                        // 排除SVG图片和Data URLs
                        if (srcAttribute.StartsWith("data:", StringComparison.OrdinalIgnoreCase) ||
                            srcAttribute.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        try
                        {
                            var absoluteUrl = new Uri(new Uri(url), srcAttribute).ToString();

                            // 二次检查绝对URL路径（防止相对路径绕过）
                            if (!absoluteUrl.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                            {
                                imageUrls.Add(absoluteUrl);
                            }
                        }
                        catch (UriFormatException)
                        {
                            // 忽略无效的URL格式
                            continue;
                        }
                    }
                }

                // 返回结果处理
                if (imageUrls.Count >= 20)
                {
                    return imageUrls.Take(20).ToList();
                }
                else if (imageUrls.Count > 0)
                {
                    return imageUrls;
                }
                else
                {
                    return new List<string> { "No valid images found (excluding SVGs)" };
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