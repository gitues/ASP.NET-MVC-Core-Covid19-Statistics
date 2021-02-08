using ASPNETCoreMVCcovid19App.Controllers;
using ASPNETCoreMVCcovid19App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace XUnitTest
{    
    public class HomeControllerTest
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStatisticsService _httpClientFactory;
        private readonly IConfiguration _config;
        public async System.Threading.Tasks.Task IndexAsync()
        {
            // Arrange
            HomeController controller = new HomeController(_logger, _httpClientFactory, _config);

            // Act
            ViewResult result = await controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

    }

}
