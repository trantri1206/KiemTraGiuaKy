using Abp.AspNetCore.Mvc.Authorization;
using _2380600189.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace _2380600189.Web.Controllers;

[AbpMvcAuthorize]
public class AboutController : _2380600189ControllerBase
{
    public ActionResult Index()
    {
        return View();
    }
}
