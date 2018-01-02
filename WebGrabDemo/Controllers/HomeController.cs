using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebGrabDemo.Helper;
using WebGrabDemo.Models;
using WebGrabDemo.Service;

namespace WebGrabDemo.Controllers
{
    public class HomeController : Controller
    {

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="isRefresh"></param>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<MovieInfo> lstMovie = MovieService.GetAllMovieInfo();
            return View(lstMovie);
        }

        public IActionResult ShowMoiveDetail(string onlineURL)
        {
            var movieInfo = MovieService.GetMovieInfoFromOnlineUrl(onlineURL, true);
            return View(movieInfo);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
