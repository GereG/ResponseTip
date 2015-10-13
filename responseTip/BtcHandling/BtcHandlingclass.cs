using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitcoinLib.Services.Coins.Base;
using BitcoinLib.Services.Coins.Bitcoin;
using System.Diagnostics;
using BitcoinLib.ExceptionHandling.Rpc;
using System.IO;

namespace BtcHandling
{
    public sealed class BtcHandlingClass
    {
        private static ICoinService CoinService;
        private static responseTip.Helpers.Logger bitcoin_Logger;

        public static void ConnectToRpc(string daemonUrl, string rpcUsername, string rpcPassword, string walletPassword)
        {
            bitcoin_Logger = new responseTip.Helpers.Logger();
            bitcoin_Logger.SetPath(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));
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
//                Debug.WriteLine("[Failed]\n\nPlease check your configuration and make sure that the daemon is up and running and that it is synchronized. \n\nException: " + exception);
                bitcoin_Logger.LogLine("[Failed]\n\nPlease check your configuration and make sure that the daemon is up and running and that it is synchronized. \n\nException: " + exception, responseTip.Helpers.Logger.log_types.ERROR_LOG);
//                newBtcAdress = "RPC exception";
            }
            catch (Exception e)
            {
//                Debug.WriteLine("General exception at: " + e.StackTrace);
                bitcoin_Logger.LogLine("General exception at: " + e.StackTrace, responseTip.Helpers.Logger.log_types.ERROR_LOG);
                //                newBtcAdress = "General exception";
            }

            return newBtcAdress;
        }

        public static decimal CheckAdressBalance(string address)
        {
            decimal addressBalance = 0;
            bool isAdressValidAndMine=false;

                isAdressValidAndMine = IsAdressValidAndMine(address);
            try
            {
                addressBalance = CoinService.GetAddressBalance(address, 2, true);
            }
            catch (RpcException exception)
            {
                //                Debug.WriteLine("[Failed]\n\nPlease check your configuration and make sure that the daemon is up and running and that it is synchronized. \n\nException: " + exception);
                bitcoin_Logger.LogLine("[Failed]\n\nPlease check your configuration and make sure that the daemon is up and running and that it is synchronized. \n\nException: " + exception, responseTip.Helpers.Logger.log_types.ERROR_LOG);
                //                newBtcAdress = "RPC exception";
            }
            catch (Exception e)
            {
                //                Debug.WriteLine("General exception at: " + e.StackTrace);
                bitcoin_Logger.LogLine("General exception at: " + e.StackTrace, responseTip.Helpers.Logger.log_types.ERROR_LOG);
                //                newBtcAdress = "General exception";
            }

//            bitcoin_Logger.LogLine(address + " has " + addressBalance + " and is valid and mine.",responseTip.Helpers.Logger.log_types.MESSAGE_LOG);

            return addressBalance;
        }

        public static bool IsAdressValidAndMine(string address)
        {
            bool isValid = false;
            bool isMine = false;
            BitcoinLib.Responses.ValidateAddressResponse validateResponse=null;

            try
            {
                validateResponse = CoinService.ValidateAddress(address);
            }
            catch (RpcException exception)
            {
                //                Debug.WriteLine("[Failed]\n\nPlease check your configuration and make sure that the daemon is up and running and that it is synchronized. \n\nException: " + exception);
                bitcoin_Logger.LogLine("[Failed]\n\nPlease check your configuration and make sure that the daemon is up and running and that it is synchronized. \n\nException: " + exception, responseTip.Helpers.Logger.log_types.ERROR_LOG);
                //                newBtcAdress = "RPC exception";
            }
            catch (Exception e)
            {
                //                Debug.WriteLine("General exception at: " + e.StackTrace);
                bitcoin_Logger.LogLine("General exception at: " + e.StackTrace, responseTip.Helpers.Logger.log_types.ERROR_LOG);
                //                newBtcAdress = "General exception";
            }

            isValid =validateResponse.IsValid;
            isMine = validateResponse.IsMine;

            return (isValid && isMine);
        }


    }
}
