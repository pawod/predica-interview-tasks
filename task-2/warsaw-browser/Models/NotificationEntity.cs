using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using JqueryDataTables.ServerSide.AspNetCoreWeb;

namespace task_2.Models
{
    public class NotificationEntity
    {
        [Key]
        public int Id { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string District { get; set; }
        public string NotificationType { get; set; }
        public string Source { get; set; }
        public string Event { get; set; }
        public long CreateDate { get; set; }
    }
}