using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using JqueryDataTables.ServerSide.AspNetCoreWeb;
using Microsoft.EntityFrameworkCore;
using task_2.Contracts;
using task_2.Models;

namespace task_2.Services
{
    public class DataTableService : IDataTableService
    {
        private readonly MemoryDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _mappingConfiguration;

        public DataTableService(MemoryDbContext context,
                                AutoMapper.IConfigurationProvider mappingConfiguration)
        {
            this._context = context;
            this._mappingConfiguration = mappingConfiguration;
        }

        public async Task<int> SetDataAsync(NotificationEntity[] entities)
        {
            this._context.Notifications.AddRange(entities);
            return await this._context.SaveChangesAsync();
        }

        public async Task<Notification[]> GetDataAsync(DTParameters table)
        {
            IQueryable<NotificationEntity> query = _context.Notifications;
            query = new SearchOptionsProcessor<Notification, NotificationEntity>().Apply(query, table.Columns);
            query = new SortOptionsProcessor<Notification, NotificationEntity>().Apply(query, table);

            return await query
            .AsNoTracking()
            .Skip(table.Start - 1 * table.Length)
            .Take(table.Length)
            .ProjectTo<Notification>(_mappingConfiguration)
            .ToArrayAsync();           
        }

        public int TotalRecords()
        {
            return this._context.Notifications.Count();
        }
    }
}