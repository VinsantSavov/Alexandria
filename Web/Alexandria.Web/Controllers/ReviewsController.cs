﻿namespace Alexandria.Web.Controllers
{
    using Alexandria.Data.Models.Enums;
    using Microsoft.AspNetCore.Mvc;

    public class ReviewsController : Controller
    {
        public IActionResult Create(int id)
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(string description, ReadingProgress readingProgress, bool thisEdition)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.RedirectToAction("Create");
        }
    }
}
