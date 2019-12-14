using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDataService
    {
        Task<bool> Authenticate(LoginDetails loginDetails);
        void Logout();
        Task<IList<Server>> GetServerList();
    }
}