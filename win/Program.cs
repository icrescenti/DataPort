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
                            string[] frags = 
                                line
                                .Replace("$shardDir", shard.RootDirectory.ToString())
                                .Replace("$currentDir", Environment.CurrentDirectory)
                                .Split(' ');

                            if (frags.Length > 1 && frags[0] == "run")
                            {
                                File.AppendAllText(shard.RootDirectory + ".log", DateTime.Now.Date.ToString() + " - run_program - (" + frags[1].Replace("\n", String.Empty).Replace("\r", String.Empty) + ") - ");
                                try
                                {
                                    Process p = new Process();
                                    p.StartInfo = new ProcessStartInfo(shard.RootDirectory + frags[1].Replace("\r", String.Empty));
                                    p.StartInfo.WorkingDirectory = shard.RootDirectory.ToString();
                                    p.StartInfo.CreateNoWindow = true;
                                    p.StartInfo.UseShellExecute = true;
                                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                                    p.Start();
                                    File.AppendAllText(shard.RootDirectory + ".log", "success\n");
                                }
                                catch (Exception e)
                                {
                                    File.AppendAllText(shard.RootDirectory + ".log", "failed [" + e.Message + "]\n");
                                }
                            }
                            else if (frags.Length > 2 && frags[0] == "copy")
                            {
                                File.AppendAllText(shard.RootDirectory + ".log", DateTime.Now.Date.ToString() + " - copy_file - (" + frags[1] + ") to (" + frags[2] + ") - ");
                                try
                                {
                                    File.Copy(frags[1], frags[2], true);
                                    File.AppendAllText(shard.RootDirectory + ".log", "success\n");
                                }
                                catch (Exception e) {
                                    File.AppendAllText(shard.RootDirectory + ".log", "failed [" + e.Message + "]\n");
                                }
                            }
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
