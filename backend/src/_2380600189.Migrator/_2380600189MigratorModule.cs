using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using _2380600189.Configuration;
using _2380600189.EntityFrameworkCore;
using _2380600189.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace _2380600189.Migrator;

[DependsOn(typeof(_2380600189EntityFrameworkModule))]
public class _2380600189MigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public _2380600189MigratorModule(_2380600189EntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(_2380600189MigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            _2380600189Consts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(_2380600189MigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
