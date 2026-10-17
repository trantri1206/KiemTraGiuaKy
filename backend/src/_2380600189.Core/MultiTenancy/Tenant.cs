using Abp.MultiTenancy;
using _2380600189.Authorization.Users;

namespace _2380600189.MultiTenancy;

public class Tenant : AbpTenant<User>
{
    public Tenant()
    {
    }

    public Tenant(string tenancyName, string name)
        : base(tenancyName, name)
    {
    }
}
