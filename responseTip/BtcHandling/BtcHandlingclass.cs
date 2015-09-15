using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitcoinLib.Services.Coins.Base;
using BitcoinLib.Services.Coins.Bitcoin;
using System.Diagnostics;
using BitcoinLib.ExceptionHandling.Rpc;


namespace BtcHandling
{
    public class BtcHandlingClass
    {
        private static ICoinService CoinService;

        public static void ConnectToRpc(string daemonUrl, string rpcUsername, string rpcPassword, string walletPassword)
        {
            CoinService = new BitcoinService(daemonUrl, rpcUsername, rpcPassword, walletPassword);
        }
        public static string GetNewBtcAdress()
        {
            string newBtcAdress = "";
            try
            {
                newBtcAdress = CoinService.GetNewAddress();
            }
            catch (RpcException exception)
            {
                Debug.WriteLine("[Failed]\n\nPlease check your configuration and make sure that the daemon is up and running and that it is synchronized. \n\nException: " + exception);
            }
            catch (Exception e)
            {
                Debug.WriteLine("General exception at: " + e.StackTrace);
            }

            return newBtcAdress;
        }
    }
}
