using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGrabDemo.Grab
{
    public class Position
    {
        private static MovieInfoHelper hotMovieList = new MovieInfoHelper(Path.Combine(GlobalConfig.WWWRootPath, "hotMovie.json"));

        private static HtmlParser htmlParser = new HtmlParser();

    }
}
