using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Collections.Generic;

public class AssetBunlids
{
    /// <summary>
    /// 打包平台
    /// </summary>
    public static BuildTarget target
    {
        get
        {
            Platform plat = Global.platform;
            if (plat == Platform.Android)
                return UnityEditor.BuildTarget.Android;
            else if (plat == Platform.IOS)
                return UnityEditor.BuildTarget.iPhone;
            else
                return UnityEditor.BuildTarget.StandaloneWindows;
        }
    }
    /// <summary>
    /// 打包多个文件
    /// </summary>
    //[MenuItem("Custom Editor/Create AssetBunldes Main")]
    public static void CreateAssetBunldesMain()
    {
        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        //遍历所有的游戏对象
        foreach (Object obj in SelectedAsset)
        {
            //string sourcePath = AssetDatabase.GetAssetPath(obj);
            //本地测试：建议最后将Assetbundle放在StreamingAssets文件夹下，如果没有就创建一个，因为移动平台下只能读取这个路径
            //StreamingAssets是只读路径，不能写入
            //服务器下载：就不需要放在这里，服务器上客户端用www类进行下载。
            string targetPath = Application.dataPath + "/StreamingAssets/" + obj.name + ".assetbundle";
            //单个打包
            if (BuildPipeline.BuildAssetBundle(obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, AssetBunlids.target))
                Trace.Log(obj.name + "资源打包成功");
            else
                Trace.Log(obj.name + "资源打包失败");
        }
        //刷新编辑器
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 打包整个文件
    /// </summary>
    //[MenuItem("Custom Editor/Create AssetBunldes ALL")]
    public static void CreateAssetBunldesALL()
    {
        //清除WWW.LoadFromCacheOrDownload缓存
        Caching.CleanCache();
        //打包路径
        string Path = Application.dataPath + "/StreamingAssets/ALL.assetbundle";
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        foreach (Object obj in SelectedAsset)
        {
            Trace.Log("Create AssetBunldes name :" + obj);
        }

        //打包列表
        if (BuildPipeline.BuildAssetBundle(null, SelectedAsset, Path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, AssetBunlids.target))
        {
            //重新加载资源让其在Project中可见
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// 分布级别打包
    /// </summary>
    //[MenuItem("Custom Editor/Create AssetBunldes Distributed")]
    public static void CreateAssetBunldesDis()
    {
        //设置打包资源依赖关系，完整资源引用（共享资源引用），固定资源ID（支持资源包重建）
        BuildAssetBundleOptions options =
        BuildAssetBundleOptions.CollectDependencies |
        BuildAssetBundleOptions.CompleteAssets |
        BuildAssetBundleOptions.DeterministicAssetBundle;
        //开启依赖级别1
        BuildPipeline.PushAssetDependencies();
        //1.图片类资源打包
        BuildPipeline.BuildAssetBundle(
        AssetDatabase.LoadMainAssetAtPath("assets/artwork/lerpzuv.tif"),
        null, "Shared.unity3d", options);


        //2.Mesh

        //3.Sence

        //关闭依赖级别1
        BuildPipeline.PopAssetDependencies();
    }

}

public class BuildAssetBundlesFromDirectory
{
    /// <summary>
    /// 过滤不打包的文件类型
    /// </summary>
    private static string[] s_szSkipType = new string[]
    {
    ".unity3d",
    ".log",
    ".db",
    ".meta",
    };

    /// <summary>
    /// 输出路径
    /// </summary>
    static string s_sOutPutPath = Application.dataPath + "/StreamingAssets/build/";

    /// <summary>
    /// 输出设置
    /// </summary>
    static BuildAssetBundleOptions s_nBuildOptions = BuildAssetBundleOptions.CollectDependencies |
                                                        BuildAssetBundleOptions.CompleteAssets |
                                                        BuildAssetBundleOptions.DeterministicAssetBundle;

    [MenuItem("Custom Editor/Raname")]
    public static void Raname()
    {
        string file = AssetDatabase.GetAssetPath(Selection.activeObject);
        //查找单个目录下的所有文件对象
        List<Object> findobjlist = GetAllFileInDirectory(file);

        //创建输出目录
        AssetDatabase.CreateFolder(file, "Raname");
        foreach (Object obj in findobjlist)
        {
            string name = AssetDatabase.GetAssetPath(obj);
            string newname = name.Replace("CFX", "CFXM").Replace(file, file + "Raname/");
            Trace.Log(name + "   " + newname);
            AssetDatabase.CopyAsset(name, newname);
        }
    }
    /// <summary>
    /// 过滤不打包的文件类型
    /// </summary>
    static bool CheckRule(string filename)
    {
        foreach (string s in BuildAssetBundlesFromDirectory.s_szSkipType)
        {
            if (filename.EndsWith(s, System.StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 过滤不打包的文件类型
    /// </summary>
    static bool CheckRule(string filename, string[] exrule)
    {
        foreach (string s in BuildAssetBundlesFromDirectory.s_szSkipType)
        {
            if (filename.EndsWith(s, System.StringComparison.OrdinalIgnoreCase))
                return true;
        }
        foreach (string s in exrule)
        {
            if (filename.EndsWith(s, System.StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 根据AssetLevelItem列表打包文件
    /// </summary>
    public static int AssetBundlesByAssetLevelItem(List<AssetLevelItem> assetLevels)
    {
        int nNormalFile = 0;
        int nSencesFile = 0;
        string time = System.DateTime.Now.ToString("yyyy-MM-dd  HH：mm：ss");

        BuildPipeline.PushAssetDependencies();
        foreach (AssetLevelItem level in assetLevels)
        {
            if (level.AssetPackageList == null || level.AssetPackageList.Count == 0)
                continue;
            //创建输出目录
            Directory.CreateDirectory(BuildAssetBundlesFromDirectory.s_sOutPutPath + time + "/" + level.Level + "/");
            foreach (AssetPackageItem package in level.AssetPackageList)
            {
                if (package.FilesList == null || package.FilesList.Count == 0)
                    continue;
                List<Object> objs = new List<Object>();
                List<string> sences = new List<string>();

                foreach (AssetFileItem filepath in package.FilesList)
                {
                    int filetype = filepath.GetFileType();
                    //判定路径是否是目录,如果是目录按目录查找,如果是文件，直接判定和加入打包列表
                    if (filetype == AssetFileItem.FILETYPE_DOC)
                    {
                        //查找单个目录下的所有文件对象
                        List<Object> findobjlist = GetAllFileInDirectory(filepath.FilePath);
                        if (findobjlist.Count > 0)
                            objs.AddRange(findobjlist);
                        else
                        {
                            //如果文件对象数量为0,尝试查找sence对象
                            List<string> findsencelist = GetAllSencePathInDirectory(filepath.FilePath);
                            if (findsencelist.Count > 0)
                                sences.AddRange(findsencelist);
                        }
                    }
                    else if (filetype == AssetFileItem.FILETYPE_NORMAL)
                    {
                        string fileName = filepath.FilePath.Replace("\\", "/");
                        int startindex = fileName.IndexOf("Assets/");
                        int endindex = fileName.LastIndexOf("/");
                        //获取相对文件路径
                        string filePath = fileName.Substring(startindex, endindex - startindex);
                        //获取文件名
                        fileName = fileName.Substring(endindex);
                        //检测文件类型，去处不需要打包的文件
                        if (CheckRule(fileName))
                        {
                            //Debug.Log("Skip:" + fileName);
                            continue;
                        }
                        string localPath = filePath;
                        if (endindex > 0)
                            localPath += fileName;
                        Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
                        if (t == null)
                        {
                            Trace.LogWarning("BuildOnceDirectory NotFound:" + fileName);
                            continue;
                        }
                        objs.Add(t);
                    }
                    else if (filetype == AssetFileItem.FILETYPE_SENCE)
                    {
                        sences.Add(filepath.FilePath);
                    }
                }

                string bundlePath = BuildAssetBundlesFromDirectory.s_sOutPutPath + time + package.GetPackageFile(level.Level);

                //打包对象
                if (objs.Count > 0)
                {
                    if (level.Level == 2)
                    {
                        //BuildPipeline.PushAssetDependencies();
                        if (!BuildPipeline.BuildAssetBundle(null, objs.ToArray(), bundlePath, s_nBuildOptions, AssetBunlids.target))
                        {
                            Trace.LogError("BuildOneAssetBundle file Error: lv:" + level.Level + "  name:" + level.LevelName + "  packagename:" + package.PackageName + "  bundlePath:" + bundlePath);
                            break;
                        }
                        //BuildPipeline.PopAssetDependencies();
                    }
                    else
                    {
                        if (package.IsSingle)
                        {
                            string bundlePathSingle = BuildAssetBundlesFromDirectory.s_sOutPutPath + time + "/" + level.Level + "/" + package.PackageName;
                            Directory.CreateDirectory(bundlePathSingle);

                            objs.ForEach(delegate(Object obj)
                            {
                                if (!BuildPipeline.BuildAssetBundle(obj, null, bundlePathSingle + "/" + obj.name + Global.AssetPackageSuffix,
                                    s_nBuildOptions, AssetBunlids.target))
                                {
                                    Trace.LogError("BuildOneAssetBundle file Error: lv:" + level.Level + "  name:" + level.LevelName + "  packagename:" + package.PackageName + "  bundlePath:" + bundlePath);
                                }
                            });
                        }
                        else
                            if (!BuildPipeline.BuildAssetBundle(null, objs.ToArray(), bundlePath, s_nBuildOptions, AssetBunlids.target))
                            {
                                Trace.LogError("BuildOneAssetBundle file Error: lv:" + level.Level + "  name:" + level.LevelName + "  packagename:" + package.PackageName + "  bundlePath:" + bundlePath);
                                break;
                            }
                    }

                    Trace.Log("BuildOneAssetBundle file Done: lv:" + level.Level + "  name:" + level.LevelName + "  packagename:" + package.PackageName + "  finish number:" + objs.Count + "  bundlePath:" + bundlePath);
                    nNormalFile += objs.Count;
                    objs.Clear();
                    objs = null;
                }
                else if (sences.Count > 0)
                {
                    BuildPipeline.PushAssetDependencies();
                    BuildPipeline.BuildStreamedSceneAssetBundle(sences.ToArray(), bundlePath, AssetBunlids.target);
                    Trace.Log("BuildOneAssetBundle sence Done: lv:" + level.Level + "  name:" + level.LevelName + "  packagename:" + package.PackageName + "  finish number:" + sences.Count + "  bundlePath:" + bundlePath);
                    nSencesFile += sences.Count;
                    sences.Clear();
                    sences = null;
                    BuildPipeline.PopAssetDependencies();
                }
            }
        }
        BuildPipeline.PopAssetDependencies();
        Trace.Log("BuildFinished finish number: normalfile:" + nNormalFile + "  sencefile:" + nSencesFile);
        AssetDatabase.Refresh();
        return nNormalFile + nSencesFile;
    }

    /// <summary>
    /// 打包文件夹,包含子文件夹所有文件打包成一个文件
    /// </summary>
    //[MenuItem("AssetBuild/MixAll AssetBundles From All Directory of Files")]
    static void ExportAssetBundles()
    {
        // Get the selected directory
        //获取选择的目录
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        Trace.Log("Selected Folder: " + path);
        if (path.Length != 0)
        {
            path = path.Replace("Assets/", "");
            path = Application.dataPath + "/" + path;
            int num = BuildDirectoryInOneFile(path, "test");
            Trace.Log("ExportAssetBundles finish number:" + num);
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// 文件夹内每一个文件单独打包
    /// </summary>
    //[MenuItem("AssetBuild/BuildAll AssetBundles From All Directory of Files")]
    static void ExportAssetAllBundles()
    {
        // Get the selected directory
        //获取选择的目录
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path.Length != 0)
        {
            path = path.Replace("Assets/", "");
            //获取该目录下所有文件(包含子目录)
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path, "*.*", SearchOption.AllDirectories);
            foreach (string filePathName in fileEntries)
            {
                string fileName = filePathName.Replace("\\", "/");
                int endindex = fileName.LastIndexOf("/");
                int startindex = fileName.IndexOf("Assets/");
                //获取相对文件路径
                string filePath = fileName.Substring(startindex, endindex - startindex);
                //获取文件名
                fileName = fileName.Substring(endindex);

                //检测文件类型，去处不需要打包的文件
                if (CheckRule(fileName))
                {
                    //Debug.Log("Skip:" + fileName);
                    continue;
                }
                Trace.Log("Making:" + fileName);
                string localPath = filePath;
                if (endindex > 0)
                    localPath += fileName;
                BuildOnceFile(fileName, filePath, localPath);

            }
            AssetDatabase.Refresh();
        }
    }

    //[MenuItem("AssetBuild/TestBuild")]
    static void TestAssetBundles()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        Trace.Log("Selected Folder: " + path);
        //         Object t = AssetDatabase.LoadMainAssetAtPath("Assets/Script/AssetBulid.cs");
        //         Debug.Log(t);
        //         string bundlePath = Application.dataPath + "/StreamingAssets/build/test.assetbundle";
        //         string[] levels = new string[] { "Assets/DynamicElements_Effects/effects.unity" };
        //         BuildPipeline.BuildStreamedSceneAssetBundle(levels, bundlePath, AssetBunlids.target);
    }

    ///<summary>
    ///打包单个文件
    ///</summary>
    ///<param name="fileName">文件名</param>
    ///<param name="filePath">文件相对路径</param>
    ///<param name="localPath">文件相对路径+文件名</param>
    ///<returns>
    ///返回是否成功打包
    ///</returns>
    static bool BuildOnceFile(string fileName, string filePath, string localPath)
    {
        Trace.Log("load:" + localPath);
        //从相对路径中去文件
        Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
        if (t != null)
        {
            Directory.CreateDirectory(BuildAssetBundlesFromDirectory.s_sOutPutPath + filePath + "/");
            string bundlePath = BuildAssetBundlesFromDirectory.s_sOutPutPath + filePath + "/" + t.name;
            Trace.Log("Building bundle at: " + bundlePath);
            // Build the resource file from the active selection.
            //从激活的选择编译资源文件
            if (fileName.EndsWith(".unity"))
            {
                string[] levels = new string[] { localPath };
                BuildPipeline.BuildStreamedSceneAssetBundle(levels, bundlePath, AssetBunlids.target);
                return true;
            }
            else
            {
                return BuildPipeline.BuildAssetBundle(t, null, bundlePath, s_nBuildOptions, AssetBunlids.target);
            }

        }
        Trace.Log("Buildfail:" + fileName);
        return false;
    }

    ///<summary>
    ///获得单个文件夹所有Sence文件相对路径(包含子目录)
    ///</summary>
    ///<param name="directorypath">文件夹绝对路径</param>
    ///<returns>
    ///返回路径列表
    ///</returns>
    static List<string> GetAllSencePathInDirectory(string directorypath)
    {
        List<string> pathlist = new List<string>();
        //获得本目录Sence
        pathlist.AddRange(GetSencePathInDirectory(directorypath));
        //获取该目录下所有目录(包含子目录)
        string[] fileDirectory = Directory.GetDirectories(directorypath, "*.*", SearchOption.AllDirectories);

        foreach (string path in fileDirectory)
        {
            //获得子目录Sence
            pathlist.AddRange(GetSencePathInDirectory(path));
        }

        return pathlist;
    }

    /// <summary>
    /// 获得单个文件夹所有Sence文件相对路径(不包含子目录)
    /// </summary>
    /// <param name="directorypath">文件夹绝对路径</param>
    /// <returns></returns>
    static List<string> GetSencePathInDirectory(string directorypath)
    {
        List<string> pathlist = new List<string>();
        //查找该文件夹内所有文件
        string[] fileEntries = Directory.GetFiles(directorypath);
        if (fileEntries.Length == 0)
            return pathlist;

        string sName = directorypath.Replace("\\", "/");
        int startindex = sName.IndexOf("Assets/");
        //获取相对文件路径
        string filePath = sName.Substring(startindex);
        foreach (string filePathName in fileEntries)
        {
            string fileName = filePathName.Replace("\\", "/");
            int endindex = fileName.LastIndexOf("/");
            //获取文件名
            fileName = fileName.Substring(endindex);

            //检测文件类型，只打包sence
            if (!fileName.EndsWith(".unity"))
            {
                continue;
            }
            string localPath = filePath;
            if (endindex > 0)
                localPath += fileName;
            pathlist.Add(localPath);
        }

        return pathlist;
    }

    #region 按单个文件夹打包,并且打包成一个文件

    ///<summary>
    ///打包单个文件夹(包含子目录),并且打包成一个文件,注意:不能支持打包Sence,sence需要单独打包
    ///</summary>
    ///<param name="fileName">文件夹绝对路径</param>
    ///<returns>
    ///返回打包文件数量
    ///</returns>
    static int BuildDirectoryInOneFile(string directorypath, string filename)
    {
        List<Object> targhts = GetAllFileInDirectory(directorypath);
        Directory.CreateDirectory(BuildAssetBundlesFromDirectory.s_sOutPutPath + "/");
        string bundlePath = BuildAssetBundlesFromDirectory.s_sOutPutPath + "/" + filename;
        if (!BuildPipeline.BuildAssetBundle(null, targhts.ToArray(), bundlePath, s_nBuildOptions, AssetBunlids.target))
        {
            Trace.LogError("BuildDirectoryInOneFile Error: " + bundlePath);
            return 0;
        }
        Trace.Log("BuildDirectoryInOneFile Done:" + bundlePath + "  finish number:" + targhts.Count);

        return targhts.Count;
    }

    ///<summary>
    ///获得单个文件夹所有文件对象(包含子目录),注意:不能支持打包Sence,sence需要单独打包
    ///</summary>
    ///<param name="fileName">文件夹绝对路径</param>
    ///<returns>
    ///返回对象列表
    ///</returns>
    static List<Object> GetAllFileInDirectory(string directorypath)
    {
        List<Object> targhts = new List<Object>();
        //获得本目录对象
        targhts.AddRange(GetFileInDirectory(directorypath));
        //获取该目录下所有目录(包含子目录)
        string[] fileDirectory = Directory.GetDirectories(directorypath, "*.*", SearchOption.AllDirectories);

        foreach (string path in fileDirectory)
        {
            //获得子目录对象
            targhts.AddRange(GetFileInDirectory(path));
        }

        return targhts;
    }

    ///<summary>
    ///获得单个文件夹所有文件对象(不包含子目录),注意:不能支持打包Sence,sence需要单独打包
    ///</summary>
    ///<param name="fileName">文件夹绝对路径</param>
    ///<returns>
    ///返回对象列表
    ///</returns>
    static List<Object> GetFileInDirectory(string directorypath)
    {
        List<Object> targhts = new List<Object>();
        //查找该文件夹内所有文件
        string[] fileEntries = Directory.GetFiles(directorypath);
        if (fileEntries.Length == 0)
            return targhts;

        string sName = directorypath.Replace("\\", "/");
        int startindex = sName.IndexOf("Assets/");
        //获取相对文件路径
        string filePath = sName.Substring(startindex);
        foreach (string filePathName in fileEntries)
        {
            string fileName = filePathName.Replace("\\", "/");
            int endindex = fileName.LastIndexOf("/");
            //获取文件名
            fileName = fileName.Substring(endindex);
            //检测文件类型，去处不需要打包的文件
            if (CheckRule(fileName, new string[] { ".unity" }))
            {
                //Debug.Log("BuildOnceDirectory Skip:" + fileName);
                continue;
            }
            string localPath = filePath;
            if (endindex > 0)
                localPath += fileName;
            Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
            if (t == null)
            {
                Trace.LogWarning("BuildOnceDirectory NotFound:" + fileName);
                continue;
            }
            targhts.Add(t);
        }

        return targhts;
    }

    #endregion

    #region 按单个文件夹打包
    ///<summary>
    ///打包单个文件夹(包含子目录),注意:不能支持打包Sence,sence需要单独打包
    ///</summary>
    ///<param name="fileName">文件夹绝对路径</param>
    ///<returns>
    ///返回打包成功数量
    ///</returns>
    static int BuildDirectory(string directorypath)
    {
        int nFinish = 0;
        //获取该目录下所有目录(包含子目录)
        string[] fileDirectory = Directory.GetDirectories(directorypath, "*.*", SearchOption.AllDirectories);
        foreach (string path in fileDirectory)
        {
            //打包子目录
            nFinish += BuildDirectory(path);
        }
        //打包本目录
        nFinish += BuildOnceDirectory(directorypath);
        Trace.Log("BuildDirectory finish number:" + nFinish);
        return nFinish;
    }

    ///<summary>
    ///打包单个文件夹(不包含子目录),注意:不能支持打包Sence,sence需要单独打包
    ///</summary>
    ///<param name="fileName">文件夹绝对路径</param>
    ///<returns>
    ///返回打包成功数量
    ///</returns>
    static int BuildOnceDirectory(string directorypath)
    {
        int nFinish = 0;
        //查找该文件夹内所有文件
        string[] fileEntries = Directory.GetFiles(directorypath);
        if (fileEntries.Length == 0)
            return nFinish;

        string sName = directorypath.Replace("\\", "/");
        int endindex = sName.LastIndexOf("/");
        int startindex = sName.IndexOf("Assets/");
        //获取相对文件路径
        string filePath = sName.Substring(startindex);
        //获取文件夹名
        sName = sName.Substring(endindex);
        Object[] targhts = new Object[fileEntries.Length];
        foreach (string filePathName in fileEntries)
        {
            string fileName = filePathName.Replace("\\", "/");
            endindex = fileName.LastIndexOf("/");
            //获取文件名
            fileName = fileName.Substring(endindex);
            //检测文件类型，去处不需要打包的文件
            if (CheckRule(fileName, new string[] { ".unity" }))
            {
                //Debug.Log("BuildOnceDirectory Skip:" + fileName);
                continue;
            }
            string localPath = filePath;
            if (endindex > 0)
                localPath += fileName;
            Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
            if (t == null)
            {
                Trace.LogWarning("BuildOnceDirectory NotFound:" + fileName);
                continue;
            }
            targhts[nFinish++] = t;
        }
        if (nFinish == 0)
            return 0;
        Directory.CreateDirectory(BuildAssetBundlesFromDirectory.s_sOutPutPath + filePath + "/");
        string bundlePath = BuildAssetBundlesFromDirectory.s_sOutPutPath + filePath + "/" + sName;
        if (!BuildPipeline.BuildAssetBundle(null, targhts, bundlePath, s_nBuildOptions, AssetBunlids.target))
        {
            Trace.LogError("BuildOnceDirectory Error: " + bundlePath);
            return 0;
        }
        Trace.Log("BuildOnceDirectory Done:" + bundlePath + "  finish number:" + nFinish);
        return nFinish;
    }

    #endregion


}