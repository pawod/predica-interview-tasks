using JqueryDataTables.ServerSide.AspNetCoreWeb;
using System;

namespace task_2.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [SearchableString]
        [Sortable(Default = true)]
        public string Category { get; set; }
        
        [SearchableString]
        [Sortable(Default = true)]
        public string SubCategory { get; set; }
        
        [SearchableString]
        [Sortable(Default = true)]
        public string District { get; set; }
        
        [SearchableString]
        [Sortable(Default = true)]
        public string NotificationType { get; set; }
        
        [SearchableString]
        [Sortable(Default = true)]
        public string Source { get; set; }
        
        public string Event { get; set; }
        
        [SearchableDateTime]
        [Sortable]
        public long CreateDate { get; set; }
    }
}