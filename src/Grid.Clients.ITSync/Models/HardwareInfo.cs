using System;
using System.IO;
using System.Management;
using System.Threading;

namespace Grid.Clients.ITSync.Models
{
    public class HardwareInfo
    {
        public static HardwareModel GetSystemDetails()
        {
            var harwareElements = new HardwareModel
            {
                UserDomainName = Environment.UserDomainName,
                Username = Environment.UserName,
                SystemDirectory = Environment.SystemDirectory,
                ProcessorCount = Environment.ProcessorCount,
                SystemPageSize = Environment.SystemPageSize,
                SystemOS = new Microsoft.VisualBasic.Devices.ComputerInfo().OSFullName
            };

            var search = new ManagementObjectSearcher("Select * From Win32_ComputerSystem");
            foreach (var item in search.Get())
            {
                var ramBytes = Convert.ToDouble(item["TotalPhysicalMemory"]) / 1000000000;
                harwareElements.RamMemory = ramBytes;
            }

            var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
            foreach (var oManagementObject in searcher.Get())
            {
                harwareElements.Processor = (string)oManagementObject["Name"];
            }

            var currentCulture = Thread.CurrentThread.CurrentCulture;
            harwareElements.Language = currentCulture.ToString();

            var info = DriveInfo.GetDrives();
            foreach (var drive in info)
            {
                if (drive.IsReady)
                    harwareElements.HardDiskSize = harwareElements.HardDiskSize + drive.TotalSize / 1000000000;
            }

            return harwareElements;
        }
    }
}
