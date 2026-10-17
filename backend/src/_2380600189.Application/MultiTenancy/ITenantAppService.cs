using Abp.Application.Services;
using _2380600189.MultiTenancy.Dto;

namespace _2380600189.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

