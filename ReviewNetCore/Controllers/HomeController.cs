using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReviewNetCore.Models;

namespace ReviewNetCore.Controllers
{
    public class HomeController : Controller
    {
        MyStoryDBContext db = new MyStoryDBContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewStories()
        {
            var allStories = db.Stories.ToList();
            return View(allStories);
        }
        public IActionResult Delete(int id)
        {
            var story = db.Stories.SingleOrDefault(s => s.StoryId == id);
            db.Stories.Remove(story);
            db.SaveChanges();
            return RedirectToAction("Index");
            
        }
       
       
        public async Task<IActionResult> DoCreate(IFormFile postedFile, Story story)
        {
            //1. update the picture into story(entity)
            using (var dataStream = new MemoryStream())
            {
                await postedFile.CopyToAsync(dataStream);
                story.Picture = dataStream.ToArray();
            }
            //save the entity to database by using EF
            db.Stories.Add(story);
            db.SaveChanges();
            //redirect user to the index
            return RedirectToAction("Index");


        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
