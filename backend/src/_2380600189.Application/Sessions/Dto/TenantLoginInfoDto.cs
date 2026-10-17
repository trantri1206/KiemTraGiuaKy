using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using _2380600189.MultiTenancy;

namespace _2380600189.Sessions.Dto;

[AutoMapFrom(typeof(Tenant))]
public class TenantLoginInfoDto : EntityDto
{
    public string TenancyName { get; set; }

    public string Name { get; set; }
}
