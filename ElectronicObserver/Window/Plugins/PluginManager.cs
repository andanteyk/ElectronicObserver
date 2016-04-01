using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Threading;
using System.Web.Script.Serialization;
using ICSharpCode.SharpZipLib.Zip;

namespace ElectronicObserver.Window.Plugins
{
    public class PluginUpdateManager
    {
        public Dictionary<string, PluginUpdater> PluginUpdaters { get; private set; }
        static List<string> File2Update = new List<string>();
        public static string UpdatesFolder;
        public static string PluginsFolder;
        static void Wait2Update(string tempfile, string name)
        {
            File.Copy(tempfile, UpdatesFolder + "\\" + name, true);
            File2Update.Add(name);
        }
        public class PluginUpdater
        {
            Thread UpdateThread;
            public string Name { get; private set; }
            public IPluginHost PluginHost { get; private set; }
            public PluginUpdateInformation UpdateInformation { get; private set; }
            public PluginUpdateProgress UpdateProgress { get; private set; }

            public PluginUpdater(IPluginHost plugin)
            {
                PluginHost = plugin;
                Name = plugin.MenuTitle;
                UpdateProgress = new PluginUpdateProgress(this);
                UpdateInformation = plugin.UpdateInformation;

                switch (UpdateInformation.updateType)
                {
                    case PluginUpdateInformation.UpdateType.None:
                        UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.不可更新;
                        break;
                    case PluginUpdateInformation.UpdateType.Manual:
                        UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.手动更新;
                        UpdateProgress.Messages = "点击按钮手动下载该插件";
                        break;
                    default:
                        UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.尚未开始;
                        UpdateProgress.Messages = "点击按钮检查更新";
                        break;
                }
            }
            public bool Start()
            {
                if (UpdateProgress.Progress == PluginUpdateProgress.UpdatingProgress.尚未开始 ||
                    UpdateProgress.Progress == PluginUpdateProgress.UpdatingProgress.更新失败)
                {
                    UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.正在开始;
                    if (UpdateThread != null && UpdateThread.IsAlive)
                    {
                        return false;
                    }
                    UpdateThread = new Thread(Updating);
                    UpdateThread.IsBackground = true;
                    UpdateThread.Start(this);
                    return true;
                }

                if (UpdateProgress.Progress == PluginUpdateProgress.UpdatingProgress.等待下载 ||
                    UpdateProgress.Progress == PluginUpdateProgress.UpdatingProgress.下载失败)
                {
                    UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.正在下载;
                    if (UpdateThread != null && UpdateThread.IsAlive)
                    {
                        return false;
                    }
                    UpdateThread = new Thread(Updating);
                    UpdateThread.IsBackground = true;
                    UpdateThread.Start(this);
                    return true;
                }

                if (UpdateProgress.Progress == PluginUpdateProgress.UpdatingProgress.手动更新)
                {
                    System.Diagnostics.Process.Start(UpdateProgress.Updater.UpdateInformation.PluginDownloadURI);
                }
                return false;
            }
            public void Stop()
            {
                if (UpdateThread != null && UpdateThread.IsAlive)
                {
                    UpdateThread.Abort();
                    UpdateThread = null;
                }
            }

            void Updating(object Updater)
            {
                PluginUpdater PluginUpdater = (PluginUpdater)Updater;
                System.Net.WebClient client = new System.Net.WebClient();

                try
                {
                    if (PluginUpdater.UpdateProgress.Progress == PluginUpdateProgress.UpdatingProgress.正在开始)
                    {
                        client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
                        client.Encoding = Encoding.UTF8;
                        string result;
                        try
                        {
                            result = client.DownloadString(UpdateInformation.UpdateInformationURI);
                        }
                        catch
                        {
                            UpdateProgress.Messages = "无法获取更新信息文件";
                            UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.更新失败;
                            return;
                        }

                        try
                        {
                            JavaScriptSerializer Serializer = new JavaScriptSerializer();
                            var data = (Dictionary<string, object>)Serializer.DeserializeObject(result);
                            if (data.ContainsKey("Changelog"))
                                UpdateProgress.Changelog = data["Changelog"].ToString();
                            else
                                UpdateProgress.Changelog = null;

                            UpdateProgress.Version = data["Version"].ToString();

                            int ver = CompareVersion(UpdateProgress.Version, PluginUpdater.PluginHost.Version);
                            if (ver <= 0)
                            {
                                UpdateProgress.Messages = "该插件已经是最新";
                                UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.无需更新;
                                return;
                            }

                            UpdateProgress.PluginFileName = data["PluginFileName"].ToString();

                            if (data.ContainsKey("DownloadLink"))
                                UpdateProgress.DownloadLink = data["DownloadLink"].ToString();
                            else
                                UpdateProgress.DownloadLink = null;

                            if (data.ContainsKey("DownloadSite"))
                                UpdateProgress.DownloadSite = data["DownloadSite"].ToString();
                            else
                                UpdateProgress.DownloadSite = null;
                        }
                        catch
                        {
                            UpdateProgress.Messages = "更新信息文件格式错误";
                            UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.更新失败;
                            return;
                        }

                        if (UpdateProgress.DownloadLink == null)
                        {
                            UpdateProgress.Updater.UpdateInformation.PluginDownloadURI = UpdateProgress.DownloadSite;
                            UpdateProgress.Messages = "发现新版本(" + UpdateProgress.Version + ")!手动下载该插件";
                            UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.手动更新;
                            return;
                        }
                        else
                        {
                            UpdateProgress.Messages = "发现新版本(" + UpdateProgress.Version + ")!";
                            UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.等待下载;
                            return;
                        }
                    }
                    if (PluginUpdater.UpdateProgress.Progress == PluginUpdateProgress.UpdatingProgress.正在下载)
                    {
                        string temp = Path.GetTempFileName();
                        try
                        {
                            client.DownloadFile(UpdateProgress.DownloadLink, temp);
                            PluginUpdateManager.Wait2Update(temp, UpdateProgress.PluginFileName);
                        }
                        catch
                        {
                            UpdateProgress.Messages = "下载插件时发生错误";
                            UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.下载失败;
                            return;
                        }
                        UpdateProgress.Messages = "更新完成,重启软件后该插件生效";
                        UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.更新成功;
                    }
                }
                catch//ThreadAbort
                {
                    UpdateProgress.Progress = PluginUpdateProgress.UpdatingProgress.用户终止;
                }
            }

        }

        static PluginUpdateManager()
        {
            UpdatesFolder = Application.StartupPath + "\\Updates";
            PluginsFolder = Application.StartupPath + "\\Plugins";
        }
        public PluginUpdateManager(List<IPluginHost> Plugins)
        {
            PluginUpdaters = new Dictionary<string, PluginUpdater>();
            foreach (var plugin in Plugins)
            {
                PluginUpdater updater = new PluginUpdater(plugin);
                PluginUpdaters[plugin.MenuTitle] = updater;
            }
           
            if (!Directory.Exists(UpdatesFolder))
            {
                Directory.CreateDirectory(UpdatesFolder);
            }
        }

        public void Stop()
        {
            foreach (var pluginUpdater in PluginUpdaters.Values)
            {
                pluginUpdater.Stop();
            }
            File.WriteAllLines(UpdatesFolder + "\\Updates.lst", File2Update);
        }

        public static void ApplyUpdates()
        {
            string lstFile = UpdatesFolder + "\\Updates.lst";
			if (!File.Exists(lstFile))
				return;

            var files = File.ReadAllLines(lstFile);
            foreach (var file in files)
            {
                string updateFile = UpdatesFolder + "\\" + file;
                
                if (File.Exists(updateFile))
                {
                    if (Path.GetExtension(file).ToUpper() == ".PLUGIN")
                    {
                        ZipFile zip = new ZipFile(updateFile);
                        try
                        {
                            for (int i = 0; i < zip.Count; i++)
                            {
                                if (zip[i].IsFile)
                                {
                                    var stream = zip.GetInputStream(zip[i]);
                                    SaveStreamToFile(stream, zip[i].Name);
                                }
                            }
                        }
                        finally
                        {
                            zip.Close();
                        }
                    }
                    else
                    {
                        File.Copy(updateFile, PluginsFolder + "\\" + file, true);
                    }
                    ElectronicObserver.Utility.Logger.Add(2, "已经成功更新插件:" + file);
                    File.Delete(updateFile);
                }
            }
            File.Delete(lstFile);
          
            //zip.
        }

        static void SaveStreamToFile(Stream stream, string file)
        {
            byte[] buffer = new byte[65536];
            string name = Path.Combine(PluginsFolder, file);
            string dir = Path.GetDirectoryName(name);
            
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var streamWriter = new FileStream(name, FileMode.Create, FileAccess.Write);
            try
            {
                int count;
                while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    streamWriter.Write(buffer, 0, count);
                }
            }
            finally
            {
                streamWriter.Close();
            }
        }

        //From SoftwareInformation.cs
        private static int CompareVersion(string verRemote, string verLocal)
        {
            if (verRemote == verLocal)
                return 0;

            int t;
            int[] vr = verRemote.Split('.').Select(s => int.TryParse(s, out t) ? t : 0).ToArray();

            int[] vl = verLocal.Split('.').Select(s => int.TryParse(s, out t) ? t : 0).ToArray();

            if (vr.Length != vl.Length)
                return 2;

            // 2.0.0.0 -> 1.0.0.0
            if (vr[0] > vl[0])
            {
                return 2;
            }
            else if (vr[0] == vl[0])
            {

                // 2.1.0.0 -> 2.0.0.0
                if (vr[1] > vl[1])
                {
                    return 2;
                }
                else if (vr[1] == vl[1])
                {

                    // 2.1.1.0 -> 2.1.0.0
                    if (vr[2] > vl[2])
                    {
                        return 2;
                    }
                    else if (vr[2] == vl[2])
                    {

                        // 2.1.1.9 -> 2.1.1.0
                        if (vr[3] > vl[3])
                        {
                            return 1;
                        }
                    }
                }
            }

            return 0;
        }
    }

    public class PluginUpdateProgress
    {
        public enum UpdatingProgress
        {
            不可更新,
            尚未开始,
            正在开始,
            检查更新,
            无需更新,
            等待下载,
            正在下载,
            手动更新,
            更新失败,
            下载失败,
            用户终止,
            更新成功
        }

        UpdatingProgress progress;
        public UpdatingProgress Progress
        {
            get { return progress; }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    if (OnUpdateProgressChanged != null)
                        OnUpdateProgressChanged(this);
                }
            }
        }
        public PluginUpdateProgress(PluginUpdateManager.PluginUpdater PluginUpdater)
        {
            Updater = PluginUpdater;
        }

        public PluginUpdateManager.PluginUpdater Updater { get; private set; }

        public string PluginFileName { get; set; }
        public string Messages { get; set; }
        public string Changelog { get; set; }
        public string Version { get; set; }
        public string DownloadLink { get; set; }
        public string DownloadSite { get; set; }

        public event Action<PluginUpdateProgress> OnUpdateProgressChanged;

    }
    public class PluginUpdateInformation
    {
        public enum UpdateType
        {
            /// <summary>
            /// 不能自动更新
            /// </summary>
            None,
            /// <summary>
            /// 用户手动下载更新,需要提供下载地址(PluginDownloadURI)
            /// </summary>
            Manual,
            /// <summary>
            /// 提供一个UpdateInformationURI(json)调用插件管理器默认的自动更新过程,json文件格式必须满足要求
            /// </summary>
            Auto,
            /// <summary>
            /// 由插件通过代码实现DoUpdate来检查和更新
            /// (暂未实现)
            /// </summary>
            User
        }
        public PluginUpdateInformation(UpdateType type)
        {
            updateType = type;
        }

        public UpdateType updateType { get; private set; }
        /// <summary>
        /// 用于更新信息的网址,http:之类的必须加 不然无法下载,json格式参照UpdateFileFormat.json
        /// </summary>
        public string UpdateInformationURI { get; set; }
        public string PluginDownloadURI { get; set; }

        /// <summary>
        /// 如果选择User更新,那么需要挂接这个更新委托
        /// </summary>
        public event Func<PluginUpdateProgress, PluginUpdateInformation> DoUpdate;
    }

    public class PluginManager
    {
        FormMain Main = null;
        public Dictionary<string, ElectronicPlugin> Plugins
        {
            get;
            set;
        }

        public PluginManager(FormMain main)
        {
            Main = main;
            Plugins = new Dictionary<string, ElectronicPlugin>();
        }

        public bool AddPlugin(ElectronicPlugin Plugin)
        {
            Plugins[Plugin.PluginName] = Plugin;
            if (Plugin is IDialogPlugin)
            {
                IDialogPlugin plugin = Plugin as IDialogPlugin;
                var item = new ToolStripMenuItem
                {
                    Name = "ToolMenuItem_" + Plugin.PluginName,
                    Text = plugin.ToolMenuTitle,
                    Tag = plugin
                };
                if (Plugin.MenuIcon != null)
                    item.Image = Plugin.MenuIcon;
                item.Click += dialogPlugin_Click;
                ((ToolStripMenuItem)(Main.MainMenuStrip.Items["StripMenu_Tool"])).DropDownItems.Add(item);
            }
            if (Plugin is IDockPlugin)
            {
                IDockPlugin plugin = Plugin as IDockPlugin;
                DockContent dockContent = plugin.GetDockWindow() as DockContent;
                dockContent.HideOnClose = true;
                dockContent.Name = "DockWindow_" + Plugin.PluginName;
                var item = new ToolStripMenuItem
                {
                    Name = "ViewMenuItem_" + Plugin.PluginName,
                    Text = plugin.ViewMenuTitle,
                    Tag = dockContent
                };
                if (Plugin.MenuIcon != null)
                    item.Image = Plugin.MenuIcon;
                item.Click += menuitem_Click;
                ((ToolStripMenuItem)(Main.MainMenuStrip.Items["StripMenu_View"])).DropDownItems.Add(item);

                Main.SubForms.Add(dockContent);
            }
            bool b = Plugin.StartPlugin(Main);
            if (b)
                Utility.Logger.Add(2, string.Format("插件 {0}({1}) 已加载。", Plugin.PluginName, Plugin.Version));
            else
                Utility.Logger.Add(2, string.Format("插件 {0}({1}) 加载失败。", Plugin.PluginName, Plugin.Version));
            return b;

        }

        void dialogPlugin_Click(object sender, EventArgs e)
        {
            var plugin = (IDialogPlugin)((ToolStripMenuItem)sender).Tag;
            if (plugin != null)
            {
                try
                {
                    plugin.GetToolWindow().Show(Main);
                }
                catch (ObjectDisposedException) { }
                catch (Exception ex)
                {
                    Utility.ErrorReporter.SendErrorReport(ex, string.Format("插件显示出错：{0}", plugin.ToolMenuTitle));
                }
            }
        }

        void menuitem_Click(object sender, EventArgs e)
        {
            var f = ((ToolStripMenuItem)sender).Tag as DockContent;
            if (f != null)
            {
                f.Show(Main.MainPanel);
            }
        }

    }

    [Obsolete]
    public class PluginManagerObsolete
    {
        FormMain Main = null;
        public Dictionary<string, ElectronicPluginContainer> PluginContainers
        {
            get;
            set;
        }

        public PluginManagerObsolete(FormMain main)
        {
            Main = main;
            PluginContainers = new Dictionary<string, ElectronicPluginContainer>();
        }

        public bool StartPlugin(string name)
        {
            if (PluginContainers.ContainsKey(name))
            {
                if (!PluginContainers[name].Plugin.Active)
                {
                    var Plugin = PluginContainers[name].Plugin;
                    if (Plugin is IDialogPlugin)
                    {
                        IDialogPlugin plugin = Plugin as IDialogPlugin;
                        var item = new ToolStripMenuItem
                        {
                            Name = "ToolMenuItem_" + Plugin.PluginName,
                            Text = plugin.ToolMenuTitle,
                            Tag = plugin
                        };
                        if (Plugin.MenuIcon != null)
                            item.Image = Plugin.MenuIcon;
                        item.Click += dialogPlugin_Click;
                        ((ToolStripMenuItem)(Main.MainMenuStrip.Items["StripMenu_Tool"])).DropDownItems.Add(item);
                    }
                    if (Plugin is IDockPlugin)
                    {
                        IDockPlugin plugin = Plugin as IDockPlugin;
                        DockContent dockContent = plugin.GetDockWindow() as DockContent;
                        dockContent.HideOnClose = true;
                        dockContent.Name = "DockWindow_" + Plugin.PluginName;
                        var item = new ToolStripMenuItem
                        {
                            Name = "ViewMenuItem_" + Plugin.PluginName,
                            Text = plugin.ViewMenuTitle,
                            Tag = dockContent
                        };
                        if (Plugin.MenuIcon != null)
                            item.Image = Plugin.MenuIcon;
                        item.Click += menuitem_Click;
                        ((ToolStripMenuItem)(Main.MainMenuStrip.Items["StripMenu_View"])).DropDownItems.Add(item);

                        Main.SubForms.Add(dockContent);
                    }
                    Plugin.Active = true;
                    bool b = Plugin.StartPlugin(Main);
                    if (b)
                        Utility.Logger.Add(2, string.Format("插件 {0}({1}) 已启动。", Plugin.PluginName, Plugin.Version));
                    else
                        Utility.Logger.Add(2, string.Format("插件 {0}({1}) 启动失败。", Plugin.PluginName, Plugin.Version));
                    return b;
                }
            }
            return false;
        }

        void dialogPlugin_Click(object sender, EventArgs e)
        {
            var plugin = (IDialogPlugin)((ToolStripMenuItem)sender).Tag;
            if (plugin != null)
            {
                try
                {
                    plugin.GetToolWindow().Show(Main);
                }
                catch (ObjectDisposedException) { }
                catch (Exception ex)
                {
                    Utility.ErrorReporter.SendErrorReport(ex, string.Format("插件显示出错：{0}", plugin.ToolMenuTitle));
                }
            }
        }

        void menuitem_Click(object sender, EventArgs e)
        {
            var f = ((ToolStripMenuItem)sender).Tag as DockContent;
            if (f != null)
            {
                f.Show(Main.MainPanel);
            }
        }

        public bool StopPlugin(string name)
        {
            if (PluginContainers.ContainsKey(name))
            {
                if (PluginContainers[name].Plugin.Active)
                {
                    var Plugin = PluginContainers[name].Plugin;
                    if (Plugin is IDialogPlugin)
                    {
                        IDialogPlugin plugin = Plugin as IDialogPlugin;

                        string ItemName = "ToolMenuItem_" + Plugin.PluginName;
                        ((ToolStripMenuItem)(Main.MainMenuStrip.Items["StripMenu_Tool"])).DropDownItems.RemoveByKey(ItemName);
                    }
                    if (Plugin is IDockPlugin)
                    {
                        IDockPlugin plugin = Plugin as IDockPlugin;

                        string ItemName = "ViewMenuItem_" + Plugin.PluginName;
                        ((ToolStripMenuItem)(Main.MainMenuStrip.Items["StripMenu_View"])).DropDownItems.RemoveByKey(ItemName);
                        Main.SubForms.ForEach(e => { if (e.Name == "DockWindow_" + Plugin.PluginName) e.Close(); });
                        Main.SubForms.RemoveAll(e => e.Name == "DockWindow_" + Plugin.PluginName);
                    }
                    Plugin.Active = false;
                    bool b = Plugin.StopPlugin();
                    if (b)
                        Utility.Logger.Add(2, string.Format("插件 {0}({1}) 已停止。", Plugin.PluginName, Plugin.Version));
                    else
                        Utility.Logger.Add(2, string.Format("插件 {0}({1}) 停止失败。", Plugin.PluginName, Plugin.Version));
                    return b;
                }
            }
            return false;
        }
        public bool LoadPlugin(string dllPath)
        {
            var oldplugin = PluginContainers.Values.FirstOrDefault(e => e.PluginPath == dllPath);
            if (oldplugin != null)
                UnloadPlugin(oldplugin.Plugin.PluginName);

            ElectronicPluginContainer container = new ElectronicPluginContainer();
            var plugin = container.LoadPlugin(dllPath);
            if (plugin == null)
                return false;
            string pluginName = plugin.PluginName;

            PluginContainers[pluginName] = container;
            Utility.Logger.Add(2, string.Format("插件 {0}({1}) 已成功加载。", plugin.PluginName, plugin.Version));
            return true;
        }

        public bool UnloadPlugin(string name)
        {
            if (PluginContainers.ContainsKey(name))
            {
                StopPlugin(name);

                AppDomain.Unload(PluginContainers[name].appDomain);
                Utility.Logger.Add(2, string.Format("插件 {0}({1}) 已成功卸载。", PluginContainers[name].Plugin.PluginName, PluginContainers[name].Plugin.Version));
                PluginContainers.Remove(name);
                return true;
            }
            return false;
        }


    }
    [Obsolete]
    public class ElectronicPluginContainer
    {
        public ElectronicPlugin Plugin
        {
            get;
            set;
        }
        public string DomainName
        {
            get;
            set;
        }

        public AppDomain appDomain
        {
            get;
            set;
        }

        public string PluginPath
        {
            get;
            set;
        }

        public ElectronicPlugin LoadPlugin(string dllPath)
        {
            DomainName = Path.GetFileNameWithoutExtension(dllPath);
            appDomain = AppDomain.CreateDomain(DomainName);

            string name = Path.GetFileName(Application.ExecutablePath);
            ElectronicPluginFactory factory = (ElectronicPluginFactory)appDomain.CreateInstanceFromAndUnwrap(name, typeof(ElectronicPluginFactory).FullName);
            factory.LoadAssembly(dllPath);
            Plugin = factory.Invoke();

            if (Plugin == null)
            {
                AppDomain.Unload(appDomain);
                return null;
            }
            appDomain.UnhandledException += appDomain_UnhandledException;
            return Plugin;
        }

        void appDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            MessageBox.Show(ex.ToString(), "ElectronicObserver", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Utility.ErrorReporter.SendErrorReport(ex, DomainName + "插件中错误：" + ex.Message);
        }
    }
    [Obsolete]
    class ElectronicPluginFactory : MarshalByRefObject
    {
        Assembly assembly = null;

        public void LoadAssembly(string dll)
        {
            string path = Application.StartupPath;
            assembly = Assembly.LoadFile(dll);
        }
        public ElectronicPlugin Invoke()
        {
            if (assembly == null)
                return null;
            var types = assembly.GetExportedTypes();
            foreach (var Type in types)
            {
                if (Type.IsSubclassOf(typeof(ElectronicPlugin)))
                {
                    var plugin = assembly.CreateInstance(Type.FullName) as ElectronicPlugin;
                    return plugin;
                }
            }
            return null;
        }
    }
}
