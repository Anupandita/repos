using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.UserRbac.Test.Integrated.TestData
{
    public class LoginTestData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class Content
    {
        public const string ContentType = "application/json";

    }

    public class UrlTestData
    {
        public string Login { get; set; }
        public string Menus { get; set; }
        public string Preferences { get; set; }
        public string Permissions { get; set; }

    }

    public class Headers
    {
        public const string Authorization = "Authorization";
        public const string Bearer = "Bearer ";
    }



}  