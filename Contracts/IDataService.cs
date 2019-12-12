using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDataService
    {
        Task Authenticate(LoginDetails loginDetails);
        Task<IList<Server>> GetServerList();
    }
}