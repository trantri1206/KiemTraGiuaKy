using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using _2380600189.EntityFrameworkCore;
using _2380600189.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace _2380600189.Web.Tests;

[DependsOn(
    typeof(_2380600189WebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class _2380600189WebTestModule : AbpModule
{
    public _2380600189WebTestModule(_2380600189EntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(_2380600189WebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(_2380600189WebMvcModule).Assembly);
    }
}