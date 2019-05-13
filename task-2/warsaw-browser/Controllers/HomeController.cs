using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JqueryDataTables.ServerSide.AspNetCoreWeb;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using task_2.Contracts;
using task_2.Models;
using Microsoft.Extensions.Logging;
using WarsawBrowser.Models;
using Microsoft.Extensions.Options;

namespace task_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataTableService _dtService;
        private readonly AppSettings _settings;
        private readonly ILogger _logger;

        public HomeController(IDataTableService dtService, IOptions<AppSettings> settings, ILogger<Startup> logger)
        {
            this._dtService = dtService;
            this._settings = settings.Value;
            this._logger = logger;
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
                return new JsonResult(new DTResult<Notification>
                {
                    draw = table.Draw,
                    data = items,
                    recordsFiltered = _dtService.TotalRecords(),
                    recordsTotal = _dtService.TotalRecords()
                });
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new JsonResult(new { error = "Internal Server Error" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadRemoteData(long dateFrom, long dateTo)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var url = $"{this._settings.WarsawApiUrl}?id={this._settings.WarsawApiView}";
                    url = (dateFrom > 0) ? $"{url}&dateFrom={dateFrom}" : $"{url}&dateFrom={_settings.MinLinuxUtcTimestamp}";
                    url = (dateTo > 0) ? $"{url}&dateTo={dateTo}" : $"{url}&dateTo={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
                    url = $"{url}&apikey={this._settings.WarsawApiKey}";

                    var response = await client.GetStringAsync(url);
                    var root = JObject.Parse(response);

                    if(root["error"] != null) throw new Exception(root["error"].Value<string>());

                    var list = root["result"]["result"]["notifications"];
                    var parsed = list.Select(item => JsonConvert.DeserializeObject<NotificationEntity>(item.ToString())).ToArray();

                    await this._dtService.SetDataAsync(parsed);
                    return StatusCode(200);
                }
            }
            catch (Exception e)
            {
                this._logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }
    }
}