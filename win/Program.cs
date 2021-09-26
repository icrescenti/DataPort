using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace DataShardPort
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            while (true)
            {
                var shards = DriveInfo.GetDrives().Where(drive => drive.IsReady && drive.DriveType == DriveType.Removable);

                foreach (DriveInfo shard in shards)
                {
                    if (shard.DriveType.ToString() == "Removable")
                    {
                        string[] lines = File.ReadAllText(shard.RootDirectory + "_").Split('\n');
                        foreach (string line in lines)
                        {
                            Process p = new Process();
                            p.StartInfo = new ProcessStartInfo(shard.RootDirectory + line.Replace("\r", String.Empty));
                            p.StartInfo.WorkingDirectory = shard.RootDirectory.ToString();
                            p.StartInfo.CreateNoWindow = true; 
                            p.StartInfo.UseShellExecute = true;
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                            p.Start();
                        }

                        showContents nw = new showContents(shard);
                        nw.ShowDialog();
                    }
                }
                Thread.Sleep(500);
            }
        }
    }
}
