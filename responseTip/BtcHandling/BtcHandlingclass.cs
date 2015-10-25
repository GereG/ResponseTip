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
        public static DateTime timeEstimatedFeeChecked;
        public const int estimatedFeeCheckingIntervalInMinutes = 100;
        private static uint currentBlockCount=0;
        private static double actualKeyPoolSize = 0;



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
                currentBlockCount = newBlockCount;
                EstimateTxFee();
                return true;
            }
            else { return false; }
        }

        public static decimal UpdateEstimatedTxFee()
        {
            TimeSpan timeFromLastUpdate = DateTime.Now.Subtract(timeEstimatedFeeChecked);
            if (estimatedFeeCheckingIntervalInMinutes < timeFromLastUpdate.TotalMinutes)
            {
                EstimateTxFee();
            }


            return estimatedFee;
        }

        public static void WalletPassphrase()
        {
            try
            {
                CoinService.WalletPassphrase("KPh,D%ks^9rg", 60); //works as keypool refill
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

        public static void EstimateTxFee()
        {
            try
            {
//                estimatedFee = CoinService.EstimateFee(500);
                estimatedFee= CoinService.GetMinimumNonZeroTransactionFeeEstimate(1, 1);
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
                CoinService.SetAccount(newBtcAdress, newBtcAdress);
                actualKeyPoolSize--;
            }
            catch(RpcInternalServerErrorException exception)
            {
                if(exception.RpcErrorCode==BitcoinLib.RPC.Specifications.RpcErrorCode.RPC_WALLET_KEYPOOL_RAN_OUT)
                {
                    //                    CoinService.WalletPassphrase(passphrase, 60);
                    //                    CoinService.KeyPoolRefill(1000);
                    //                    newBtcAdress = GetNewBtcAdress();
                    bitcoin_Logger.LogLine("keypool ran out", responseTip.Helpers.Logger.log_types.ERROR_LOG);
                }
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

/*        public static void SendToAddress(string targetAddress,decimal amount) //depreciated - dont use
        {
//            decimal originAddressBalance = CheckAdressBalance(originAddress);
            decimal txFee = BtcHandlingClass.UpdateEstimatedTxFee();
            if (txFee >= amount)
            {
                bitcoin_Logger.LogLine("Amount too low to send(amount lower then TxFee).", responseTip.Helpers.Logger.log_types.MESSAGE_LOG);
                return;
            }
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
        }*/

        public static void SendFromAndToAddress(string sourceAddress,string targetAddress, decimal amount,string passphrase) // since every generation of new address creates account with same name, we can use sendFrom to send from particular address
        {
            decimal txFee = BtcHandlingClass.UpdateEstimatedTxFee();
            if (txFee >= amount)
            {
                bitcoin_Logger.LogLine("Amount too low to send(amount lower then TxFee).", responseTip.Helpers.Logger.log_types.MESSAGE_LOG);
                return;
            }
            try
            {
                CoinService.WalletPassphrase(passphrase, 10);
                CoinService.SetTxFee(txFee);
                CoinService.SendFrom(sourceAddress, targetAddress, amount - txFee, 2);
                CoinService.WalletLock();
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

        public static double KeyPoolSize(string passphrase)
        {
            double keyPoolSize = 0;
            BitcoinLib.Responses.GetInfoResponse getInfoResponse=null;
            try {
                getInfoResponse = CoinService.GetInfo();
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
            keyPoolSize = getInfoResponse.KeyPoolSize;

            return keyPoolSize;
        }

        public static void UpdateKeyPool(string passphrase)
        {
            if (actualKeyPoolSize == 0)
            {
                actualKeyPoolSize=KeyPoolSize(passphrase);
            }
            if(actualKeyPoolSize<100)
            {
                try {
                    CoinService.WalletPassphrase(passphrase, 300);

                    CoinService.KeyPoolRefill(1000);
                    
//                    CoinService.WalletLock();
                }
                catch(BitcoinLib.ExceptionHandling.Rpc.RpcRequestTimeoutException exception)
                {
                    bitcoin_Logger.LogLine("timeoutRequest", responseTip.Helpers.Logger.log_types.WARNING_LOG);
                    System.Threading.Thread.Sleep(300000);
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

                //TODO add wallet backup 
                actualKeyPoolSize = 1000;
                bitcoin_Logger.LogLine("KeypoolSize regenerate to 1000.", responseTip.Helpers.Logger.log_types.MESSAGE_LOG);
            }
        }


    }
}
