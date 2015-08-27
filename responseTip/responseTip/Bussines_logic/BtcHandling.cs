using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitcoinLib.Auxiliary;
using BitcoinLib.ExceptionHandling.Rpc;
using BitcoinLib.Responses;
using BitcoinLib.Services.Coins.Base;
using BitcoinLib.Services.Coins.Bitcoin;

namespace responseTip.Bussines_logic
{
    internal sealed class BtcHandling
    {
        private static readonly ICoinService CoinService = new BitcoinService(useTestnet: false);

        public static string GetNewBtcAdress()
        {
            string newBtcAdress= CoinService.GetNewAddress();
            
            return newBtcAdress;
        }
    }
}
   