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
        private static decimal estimatedFee=0;
        private static uint currentBlockCount=0;



        public static void ConnectToRpc(string daemonUrl, string rpcUsername, string rpcPassword, string walletPassword)
        {
            bitcoin_Logger = new responseTip.Helpers.Logger();
            bitcoin_Logger.SetPath("C:\\Users\\GereG\\Source\\Repos\\ResponseTip\\responseTip");
            CoinService = new BitcoinService(daemonUrl, rpcUsername, rpcPassword, walletPassword);
            
        }

        public static bool IsNextBlock()
        {
            uint newBlockCount = 0;
            try
            {
                newBlockCount = CoinService.GetBlockCount();
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

            if (newBlockCount>currentBlockCount)
            {
                EstimateTxFee();
                return true;
            }
            else { return false; }
        }


        public static void EstimateTxFee()
        {
            try
            {
                estimatedFee = CoinService.EstimateFee(50);
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

            return;
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

                isAdressValidAndMine = IsAddressValidAndMine(address);
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

        public static bool IsAddressValidAndMine(string address)
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

        public static bool IsAddressValid(string address)
        {
            bool isValid = false;
            BitcoinLib.Responses.ValidateAddressResponse validateResponse = null;

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

            if (validateResponse != null)
            {
                isValid = validateResponse.IsValid;
            }

            return (isValid);
        }

        public static void SendToAddress(string targetAddress,decimal amount)
        {
//            decimal originAddressBalance = CheckAdressBalance(originAddress);
            decimal txFee = BtcHandlingClass.estimatedFee;
            try
            {
                CoinService.SetTxFee(txFee);
                CoinService.SendToAddress(targetAddress, amount - txFee);
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
        }


    }
}
