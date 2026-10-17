using Abp.Authorization;
using Abp.Runtime.Session;
using _2380600189.Configuration.Dto;
using System.Threading.Tasks;

namespace _2380600189.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : _2380600189AppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
