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
        static void Main(string[] args)
        {
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

            Clear();
            string current_res_ver = "";
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
            string current_manifest = "http://storage.game.starlight-stage.jp/dl/" + current_res_ver + "/manifests/Android_AHigh_SHigh";
            string previous_manifest = "";
            if (isDiff)
            {
                previous_manifest = "http://storage.game.starlight-stage.jp/dl/" + previous_res_ver + "/manifests/Android_AHigh_SHigh";
            }
            Process p = new Process();
            p.StartInfo.FileName = "wget.exe";
            p.StartInfo.Arguments = current_manifest + " --no-check-certificate";
            p.Start();
            p.WaitForExit();
            File.Move("Android_AHigh_SHigh", "current_manifest");
            if (isDiff)
            {
                p.StartInfo.Arguments = previous_manifest + " --no-check-certificate";
                p.Start();
                p.WaitForExit();
                File.Move("Android_AHigh_SHigh", "previous_manifest");
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
            //string query = "select name,hash from manifests where name like \"chara%\" and name like \"%base%\" or name like \"card%\" and name like \"%petit%\" or name like \"card%\" and name like \"%live%\" or name like \"card%\" and name like \"%sign%\" or name like \"card%\" and name like \"%xl%\" or name like \"card%\" and name like \"%bg%\" or name like \"comic%\" and name like \"%m.%\"";
            //string query = "select name,hash from manifests where name like \"%237%.acb\"";
            //string query = "select name,hash from manifests where name like \"l/song%\"";
            //string query = "select name,hash from manifests where name like \"%.acb%\"";
            //string query = "select name,hash from manifests where name like \"story_storydata%\"";
            //string query = "select name,hash from manifests where name like \"card%\" and name like \"%xl%\"";
            //string query = "select name,hash from manifests where name like \"card%\" and name like \"%bg%\"";
            //string query = "select name,hash from manifests where name like \"gachaselect%\"";
            //string query = "select name,hash from manifests where name like \"chara%\" and name like \"%174%\" and name like \"%.unity3d\"";
            //string query = "select name,hash from manifests where name like \"%bgm_event_3005%\"";
            //string query = "select name,hash from manifests where name like \"item_%\"";
            //string query = "select name,hash from manifests where name like \"%chara_283%\" and name like \"%unity3d\"";
            //string query = "select name,hash from manifests where name like \"%stamp%\"";
            //string query = "select name,hash from manifests where name like \"v/event_announce_%\" or name like \"story_thumbnail%\"";
            //string query = "select name,hash from manifests where name like \"%photo_l%\"";
            //string query = "select name,hash from manifests where name like \"item_20001%\"";
            //string query = "select name,hash from manifests where name like \"%gachaselect_3%\"";
            //string query = "select name,hash from manifests where name like 'atlas_gacha_common.unity3d' or name like 'item_50006_s.unity3d' or name like 'item_50007_s.unity3d' or name like 'live_atlas_liveuiadd.unity3d' or name like 'loginbonus_bg_9005.unity3d' or name like 'banner_bnr_000047.unity3d' or name like 'jacket_1012.unity3d'";
            //string query = "select name,hash from manifests where name like \"%loginbonus_bg%\"";
            string query = "select name,hash from manifests";

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
                if (!stack1.Contains(reader["name"].ToString()))
                //if (!stack2.Contains(reader["hash"].ToString()))
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
                    File.Move(filehash, filename + ".lz4");
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
            foreach (string filehash in soundStack)
            {
                if (query == "select name,hash from manifests where name like \"" + filename + "\"")
                {
                    File.Move(filehash, filename.Split('/').Last());
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
            foreach (string filehash in blobDbStack)
            {
                if (query == "select name,hash from manifests where name like \"" + filename + "\"")
                {
                    File.Move(filehash, filename + ".lz4");
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
                    File.Move(filehash, filename + ".lz4");
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
            if (query == "select name,hash from manifests where name like \"" + filename + "\"" || query == "select name,hash from manifests where name like \"v/event_announce_%\" or name like \"story_thumbnail%\"")
            {
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
            if (query.Equals("select name,hash from manifests where name like \"card%\" and name like \"%bg%\""))
            {
                p.StartInfo.FileName = @"c:\windows\system32\cmd.exe";
                p.StartInfo.Arguments = "/c call create_montage.bat";
                p.StartInfo.WorkingDirectory = ".\\";
                p.Start();
                p.WaitForExit();
            }
            Clear();
        }

        static void Clear()
        {
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
                File.Delete("Android_AHigh_SHigh");
            }
            catch { }
            try
            {
                File.Delete("current_manifest");
            }
            catch { }
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
