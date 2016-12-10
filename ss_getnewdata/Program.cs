using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace ss_getnewdata
{
    class Program
    {
        static string current_res_ver = "";
        static string manifestString = "Android_AHigh_SHigh";

        static void Main(string[] args)
        {
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

            if (Environment.GetCommandLineArgs().Contains("-audio_low"))
            {
                manifestString = "Android_AHigh_SLow";
            }

            bool no_window = false;

            string previous_res_ver = "";
            string filename = "";
            bool isDiff = true;
            try
            {
                current_res_ver = args[Array.IndexOf(args, "-current") + 1];
                if (current_res_ver == args[0])
                {
                    Environment.Exit(1);
                }
            }
            catch
            {
                Environment.Exit(1);
            }
            Clear();

            if (args.Contains("-filelist"))
            {
                Process tempProcess = new Process();
                tempProcess.StartInfo.FileName = Process.GetCurrentProcess().MainModule.FileName.Replace(".vshost", "");
                string[] tempStrings = File.ReadAllLines("filelist.txt");
                foreach (string tempString in tempStrings)
                {
                    tempProcess.StartInfo.Arguments = "-current " + current_res_ver + " -file " + tempString.Split(':')[0].Replace("\"", "");
                    if (Environment.GetCommandLineArgs().Contains("-audio_low"))
                    {
                        tempProcess.StartInfo.Arguments = "-current " + current_res_ver + " -file " + tempString.Split(':')[0].Replace("\"", "") + " -audio_low";
                    }
                    tempProcess.StartInfo.UseShellExecute = false;
                    tempProcess.Start();
                    tempProcess.WaitForExit();
                }
                Environment.Exit(0);
            }

            try
            {
                previous_res_ver = args[Array.IndexOf(args, "-previous") + 1];
                if (previous_res_ver == args[0])
                {
                    isDiff = false;
                }
            }
            catch
            {
                isDiff = false;
            }
            string current_manifest = "http://storage.game.starlight-stage.jp/dl/" + current_res_ver + "/manifests/" + manifestString;
            string previous_manifest = "";
            if (isDiff)
            {
                previous_manifest = "http://storage.game.starlight-stage.jp/dl/" + previous_res_ver + "/manifests/" + manifestString;
            }
            Process p = new Process();
            p.StartInfo.FileName = "wget.exe";
            p.StartInfo.CreateNoWindow = no_window;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = current_manifest + " --no-check-certificate";
            if (!File.Exists("current_manifest"))
            {
                p.Start();
                p.WaitForExit();
                File.Move(manifestString, "current_manifest");
            }
            if (isDiff)
            {
                p.StartInfo.Arguments = previous_manifest + " --no-check-certificate";
                p.Start();
                p.WaitForExit();
                File.Move(manifestString, "previous_manifest");
            }
            p.StartInfo.FileName = "lz4er-win.exe";
            p.StartInfo.Arguments = "current_manifest";
            p.Start();
            p.WaitForExit();
            if (isDiff)
            {
                p.StartInfo.Arguments = "previous_manifest";
                p.Start();
                p.WaitForExit();
            }
            SQLiteConnection connection = null;
            if (isDiff)
            {
                connection = new SQLiteConnection("Data Source=previous_manifest.extracted;Version=3;");
                connection.Open();
            }
            //string query = "select name,hash from manifests where name like \"chara%\" and name like \"%base%\" or name like \"card%\" and name like \"%petit%\" or name like \"card%\" and name like \"%live%\" or name like \"card%\" and name like \"%sign%\" or name like \"card%\" and name like \"%xl%\" or name like \"card%\" and name like \"%bg%\" or name like \"comic%\" and name like \"%m.%\" or name like \"%photo_l%\"";
            //string query = "select name,hash from manifests where name like \"%chara_201%\"";
            //string query = "select name,hash from manifests where name like \"%237%.acb\"";
            //string query = "select name,hash from manifests where name like \"l/song%\"";
            //string query = "select name,hash from manifests where name like \"%.acb%\"";
            //string query = "select name,hash from manifests where name like \"story_storydata%\"";
            //string query = "select name,hash from manifests where name like \"card%\" and name like \"%xl%\"";
            //string query = "select name,hash from manifests where name like \"card%\" and name like \"%bg%\" and name not like \"%\\_01.unity3d\" escape \"\\\"";
            //string query = "select name,hash from manifests where name like \"gachaselect%\"";
            //string query = "select name,hash from manifests where name like \"chara%\" and name like \"%174%\" and name like \"%.unity3d\"";
            //string query = "select name,hash from manifests where name like \"%bgm_event_3005%\"";
            //string query = "select name,hash from manifests where name like \"item_%\"";
            //string query = "select name,hash from manifests where name like \"%chara_283%\" and name like \"%unity3d\"";
            //string query = "select name,hash from manifests where name like \"%stamp%\"";
            //string query = "select name,hash from manifests where name like \"v/event_announce_%\" or name like \"story_thumbnail%\" and name not like \"story_thumbnail_5%\" or name like \"banner%\"";
            //string query = "select name,hash from manifests where name like \"%photo_l%\"";
            //string query = "select name,hash from manifests where name like \"item_20001%\"";
            //string query = "select name,hash from manifests where name like \"%gachaselect_3%\"";
            //string query = "select name,hash from manifests where name like \"%gachaselect_%\"";
            //string query = "select name,hash from manifests where name like 'atlas_gacha_common.unity3d' or name like 'item_50006_s.unity3d' or name like 'item_50007_s.unity3d' or name like 'live_atlas_liveuiadd.unity3d' or name like 'loginbonus_bg_9005.unity3d' or name like 'banner_bnr_000047.unity3d' or name like 'jacket_1012.unity3d'";
            //string query = "select name,hash from manifests where name like 'atlas%.unity3d'";
            //string query = "select name,hash from manifests where name like \"%loginbonus_bg%\"";
            //string query = "select name,hash from manifests where name like \"%tutorial%\"";
            //string query = "select name,hash from manifests where name like \"%emblem%\"";
            //string query = "select name,hash from manifests where name like \"bg%\""
            //string query = "select name,hash from manifests where name like \"3d_uvm_uv_movie%\"";
            //string query = "select name,hash from manifests where name like \"%530120%\"";
            //string query = "select name,hash from manifests where name like \"%jacket%\"";
            string query = "select name,hash from manifests";

            if (Environment.GetCommandLineArgs().Contains("-getcards"))
            {
                query = "select name,hash from manifests where name like \"card%\" and name like \"%xl%\"";
            }
            if (Environment.GetCommandLineArgs().Contains("-getcards_bg"))
            {
                query = "select name,hash from manifests where name like \"card%\" and name like \"%bg%\" and name not like \"%\\_01.unity3d\" escape \"\\\"";
            }
            if (Environment.GetCommandLineArgs().Contains("-getcards_all"))
            {
                query = "select name,hash from manifests where name like \"chara%\" and name like \"%base%\" or name like \"card%\" and name like \"%petit%\" or name like \"card%\" and name like \"%live%\" or name like \"card%\" and name like \"%sign%\" or name like \"card%\" and name like \"%xl%\" or name like \"card%\" and name like \"%bg%\" or name like \"comic%\" and name like \"%m.%\" or name like \"%photo_l%\"";
            }
            if (Environment.GetCommandLineArgs().Contains("-end_of_event"))
            {
                query = "select name,hash from manifests where name like \"v/event_announce_%\" or name like \"story_thumbnail%\" and name not like \"story_thumbnail_5%\" or name like \"banner%\"";
            }
            if (Environment.GetCommandLineArgs().Contains("-platgacha_only"))
            {
                query = "select name,hash from manifests where name like \"%gachaselect_3%\"";
            }
            if (Environment.GetCommandLineArgs().Contains("-song_only"))
            {
                query = "select name,hash from manifests where name like \"l/song%\"";
            }

            string card_id = "";

            try
            {
                card_id = args[Array.IndexOf(args, "-card") + 1];
                if (card_id == args[0])
                {
                }
                else
                {
                    query = "select name,hash from manifests where name like '%card%' and name like '%.unity3d' and name like '%" + card_id + "%'";
                }
            }
            catch
            {
            }

            //query = "select name,hash from manifests where name like \"%photo_l_sx%\"";
            //string[] s_array = File.ReadAllLines("gacha2.csv");
            //foreach (string sx in s_array)
            //{
            //    query += " or name like \"%photo_l%\" and name like \"%" + sx + "%\"";
            //    //query += " or name like \"%photo%\" and name like \"%" + sx + "%\"";
            //}
            //Console.WriteLine(query);
            //Console.ReadLine();

            //string[] s_array = File.ReadAllLines("voiced_card_skills.csv");
            //string tmptext = "";
            //foreach (string sx in s_array)
            //{
            //    query = "select name,hash from manifests where name like \"%" + sx + "%\" and name like \"%.acb%\" and name like \"%card%\"";
            //    connection = new SQLiteConnection("Data Source=current_manifest.extracted;Version=3;");
            //    connection.Open();
            //    SQLiteCommand sqc = new SQLiteCommand(query, connection);
            //    SQLiteDataReader sqdr = sqc.ExecuteReader();
            //    while (sqdr.Read())
            //    {
            //        tmptext += "http://storage.game.starlight-stage.jp/dl/resources/High/Sound/Common/" + sqdr["name"].ToString().Split('/')[0] + "/" + sqdr["hash"].ToString() + "\r\n";
            //    }
            //    connection.Close();
            //}
            //File.WriteAllText("skills.txt", tmptext);
            //Environment.Exit(0);

            try
            {
                filename = args[Array.IndexOf(args, "-file") + 1];
                if (filename != args[0])
                {
                    query = "select name,hash from manifests where name like \"" + filename + "\"";
                }
            }
            catch
            { }
            SQLiteCommand command = null;
            SQLiteDataReader reader = null;
            if (isDiff)
            {
                command = new SQLiteCommand(query, connection);
                reader = command.ExecuteReader();
            }
            Stack<string> stack1 = new Stack<string>();
            Stack<string> stack2 = new Stack<string>();
            if (isDiff)
            {
                while (reader.Read())
                {
                    stack1.Push(reader["name"].ToString());
                    stack2.Push(reader["hash"].ToString());
                }
                connection.Close();
            }
            connection = new SQLiteConnection("Data Source=current_manifest.extracted;Version=3;");
            connection.Open();
            command = new SQLiteCommand(query, connection);
            reader = command.ExecuteReader();
            string text1 = "";
            Stack<string> assetBundleStack = new Stack<string>();
            Stack<string> soundStack = new Stack<string>();
            Stack<string> blobDbStack = new Stack<string>();
            Stack<string> mdbStack = new Stack<string>();
            while (reader.Read())
            {
                bool checkCondition = !stack1.Contains(reader["name"].ToString());
                if (Environment.GetCommandLineArgs().Contains("-diff"))
                {
                    checkCondition = !stack2.Contains(reader["hash"].ToString());
                }
                if (Environment.GetCommandLineArgs().Contains("-diff_only"))
                {
                    checkCondition = stack1.Contains(reader["name"].ToString()) && !stack2.Contains(reader["hash"].ToString());
                }
                if (checkCondition)
                {
                    if (reader["name"].ToString().Contains(".unity3d"))
                    {
                        if (query == "select name,hash from manifests")
                        {
                            text1 = text1 + reader["name"].ToString() + ": ";
                        }
                        text1 = text1 + "http://storage.game.starlight-stage.jp/dl/resources/High/AssetBundles/Android/" + reader["hash"].ToString() + "\r\n";
                        assetBundleStack.Push(reader["hash"].ToString());
                    }
                    else
                    {
                        if (reader["name"].ToString().Contains(".acb"))
                        {
                            if (query == "select name,hash from manifests")
                            {
                                text1 = text1 + reader["name"].ToString() + ": ";
                            }
                            text1 = text1 + "http://storage.game.starlight-stage.jp/dl/resources/High/Sound/Common/" + reader["name"].ToString().Split('/')[0] + "/" + reader["hash"].ToString() + "\r\n";
                            if (Environment.GetCommandLineArgs().Contains("-audio_low"))
                            {
                                text1 = text1.Replace("High/Sound", "Low/Sound");
                            }
                            soundStack.Push(reader["hash"].ToString());
                        }
                        else
                        {
                            if (reader["name"].ToString().Contains(".bdb"))
                            {
                                if (query == "select name,hash from manifests")
                                {
                                    text1 = text1 + reader["name"].ToString() + ": ";
                                }
                                text1 = text1 + "http://storage.game.starlight-stage.jp/dl/resources/Generic//" + reader["hash"].ToString() + "\r\n";
                                blobDbStack.Push(reader["hash"].ToString());
                            }
                            else
                            {
                                if (reader["name"].ToString().Contains(".mdb"))
                                {
                                    if (query == "select name,hash from manifests")
                                    {
                                        text1 = text1 + reader["name"].ToString() + ": ";
                                    }
                                    text1 = text1 + "http://storage.game.starlight-stage.jp/dl/resources/Generic//" + reader["hash"].ToString() + "\r\n";
                                    mdbStack.Push(reader["hash"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            connection.Close();
            File.WriteAllText(current_res_ver + ".txt", text1);
            if (query == "select name,hash from manifests")
            {
                Environment.Exit(0);
            }
            p.StartInfo.FileName = "wget.exe";
            p.StartInfo.Arguments = "-i " + current_res_ver + ".txt" + " --no-check-certificate";
            p.Start();
            p.WaitForExit();
            foreach (string filehash in assetBundleStack)
            {
                if (query == "select name,hash from manifests where name like \"" + filename + "\"")
                {
                    try
                    {
                        Directory.CreateDirectory("downloaded_files");
                    }
                    catch { }
                    try
                    {
                        File.Delete("downloaded_files\\" + filename + ".lz4");
                    }
                    catch { }
                    File.Move(filehash, "downloaded_files\\" + filename + ".lz4");
                }
                else
                {
                    if (query == "select name,hash from manifests where name like \"" + filename + "\"" || query == "select name,hash from manifests where name like \"v/event_announce_%\" or name like \"story_thumbnail%\" and name not like \"story_thumbnail_5%\" or name like \"banner%\"")
                    {
                        try
                        {
                            Directory.CreateDirectory("downloaded_files");
                        }
                        catch { }
                        try
                        {
                            File.Delete("downloaded_files\\" + filehash + ".unity3d.lz4");
                        }
                        catch { }
                        File.Move(filehash, "downloaded_files\\" + filehash + ".unity3d.lz4");
                    }
                    else
                    {
                        try
                        {
                            File.Move(filehash, filehash + ".unity3d.lz4");
                        }
                        catch { }
                    }
                }
            }
            foreach (string filehash in soundStack)
            {
                if (query == "select name,hash from manifests where name like \"" + filename + "\"")
                {
                    try
                    {
                        Directory.CreateDirectory("downloaded_files");
                    }
                    catch { }
                    try
                    {
                        File.Delete("downloaded_files\\" + filename.Split('/').Last());
                    }
                    catch { }
                    File.Move(filehash, "downloaded_files\\" + filename.Split('/').Last());
                }
                else
                {
                    if (query == "select name,hash from manifests where name like \"" + filename + "\"" || query == "select name,hash from manifests where name like \"v/event_announce_%\" or name like \"story_thumbnail%\" and name not like \"story_thumbnail_5%\" or name like \"banner%\"")
                    {
                        try
                        {
                            Directory.CreateDirectory("downloaded_files");
                        }
                        catch { }
                        try
                        {
                            File.Delete("downloaded_files\\" + filehash + ".acb");
                        }
                        catch { }
                        File.Move(filehash, "downloaded_files\\" + filehash + ".acb");
                    }
                    else
                    {
                        try
                        {
                            File.Move(filehash, filehash + ".acb");
                        }
                        catch { }
                    }
                }
            }
            foreach (string filehash in blobDbStack)
            {
                if (query == "select name,hash from manifests where name like \"" + filename + "\"")
                {
                    try
                    {
                        Directory.CreateDirectory("downloaded_files");
                    }
                    catch { }
                    try
                    {
                        File.Delete("downloaded_files\\" + filename + ".lz4");
                    }
                    catch { }
                    File.Move(filehash, "downloaded_files\\" + filename + ".lz4");
                }
                else
                {
                    try
                    {
                        File.Move(filehash, filehash + ".bdb.lz4");
                    }
                    catch { }
                }
            }
            foreach (string filehash in mdbStack)
            {
                if (query == "select name,hash from manifests where name like \"" + filename + "\"")
                {
                    try
                    {
                        Directory.CreateDirectory("downloaded_files");
                    }
                    catch { }
                    try
                    {
                        File.Delete("downloaded_files\\" + filename + ".lz4");
                    }
                    catch { }
                    File.Move(filehash, "downloaded_files\\" + filename + ".lz4");
                }
                else
                {
                    try
                    {
                        File.Move(filehash, filehash + ".mdb.lz4");
                    }
                    catch { }
                }
            }
            File.Delete(current_res_ver + ".txt");
            if (query == "select name,hash from manifests where name like \"" + filename + "\"")
            {
                Environment.Exit(0);
            }
            if (query == "select name,hash from manifests where name like \"v/event_announce_%\" or name like \"story_thumbnail%\" and name not like \"story_thumbnail_5%\" or name like \"banner%\"")
            {
                DirectoryInfo d2 = new DirectoryInfo(".\\downloaded_files");
                p.StartInfo.FileName = "unitystudio\\Unity Studio.exe";
                p.StartInfo.WorkingDirectory = ".\\downloaded_files";
                foreach (FileInfo f in d2.EnumerateFiles("*.unity3d.lz4"))
                {
                    p.StartInfo.Arguments = "-assetbundle \"" + f.FullName + "\"";
                    p.Start();
                    p.WaitForExit();
                }
                p.StartInfo.FileName = @"c:\windows\system32\cmd.exe";
                p.StartInfo.Arguments = "/c call ..\\create_montage.bat";
                p.Start();
                p.WaitForExit();
                Environment.Exit(0);
            }
            DirectoryInfo d = new DirectoryInfo(".\\");
            p.StartInfo.FileName = "unitystudio\\Unity Studio.exe";
            foreach (FileInfo f in d.EnumerateFiles("*.unity3d.lz4"))
            {
                p.StartInfo.Arguments = "-assetbundle \"" + f.FullName + "\"";
                p.Start();
                p.WaitForExit();
            }
            if (query.Equals("select name,hash from manifests where name like \"card%\" and name like \"%xl%\""))
            {
                p.StartInfo.FileName = @"c:\windows\system32\cmd.exe";
                p.StartInfo.Arguments = "/c call create_montage.bat";
                p.StartInfo.WorkingDirectory = ".\\";
                p.Start();
                p.WaitForExit();
            }
            if (query.Equals("select name,hash from manifests where name like \"card%\" and name like \"%bg%\" and name not like \"%\\_01.unity3d\" escape \"\\\""))
            {
                p.StartInfo.FileName = @"c:\windows\system32\cmd.exe";
                p.StartInfo.Arguments = "/c call create_montage.bat";
                p.StartInfo.WorkingDirectory = ".\\";
                p.Start();
                p.WaitForExit();
                int jpgQuality = 92;
                if (File.Exists(@".\Texture2D\out.jpg"))
                {
                    FileInfo file1 = new FileInfo(@".\Texture2D\out.jpg");
                    while (file1.Length >= 4 * 1024 * 1024)
                    {
                        File.Delete(@".\Texture2D\out.jpg");
                        p.StartInfo.Arguments = "/c call convert_jpg.bat " + jpgQuality--.ToString();
                        p.Start();
                        p.WaitForExit();
                        file1 = new FileInfo(@".\Texture2D\out.jpg");
                    }
                }
            }
            Clear();
        }

        static void Clear()
        {
            try
            {
                string newpath = DateTime.Now.ToString();
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    newpath = newpath.Replace(c.ToString(), "");
                }
                Directory.Move(@".\Texture2D", @".\Texture2D" + newpath);
            }
            catch { }
            try
            {
                DirectoryInfo d = new DirectoryInfo(".\\");
                foreach (FileInfo f in d.EnumerateFiles("*.unity3d.lz4"))
                {
                    File.Delete(f.FullName);
                }
            }
            catch { }
            try
            {
                File.Delete(manifestString);
            }
            catch { }
            if (File.Exists("current_manifest"))
            {
                new System.Net.WebClient().DownloadFile("http://storage.game.starlight-stage.jp/dl/" + current_res_ver + "/manifests/all_dbmanifest", "hashes");
                string[] lines = File.ReadAllLines("hashes");
                string hash = "";
                foreach (string line in lines)
                {
                    if (line.Contains(manifestString))
                    {
                        hash = line.Split(',')[1];
                    }
                }
                string current_hash = BitConverter.ToString(System.Security.Cryptography.MD5.Create().ComputeHash(File.ReadAllBytes("current_manifest"))).Replace("-", "").ToLower();
                if (!hash.Equals(current_hash))
                {
                    try
                    {
                        File.Delete("current_manifest");
                    }
                    catch { }
                }
                File.Delete("hashes");
            }
            try
            {
                File.Delete("previous_manifest");
            }
            catch { }
            try
            {
                File.Delete("current_manifest.extracted");
            }
            catch { }
            try
            {
                File.Delete("previous_manifest.extracted");
            }
            catch { }
        }
    }
}
