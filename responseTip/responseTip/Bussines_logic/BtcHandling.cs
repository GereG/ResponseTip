using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitcoinLib.Auxiliary;
using BitcoinLib.ExceptionHandling.Rpc;
using BitcoinLib.Responses;
using BitcoinLib.Services.Coins.Base;
using BitcoinLib.Services.Coins.Bitcoin;
using System.Diagnostics;

namespace responseTip.Bussines_logic
{
    internal sealed class BtcHandling
    {
        private static readonly ICoinService CoinService = new BitcoinService(useTestnet: false);

        public static string GetNewBtcAdress()
        {
            string newBtcAdress="";
            try
            {
                newBtcAdress = CoinService.GetNewAddress();
            }
            catch (RpcException exception)
            {
                Debug.WriteLine("[Failed]\n\nPlease check your configuration and make sure that the daemon is up and running and that it is synchronized. \n\nException: " + exception);
            }
            
            return newBtcAdress;
        }
    }
}
   