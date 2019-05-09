using JqueryDataTables.ServerSide.AspNetCoreWeb;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace task_2.Models
{
    public class ArmTemplateParams
    {
        [Required(ErrorMessage = "Please select a hosting plan")]
        public string EnvironmentType { get; set; }
        public List<SelectListItem> EnvironmentTypes { get; set; } = new List<SelectListItem>
                                                                    {
                                                                        new SelectListItem("dev", "dev"),
                                                                        new SelectListItem("test", "test"),
                                                                        new SelectListItem("prod", "prod")
                                                                    };
        [Required]
        public string ResourceName { get; set; }
        
        [Required]
        public string ResourceGroup { get; set; }
        

        [Required(ErrorMessage = "Please select a hosting plan")]

        public string HostingPlanName { get; set; }

        public List<SelectListItem> HostingPlanNames { get; set; } =  new List<SelectListItem>
                                                                    {
                                                                        new SelectListItem("free", "free"),
                                                                        new SelectListItem("shared", "shared"),
                                                                        new SelectListItem("basic", "basic"),
                                                                        new SelectListItem("standard", "standard"),
                                                                        new SelectListItem("premium", "premium"),
                                                                        new SelectListItem("isolated", "isolated"),
                                                                        new SelectListItem("app service linux", "app service linux"),
                                                                        new SelectListItem("consumption plan", "consumption plan")
                                                                    };
        [Required]
        public string ServiceOwner { get; set; }
        
        [Required, UrlAttribute]
        public string WarsawApiUrl { get; set; }

        public string SubscriptionId { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string AzSecret { get; set; }
    }
}