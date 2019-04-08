using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodeQuiz01
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ModelBinders.Binders.DefaultBinder = new AbstractBinder();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
