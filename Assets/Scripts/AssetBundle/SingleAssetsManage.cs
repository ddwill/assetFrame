using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// 单资源包管理
/// </summary>
public class SingleAssetsManage : AssetsManage
{

    public delegate void OnLoadSingleAssets(SingleAssetData data);
    /// <summary>
    /// 单资源包字典
    /// </summary>
    private static List<SingleAssetData>[] assetDate = new List<SingleAssetData>[(int)AssetType.Count];

    /// <summary>
    /// 获取单资源包,如果资源包不在内存则加载一次
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="onLoad"></param>
    public static void FindSingleAssetData(MonoBehaviour mono, string name, AssetType type, OnLoadSingleAssets onLoad)
    {
        //单资源包列表
        SingleAssetData data = FindAssetByType(name, type);
        if (data == null)
        {
            LoadResByPath(mono, name, type, delegate(object obj)
            {
                //Debug.LogError("loadOver!!!!!!!!!!!!!!!!!");
                onLoad(FindAssetByType(name, type));
            }, true, true);
        }
        else
        {
            //如果存在则，判断是否在卸载列表中，如果在则移除
            RemoveUnLoad(name, type);
            if (onLoad != null)
            {
                onLoad(data);
            }
        }
    }
    /// <summary>
    /// 获取单资源包按类型
    /// </summary>
    /// <param name="Name">资源包名</param>
    /// <param name="type">type</param>
    /// <returns></returns>
    private static SingleAssetData FindAssetByType(string name, AssetType type)
    {
        //单资源包列表
        List<SingleAssetData> singleAssets = assetDate[(int)type];
        SingleAssetData data = null;
        if (singleAssets != null && singleAssets.Count > 0)
        {
            data = FindAssetByLiset(name, singleAssets);
        }
        else
        {
            singleAssets = new List<SingleAssetData>();
            assetDate[(int)type] = singleAssets;
        }
        //Debug.LogError(name + " " + data + " " + type + " " + singleAssets.Count);
        return data;
    }

    /// <summary>
    /// 获取单资源包按列表
    /// </summary>
    /// <param name="Name">资源包名</param>
    /// <param name="singleAssets">单资源包</param>
    /// <returns></returns>
    private static SingleAssetData FindAssetByLiset(string Name, List<SingleAssetData> singleAssets)
    {
        //遍历资源列表
        foreach (SingleAssetData data in singleAssets)
        {
            if (data.Name == Name)
                return data;
        }
        return null;
    }
    /// <summary>
    /// 根据path列表加载资源
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    /// <param name="isAsyn">是否异步加载</param>
    public static void LoadResByPath(MonoBehaviour mono, string name, AssetType type, signal.osnet.signals.Signal.listen callback, bool isAsyn)
    {
        LoadResByPath(mono, name, type, callback, false, false);
    }

    private static void LoadResByPath(MonoBehaviour mono, string name, AssetType type, signal.osnet.signals.Signal.listen callback, bool isAsyn, bool isFind)
    {
        singleAssetEvents.onLoadOk.addOnce(callback);
        //SingleAssetsManage.isAsyn = isAsyn;
        //加载单个资源也拼成批量加载的模式，节省代码量
        List<SALoadItem> names = new List<SALoadItem>();
        names.Add(new SALoadItem(type, name));
        LoadInfos(mono, names, isFind);
    }

    /// <summary>
    /// 根据path列表加载资源
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="names"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    /// <param name="isAsyn">是否异步加载</param>
    public static void LoadResByPathList(MonoBehaviour mono, List<SALoadItem> names, signal.osnet.signals.Signal.listen callback, bool isAsyn)
    {
        singleAssetEvents.onLoadOk.addOnce(callback);
        //SingleAssetsManage.isAsyn = isAsyn;
        LoadInfos(mono, names, false);
    }

    //private static bool isAsyn = false;

    /// <summary>
    /// 是否注册场景切换时，清除Load列表的事件
    /// </summary>
    private static bool isRegClearEvent = false;
    /// <summary>
    /// 加载信息
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="names"></param>
    /// <param name="type"></param>
    /// <param name="isFind">已经找过了，不需要再遍历列表查看是否已经在内存中</param>
    private static void LoadInfos(MonoBehaviour mono, List<SALoadItem> names, bool isFind)
    {
        if (!isRegClearEvent)
        { 
            isRegClearEvent = true;
//            MonoMainScene.onOpenScenes += ClearLoad;
        }
        //单资源包列表
        List<SingleAssetData> singleAssets = null;
        List<SALoadItem> loadNames = new List<SALoadItem>();

        //判断资源包是否已经存在
        foreach (SALoadItem load in names)
        {
            if (!isFind)
            {
                singleAssets = assetDate[(int)load.type];
                if (singleAssets == null)
                    singleAssets = new List<SingleAssetData>();

                if (singleAssets.Count > 0)
                    if (FindAssetByLiset(load.name, singleAssets) == null)
                        loadNames.Add(load);
                    else
                    {
                        //如果存在则，判断是否在卸载列表中，如果在则从卸载列表中移除
                        RemoveUnLoad(load.name, load.type);
                    }
                else
                    loadNames.Add(load);
            }
            else
                loadNames.Add(load);
        }

        //Debug.LogError("WillLoad loadNames: " + loadNames.Count + " " + (NowLoadNames != null ? NowLoadNames.Count : 0) + " " + isLoad);
        //不在内存中需要加载列表
        if (loadNames.Count > 0)
        {
            if (isLoad)
            {
                //当前加载列表中如果
                if (NowLoadNames != null && NowLoadNames.Count > 0)
                {
                    bool ishas = false;
                    foreach (SALoadItem loadname in loadNames)
                    {
                        foreach (SALoadItem nowname in NowLoadNames)
                        {
                            if (nowname.name == loadname.name && nowname.type == loadname.type)
                                ishas = true;
                        }
                        if (!ishas)
                        {
                            Debug.Log("Add:" + loadname.name);
                            NowLoadNames.Add(loadname);
                        }
                    }
                }
            }
            else
            {
                wwwHelp = new WWWHelp();
                //赋值当前加载列表
                NowLoadNames = loadNames;
                //Debug.Log(isAsyn);
                //同步加载暂停
                //if (!isAsyn)
                //    Time.timeScale = 0;

                isLoad = true;
//                UIMask.Open();
                //链式加载
                LoadChainAsset(mono);
            }
        }
        else
        {
            singleAssetEvents.onLoadOk.dispatch(null);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="names"></param>
    /// <param name="type"></param>
    private static void RemoveUnLoad(string name, AssetType type)
    {
        //如果存在则，判断是否在卸载列表中，如果在则移除
        if (UnLoadAssets != null && UnLoadAssets.Count > 0)
            foreach (SingleAssetData unAsset in UnLoadAssets)
            {
                if (unAsset.Name == name && unAsset.type == type)
                {
                    UnLoadAssets.Remove(unAsset);
                    return;
                }
            }
    }

    /// <summary>
    /// 当前加载列表
    /// </summary>
    public static List<SALoadItem> NowLoadNames;

    /// <summary>
    /// 清除当前Load链
    /// </summary>
    public static void ClearLoad()
    {
        //Debug.LogError("ClearLoad");
        isLoad = false;
        NowLoadNames.Clear();
    }

    /// <summary>
    /// 是否加载中
    /// </summary>
    public static bool isLoad = false;

    /// <summary>
    /// 资源加载辅助类
    /// </summary>
    public static WWWHelp wwwHelp;

    /// <summary>
    /// 链式加载网络资源
    /// </summary>
    private static void LoadChainAsset(MonoBehaviour mono)
    {
        if (NowLoadNames != null && NowLoadNames.Count > 0)
        {
            //当前加载资源名
            SALoadItem load = NowLoadNames[0];
            //是否是网络资源
            AssetVersionItem assetVersion = isWebAsset(load.name);
            if (assetVersion != null)
            {
                //未缓存则从网络端下载资源
                mono.StartCoroutine(wwwHelp.LoadCacheAssetBundle(WWW_NAME + assetVersion.PackageName, assetVersion.Version, delegate(WWW www)
                {
                    if (www.assetBundle != null)
                    {
                        //下载完毕赋值,立刻解压缩内存
                        SingleAssetData data = new SingleAssetData(www.assetBundle, www.assetBundle.Load(load.name), load.type);
                        //卸载压缩资源
                        data.UnLoad(false);
                        assetDate[(int)load.type].Add(data);
                    }
                    NowLoadNames.Remove(load);
                    //继续加载下一个
                    LoadChainAsset(mono);
                }));
            }
            else
            {
                //本地资源路径
                string path = GetLoadPath(load);
                //Debug.Log("Load:" + path);
                //IO资源为byte[]
                LoadStreamingAssets.LoadAssetData(path, mono, delegate(byte[] asset)
                {
                    mono.StartCoroutine(GetAssetBundle(asset, load, mono));
                });
            }
        }
        else
        {
            //加载完毕
            isLoad = false;
//            UIMask.Close();
            //同步加载重启
            //if (!isAsyn)
            //Time.timeScale = Global.GetInstance().gameConfigData.GameTimeScale;
            //assetDate[(int)NowType] = singleAssets;
            //singleAssetEvents.onLoadOk.dispatch(singleAssets);
            //Debug.LogError(singleAssetEvents.onLoadOk);
            singleAssetEvents.onLoadOk.dispatch(null);
        }
    }

    /// <summary>
    /// 异步读取byte[]为压缩资源内存
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="singleAssets"></param>
    /// <param name="name"></param>
    /// <param name="mono"></param>
    /// <returns></returns>
    private static IEnumerator GetAssetBundle(byte[] asset, SALoadItem load, MonoBehaviour mono)
    {
        AssetBundleCreateRequest assetRequest = new AssetBundleCreateRequest();
        try
        {
            assetRequest = AssetBundle.CreateFromMemory(asset);
        }
        catch
        {
            Trace.Log("CreateFromMemoryErro:" + load.name + " " + load.type);
        }
        yield return assetRequest;

        if (assetRequest.assetBundle != null)
        {
            //加载完毕赋值,立刻解压缩内存
            SingleAssetData data = new SingleAssetData(assetRequest.assetBundle, assetRequest.assetBundle.Load(load.name), load.type);
            //卸载压缩资源
            data.UnLoad(false);
            assetDate[(int)load.type].Add(data);

            //Debug.LogError("LoadCount: " + assetDate[(int)load.type].Count);
            //singleAssets.Add(data);
        }
        NowLoadNames.Remove(load);
        //继续加载下一个
        LoadChainAsset(mono);
    }

    /// <summary>
    /// 获取加载资源的路径
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetLoadPath(SALoadItem load)
    {
        GameConfigData config = Global.GetInstance().gameConfigData;
        if (config == null)
        {
            //if (NowType == AssetType.Background)
            return DOC_NAME + GameConfigData.BGFile + "/" + load.name + Global.AssetPackageSuffix;
            //else if (NowType == AssetType.Card)
            //    return DOC_NAME + GameConfigData.CardFile + "/" + name + Global.AssetPackageSuffix;
        }
        else
        {
            switch (load.type)
            {
                case AssetType.Background:
                    return DOC_NAME + config.BackgroundPath + "/" + load.name + Global.AssetPackageSuffix;
                case AssetType.Card:
                    return DOC_NAME + config.CardPath + "/" + load.name + Global.AssetPackageSuffix;
                case AssetType.BatterCard:
                    return DOC_NAME + config.BatterCardPath + "/" + load.name + Global.AssetPackageSuffix;
                case AssetType.Sound:
                    return DOC_NAME + config.SoundPath + "/" + load.name + Global.AssetPackageSuffix;;
                case AssetType.Audio:
                    return DOC_NAME + config.AudioPath + "/" + load.name + Global.AssetPackageSuffix;;
            }
            
        }

        return "";
    }

    /// <summary>
    /// 新加WWW资源包
    /// </summary>
    public static List<AssetVersionItem> NewLoadAssetData;

    /// <summary>
    /// 是否是web资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static AssetVersionItem isWebAsset(string name)
    {
        if (NewLoadAssetData != null && NewLoadAssetData.Count > 0)
        {
            foreach (AssetVersionItem data in NewLoadAssetData)
            {
                //判断文件名是否是在新加包中存在
                if (data.PackageName.Contains(name))
                {
                    return data;
                }
            }
        }
        return null;
    }

    private static GTimer gTime;

    private static int UnLoadTime;
    private static int UnLoadCardTime;


    /// <summary>
    /// 立刻卸载资源
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    public static void UnLoadAssetNow(string name, AssetType type)
    {
        SingleAssetData data = FindAssetByType(name, type);
        if (data != null)
        {
            data.UnLoadMemory();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="type"></param>
    /// <param name="name"></param>
    public static void UnLoadAsset(string name, AssetType type)
    {
        StartGTime();
        //查找卸载的单资源包
        SingleAssetData data = FindAssetByType(name, type);
        if (data != null)
        {
            if (UnLoadAssets == null)
                UnLoadAssets = new List<SingleAssetData>();
            //如果存在则，判断是否在卸载列表中，如果存在则直接返回，不再添加
            else if (UnLoadAssets.Count > 0)
                foreach (SingleAssetData unAsset in UnLoadAssets)
                {
                    if (unAsset.Name == name && unAsset.type == type)
                        return;
                }

            Trace.Log("WillUnLoadSingle:" + data.Name);
            //确定卸载的时间
            data.UnLoadTime = System.DateTime.Now.AddSeconds(data.type == AssetType.Background ? UnLoadTime : UnLoadCardTime);
            UnLoadAssets.Add(data);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="type"></param>
    /// <param name="name"></param>
    public static void UnLoadAsset(List<string> names, AssetType type)
    {
        StartGTime();

        if (UnLoadAssets == null)
            UnLoadAssets = new List<SingleAssetData>();
        //查找卸载的单资源包
        List<SingleAssetData> singleAssets = assetDate[(int)type];
        SingleAssetData data;
        bool ishas = false;
        //判断资源包是否已经存在
        foreach (string name in names)
        {
            data = FindAssetByLiset(name, singleAssets);
            if (data != null)
            {
                ishas = false;
                foreach (SingleAssetData unAsset in UnLoadAssets)
                {
                    if (unAsset.Name == name && unAsset.type == type)
                    {
                        ishas = true;
                        break;
                    }
                }
                if (!ishas)
                {
                    //确定卸载的时间
                    data.UnLoadTime = System.DateTime.Now.AddSeconds(data.type == AssetType.Background ? UnLoadTime : UnLoadCardTime);
                    UnLoadAssets.Add(data);
                }
            }
        }
    }

    /// <summary>
    /// 启动定时器
    /// </summary>
    /// <param name="mono"></param>
    private static void StartGTime()
    {
        //回收时间定时器，每一秒判断一次，目前循环20分钟，后置空，重新开启。
        if (gTime == null || !gTime.running)
        {
            if (Global.GetInstance().gameConfigData != null)
            {
                //获取延迟回收时间
                UnLoadTime = Global.GetInstance().gameConfigData.UnLoadTime;
                UnLoadCardTime = Global.GetInstance().gameConfigData.UnLoadTimeCard;
            }
            else
            {
                UnLoadTime = 20;
                UnLoadCardTime = 50;
            }

           /** gTime = GTimer.AddTimer(MonoMainScene.mono, 3000, 600, delegate(object arg)
            {
                UnLoadAsset();
            },
            delegate(object arg)
            {
                gTime = null;
            }, true)**/;
        }
    }

    /// <summary>
    /// 卸载资源列表
    /// </summary>
    public static List<SingleAssetData> UnLoadAssets;


    /// <summary>
    /// 卸载资源
    /// </summary>
    private static void UnLoadAsset()
    {
        if (UnLoadAssets != null && UnLoadAssets.Count > 0)
        {
            Trace.Log("UnLoadAsset:" + UnLoadAssets.Count);
            UnLoadAssets.ForEach(delegate(SingleAssetData data)
            {
                if (data.UnLoadTime < System.DateTime.Now)
                {
                    Trace.Log("UnLoadSignle:" + data.Name);
                    data.UnLoadMemory();
                    UnLoadAssets.Remove(data);
                    Resources.UnloadUnusedAssets();
                }
            });
        }
    }

}