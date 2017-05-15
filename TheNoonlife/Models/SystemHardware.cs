using System;
using System.Net.NetworkInformation;

namespace TheNoonlife.Models
{
    public static class SystemHardware
    {
<<<<<<< HEAD
=======

>>>>>>> 1b305000c73202a7adc9cc6592b2d5dc1b262f28
        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
    }
}