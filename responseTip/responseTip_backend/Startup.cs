﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace responseTip_backend
{
    class Startup
    {
        public void Configuration()
        {
            TwitterHandling.TwitterHandlingClass.twitterAuthentication(ConfigurationManager.AppSettings["Twitter_ConsumerKey"],
                        ConfigurationManager.AppSettings["Twitter_ConsumerSecret"],
                        ConfigurationManager.AppSettings["Twitter_AccessToken"],
                        ConfigurationManager.AppSettings["Twitter_AccessTokenSecret"]);
            BtcHandling.BtcHandlingClass.ConnectToRpc(ConfigurationManager.AppSettings["Bitcoin_DaemonUrl"],
                ConfigurationManager.AppSettings["Bitcoin_RpcUsername"],
                ConfigurationManager.AppSettings["Bitcoin_RpcPassword"],
                ConfigurationManager.AppSettings["Bitcoin_WalletPassword"]);
        }
    }
}
