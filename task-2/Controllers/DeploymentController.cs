using task_2.Contracts;
using task_2.Models;
using JqueryDataTables.ServerSide.AspNetCoreWeb;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using task_2.Services;

namespace task_2.Controllers
{
    public class DeploymentController:Controller
    {
        private readonly IConfiguration _config;
        private readonly IDeploymentService _deploymentService;

        public DeploymentController(IConfiguration config, IDeploymentService _deploymentService)
        {
            this._config = config;
            this._deploymentService = _deploymentService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ArmTemplateParams();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplate(ArmTemplateParams model)
        {
            // todo: use DI + init
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }            
            
            this._deploymentService.RunAsync(model);
            return StatusCode(200);
        }
    }
}