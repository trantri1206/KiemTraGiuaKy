using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using _2380600189.Authorization;

namespace _2380600189;

[DependsOn(
    typeof(_2380600189CoreModule),
    typeof(AbpAutoMapperModule))]
public class _2380600189ApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<_2380600189AuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(_2380600189ApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(thisAssembly)
        );
    }
}
