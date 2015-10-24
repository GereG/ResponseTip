using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using System.Globalization;

namespace responseTip.Helpers
{
    public class bitcoinAverageAPI
    {
        public static decimal CallForBitcoinAverageDollarPrice()
        {
            decimal dollarPrice=0;
            var client = new RestClient();
            client.BaseUrl = new Uri("https://api.bitcoinaverage.com/");
//            client.Authenticator = new HttpBasicAuthenticator("username", "password");

            var request = new RestRequest();
            request.Resource = "ticker/global/USD/last";

            IRestResponse response = client.Execute(request);
//            IFormatProvider formatProvider=
            dollarPrice=Convert.ToDecimal(response.Content, CultureInfo.InvariantCulture);
            return dollarPrice;
        }
}
}
