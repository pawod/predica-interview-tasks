using task_2.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb;
using System.Threading.Tasks;

namespace task_2.Contracts
{
    public interface IDataTableService
    {
        Task<Notification[]> GetDataAsync(DTParameters table);
        Task<int> SetDataAsync(NotificationEntity[] entities);
        int TotalRecords();
    }
}