﻿namespace Alexandria.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ReviewsController : Controller
    {
        public IActionResult Create()
        {
            return this.View();
        }
    }
}