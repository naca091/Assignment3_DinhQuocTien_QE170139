using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AppConfigProvider
    {
        public static AppConfig LoadAppConfig()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            var email = config["account:defaultAccount:email"];
            var password = config["account:defaultAccount:password"];
            var appConfig = new AppConfig();
            appConfig.DefaultAccount = new DefaultAccount
            {
                Email = email,
                Password = password
            };

            return appConfig;
        }
    }
}
