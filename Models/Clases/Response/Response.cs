using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.Response
{
    public class Response
    {
        public string status { get; set; }

        public Result result { get; set; }

    }
    public class Result
    {
        public string error_id { get; set; }

        public string error_msg { get; set; }
    }

    public class Successfull
    {
        public string token { get; set; }
    }
}