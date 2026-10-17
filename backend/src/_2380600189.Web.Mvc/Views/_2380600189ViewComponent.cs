using Abp.AspNetCore.Mvc.ViewComponents;

namespace _2380600189.Web.Views;

public abstract class _2380600189ViewComponent : AbpViewComponent
{
    protected _2380600189ViewComponent()
    {
        LocalizationSourceName = _2380600189Consts.LocalizationSourceName;
    }
}
