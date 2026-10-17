using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace _2380600189.Controllers
{
    public abstract class _2380600189ControllerBase : AbpController
    {
        protected _2380600189ControllerBase()
        {
            LocalizationSourceName = _2380600189Consts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
