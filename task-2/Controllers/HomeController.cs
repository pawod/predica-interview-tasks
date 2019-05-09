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

namespace task_2.Controllers
{
    public class HomeController:Controller
    {
        private readonly IDataTableService _dtService;
        private readonly IConfiguration _config;

        public HomeController(IDataTableService dtService, IConfiguration config)
        {
            _dtService = dtService;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody]DTParameters table)
        {
            try
            {
                var items = await _dtService.GetDataAsync(table);
                    return new JsonResult(new DTResult<Notification> {
                        draw = table.Draw,
                        data = items,
                        recordsFiltered = _dtService.TotalRecords(),
                        recordsTotal = _dtService.TotalRecords()
                    });
                
            } catch(Exception e)
            {
                Console.Write(e.Message);
                return new JsonResult(new { error = "Internal Server Error" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadRemoteData()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var url = ($"{_config.GetValue<string>("ApiBaseUrl")}?" +
                                $"id={_config.GetValue<string>("ApiViewId")}&" +
                                $"apikey={_config.GetValue<string>("ApiKey")}");
                    var response = await client.GetStringAsync(url);
                    var root = JObject.Parse(response);
                    var list = root["result"]["result"]["notifications"];
                    var parsed = list.Select(item => JsonConvert.DeserializeObject<NotificationEntity>(item.ToString())).ToArray();

                    await this._dtService.SetDataAsync(parsed);
                    return StatusCode(200);
                }
            } catch(Exception e)
            {
                Console.Write(e.Message);
                return StatusCode(500);
            }
        }
    }
}