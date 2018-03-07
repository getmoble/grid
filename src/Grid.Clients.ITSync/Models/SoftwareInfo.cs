using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Grid.Clients.ITSync.Models
{
    public class SoftwareInfo
    {
        public static List<SoftwareModel> GetInstalledApps()
        {
            var list = new List<SoftwareModel>();
            const string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (var rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (var skName in rk.GetSubKeyNames())
                {
                    using (var sk = rk.OpenSubKey(skName))
                    {
                        var software = new SoftwareModel();
                        try
                        {
                            software.Name = sk.GetValue("DisplayName").ToString();
                            software.Publisher = sk.GetValue("Publisher").ToString();
                            software.DisplayVersion = sk.GetValue("DisplayVersion").ToString();
                        }
                        catch (Exception)
                        {
                            // Eat it as of now
                        }

                        if (!string.IsNullOrEmpty(software.Name))
                        {
                            software.Name = software.Name.Trim();
                            list.Add(software);
                        }
                    }
                }
            }
            return list;
        }
    }
}
