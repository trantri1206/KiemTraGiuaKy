using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace _2380600189.Web.Views;

public abstract class _2380600189RazorPage<TModel> : AbpRazorPage<TModel>
{
    [RazorInject]
    public IAbpSession AbpSession { get; set; }

    protected _2380600189RazorPage()
    {
        LocalizationSourceName = _2380600189Consts.LocalizationSourceName;
    }
}
