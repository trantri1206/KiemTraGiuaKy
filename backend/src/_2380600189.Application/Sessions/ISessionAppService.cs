using Abp.Application.Services;
using _2380600189.Sessions.Dto;
using System.Threading.Tasks;

namespace _2380600189.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
