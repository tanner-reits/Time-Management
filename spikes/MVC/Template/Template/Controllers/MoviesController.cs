﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template.Models;

namespace Template.Controllers
{
    public class MoviesController : Controller
    {
        // Get: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie { Name = "Shrek" };
            return View(movie);
        }
    }
}
