using task_2.Contracts;
using task_2.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JqueryDataTables.ServerSide.AspNetCoreWeb;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace task_2.Services
{
    public class DataTableService:IDataTableService
    {
        private readonly MemoryDbContext _context;
        private readonly AutoMapper.IConfigurationProvider _mappingConfiguration;
        private readonly IConfiguration _config;

        public DataTableService(MemoryDbContext context, 
                                AutoMapper.IConfigurationProvider mappingConfiguration,
                                IConfiguration config)
        {
            this._context = context;
            this._mappingConfiguration = mappingConfiguration;
            this._config = config;
        }

        public async Task<int> SetDataAsync(NotificationEntity[] entities)
        {
            this._context.Notifications.AddRange(entities);
            return await this._context.SaveChangesAsync();
        }

        public async Task<Notification[]> GetDataAsync(DTParameters table)
        {            
            IQueryable<NotificationEntity> query = _context.Notifications;
            query = new SearchOptionsProcessor<Notification,NotificationEntity>().Apply(query,table.Columns);
            query = new SortOptionsProcessor<Notification,NotificationEntity>().Apply(query,table);

            var items = await query
                .AsNoTracking()
                .Skip(table.Start - 1 * table.Length)
                .Take(table.Length)
                .ProjectTo<Notification>(_mappingConfiguration)
                .ToArrayAsync();

            return items;
        }

        public int TotalRecords()
        {
            return this._context.Notifications.Count();
        }
    }
}