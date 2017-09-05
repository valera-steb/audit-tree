using System.Threading.Tasks;

namespace AuditTree.Core.RequestInfo
{
    public interface IRequestInfoService
    {
        string GetCurrentRequestId();
        string AddRequestId();

        void StoreInRequest<T>(string key, T data);
        Task GetFromRequest<T>(string key);
    }
}
