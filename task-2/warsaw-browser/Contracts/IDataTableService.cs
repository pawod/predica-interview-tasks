using System.Threading.Tasks;
using JqueryDataTables.ServerSide.AspNetCoreWeb;
using task_2.Models;

namespace task_2.Contracts
{
    public interface IDataTableService
    {
        Task<Notification[]> GetDataAsync(DTParameters table);
        Task<int> SetDataAsync(NotificationEntity[] entities);
        int TotalRecords();
    }
}