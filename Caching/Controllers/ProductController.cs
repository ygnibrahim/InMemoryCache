﻿using Caching.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Caching.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memortCache)
        {
            _memoryCache = memortCache;
        }
        public IActionResult Index()
        {

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            options.AbsoluteExpiration = DateTime.Now.AddSeconds(20);

            //options.SlidingExpiration = TimeSpan.FromSeconds(10);

            options.Priority = CacheItemPriority.High;

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value} => sebep:{reason}");
            });

            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);


            Product p = new Product { Id = 1, Name = "kalem", Price = 200 };

            _memoryCache.Set<Product>("product:1", p);



            return View();
        }

        public IActionResult Show()
        {

            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callback);

            ViewBag.product = _memoryCache.Get<Product>("product:1");

            ViewBag.zaman = zamancache;
            ViewBag.callback = callback;
            return View();
        }
    }
}
