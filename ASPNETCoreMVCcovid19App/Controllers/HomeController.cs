using ASPNETCoreMVCcovid19App.Models;
using ASPNETCoreMVCcovid19App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ASPNETCoreMVCcovid19App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStatisticsService _httpClientFactory;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IStatisticsService statisticsService, IConfiguration config)
        {
            _logger = logger;
            _httpClientFactory = statisticsService;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.regionsList = JsonConvert.DeserializeObject<Regions>(await _httpClientFactory.GetStatistics("/regions")).data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                //Notify error to developer for fix isue
                try
                {
                    var notificationsx = new List<INotificationService>
                {
                    new NotificationEmailService(_config.GetValue<String>("accountDeveloper"), $"New error from Test Application", ex.ToString(), _config),
                };
                    var notificationService = new NotificationService();
                    await notificationService.Send(notificationsx);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Something went wrong sending email error: {e}");
                }

                ViewBag.regionsList = new List<Regions>();
            }
            return View();
        }

        [HttpPost("/Home/ReportAsync")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReportAsync(Region model)
        {
            try
            {
                var jsonString = await _httpClientFactory.GetStatistics("/reports" + (String.IsNullOrEmpty(model.iso) ? "" : "?iso=" + model.iso));
                var countries = JsonConvert.DeserializeObject<Countries>(jsonString).data;

                //If not regions selected load top ten countries cases
                //Other case load province top ten by region filter
                if (String.IsNullOrEmpty(model.iso))
                {
                    countries = countries
                     .GroupBy(l => l.region.iso)
                     .Select(cl => new Country
                     {
                         region = cl.First().region,
                         confirmed = cl.Sum(s => s.confirmed),
                         deaths = cl.Sum(c => c.deaths),
                     }).ToList();
                    return PartialView("_PartialCountry", countries.OrderByDescending(x => x.confirmed).Take(10).ToList());
                }
                else
                {
                    return PartialView("_PartialProvince", countries.OrderByDescending(x => x.confirmed).Take(10).ToList());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");

                //Notify error to developer for fix isue
                try
                {
                    var notificationsx = new List<INotificationService>
                {
                    new NotificationEmailService(_config.GetValue<String>("accountDeveloper"), $"New error from Test Application", ex.ToString(), _config),
                };
                    var notificationService = new NotificationService();
                    await notificationService.Send(notificationsx);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Something went wrong sending email error: {e}");
                }

                return Content("Servicio no disponible");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
