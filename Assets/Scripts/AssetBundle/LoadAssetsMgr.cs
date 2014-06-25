using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public enum AssetFromType
{
    Local = 1,
    Cache
}
/// <summary>
/// 资源加载属性类
/// </summary>
class LoadPathData : AssetVersionItem
{
    /// <summary>
    /// 加载来源类型
    /// </summary>
    public AssetFromType LoadFromType = AssetFromType.Local;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="path">文件相对路径 /包级别/文件名 </param>
    /// <param name="type">文件类型</param>
    /// <param name="directload">当压缩文件读取完毕后是否直接加载到内存</param>
    public LoadPathData(AssetVersionItem item, AssetFromType fromtype)
        : base(item.PackageName, item.Version)
    {
        LoadFromType = fromtype;
    }
}

/// <summary>
/// 资源包类
/// </summary>
class AssetBundleData
{
    /// <summary>
    /// 文件相对路径 /包级别/文件名 
    /// </summary>
    public string m_sPath;

    /// <summary>
    /// 加载来源类型
    /// </summary>
    public AssetFromType m_nType;

    /// <summary>
    /// AssetBundle资源
    /// </summary>
    public AssetBundle m_AssetBundle;

    /// <summary>
    /// 已加载进内存的资源列表(暂置)
    /// </summary>
    public List<Object> m_szObject = new List<Object>();
	/// <summary>
	///  目前没有使用压缩缓存，使用数组 高效批量加载
	/// </summary>
    public Object[] m_Object;
	
    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="path"></param>
    /// <param name="type"></param>
    /// <param name="assetBundle"></param>
    public AssetBundleData(string path, AssetFromType type, AssetBundle assetBundle)
    {
        m_sPath = path;
        m_nType = type;
        m_AssetBundle = assetBundle;
    }
    /// <summary>
    /// 销毁
    /// </summary>
    public void DelThis()
    {
        DelThis(false);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    /// <param name="unloadall"></param>
    public void DelThis(bool unloadall)
    {
        UnLoad(unloadall);
        m_AssetBundle = null;
        m_szObject.Clear();
        m_szObject = null; 
		
		m_Object = null; 
        //删除自己在全局资源管理中的引用
        LoadAssetsMgr.DelAssetBundle(this);
    }

    /// <summary>
    /// 加载压缩文件到内存
    /// </summary>
    /// <returns>加载列表</returns>
    public Object[] LoadAll()
    {
        //Object[] objects;
        if (m_AssetBundle != null)
        {
            Trace.Log("LoadAll:" + m_sPath);
            //objects = m_AssetBundle.LoadAll();
            //for (int i = 0; i < objects.Length; i++)
            //{
            //   AddData(objects[i]);
            //}
			m_Object = m_AssetBundle.LoadAll();
            return m_Object;
        }
        return new Object[] { };
    }
    /// <summary>
    /// 查询解压缩数据包，按文件名
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Name"></param>
    /// <returns></returns>
    public T GetDataByName<T>(string Name) where T : class
    {
        Object data;
//        if (m_szObject != null)
//            for (int i = 0; i < m_szObject.Count; i++)
//            {
//                data = m_szObject[i];
//                //查找文件名相同并是T类型的数据
//                if (data.name == Name && data is T)
//                {
//                    return data as T;
//                }
//            }
		if (m_Object != null)
            for (int i = 0; i < m_Object.Length; i++)
            {
                data = m_Object[i];
                //查找文件名相同并是T类型的数据
                if (data.name == Name && data is T)
                {
                    return data as T;
                }
            }
        return default(T);
    }
    /// <summary>
    /// 单个文件从资源列表中移除
    /// </summary>
    /// <param name="obj">需要删除的obj</param>
    /// <returns>是否删除成功</returns>
    public bool DelData(Object obj)
    {
        Trace.Log("DelData:" + obj.name);
        return m_szObject.Remove(obj);
    }
    /// <summary>
    /// 添加单个文件到资源列表
    /// </summary>
    /// <param name="obj"></param>
    private void AddData(Object obj)
    {
        if (m_szObject.IndexOf(obj) >= 0)
            return;
        //Debug.Log("AddData:" + obj.name);
        m_szObject.Add(obj);
    }

    /// <summary>
    /// 多个文件从资源列表中移除
    /// </summary>
    /// <param name="objs">需要删除的obj列表</param>
    /// <returns>删除个数</returns>
    public int DelDatas(Object[] objs)
    {
        int num = 0;
        for (int i = 0; i < objs.Length; i++)
        {
            if (DelData(objs[i]))
                ++num;
        }
        return num;
    }

    /// <summary>
    /// 移除所有资源
    /// </summary>
    /// <returns>删除个数</returns>
    public int DelAllDatas()
    {
        int num = m_szObject.Count;
        m_szObject.Clear();
        return num;
    }

    /// <summary>
    /// 加载单个文件到内存
    /// </summary>
    /// <param name="name">需要加载的文件名</param>
    /// <returns>当前加载的obj</returns>
    public Object LoadData(string name)
    {
        Object obj = m_AssetBundle.Load(name);
        if (obj == null)
        {
            Trace.LogWarning("AddData:" + name + " is null");
            return null;
        }
        AddData(obj);
        return obj;
    }
    /// <summary>
    /// 加载单个文件到内存
    /// </summary>
    /// <param name="name">需要加载的文件名</param>
    /// <returns>当前加载的obj</returns>
    public Object LoadData(string name, System.Type type)
    {
        Object obj = m_AssetBundle.Load(name, type);
        if (obj == null)
        {
            Trace.LogWarning("AddData:" + name + " is null   type:" + type);
            return null;
        }
        AddData(obj);
        return obj;
    }
    /// <summary>
    /// 加载多个文件到内存
    /// </summary>
    /// <param name="names">需要加载的文件列表</param>
    /// <returns>当前加载的objs</returns>
    public Object[] LoadDatas(string[] names)
    {
        Object[] datas = new Object[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            datas[i] = LoadData(names[i]);
        }
        return datas;
    }

    /// <summary>
    /// 删除压缩文件
    /// </summary>
    /// <param name="unloadall">是否删除所有</param>
    public void UnLoad(bool unloadall)
    {
        if (m_AssetBundle != null)
        {
            m_AssetBundle.Unload(unloadall);
            m_AssetBundle = null;
        }
    }

    /// <summary>
    /// 删除压缩文件
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="unloadall"></param>
    public void UnLoad(MonoBehaviour mono, bool unloadall)
    {
        if (m_AssetBundle != null)
        {
            /**GTimer.AddTimer(mono, Global.GetInstance().UnLoadAssetTime, 1, delegate(object data)
            {
                Trace.Log("unload:" + m_sPath);
                UnLoad(unloadall);
                (data as GTimer).Stop();
            }, true);**/


        }
    }

}

/// <summary>
/// 资源加载类，启用协程，需赋值mono，并使用后置空
/// </summary>
class LoadAssetsMgr : AssetsManage
{
    /// <summary>
    /// 等待加载队列
    /// </summary>
    private static List<LoadPathData> m_szLoadPaths = new List<LoadPathData>();

    /// <summary>
    /// 已加载列表
    /// </summary>
    public static List<AssetBundleData> m_szAssetBundle = new List<AssetBundleData>();

    /// <summary>
    /// 所有加载信息
    /// </summary>
    private static List<LoadPathData> m_szAllLoadPaths = new List<LoadPathData>();

    /// <summary>
    /// 是否在加载中
    /// </summary>
    private static bool m_bLoading = false;


    /// <summary>
    /// 获取设置所有加载信息
    /// </summary>
    public static List<LoadPathData> allpathdata
    {
        set
        {
            m_szAllLoadPaths = value;
            DelAllAssetBundlg();
        }
        get
        {
            return m_szAllLoadPaths;
        }
    }

    /// <summary>
    /// 查找加载信息
    /// </summary>
    /// <param name="path">相对路径 /包级别/文件名</param>
    /// <returns>返回LoadPathData,找不到时返回null</returns>
    public static LoadPathData FindLoadPathData(string path)
    {
        foreach (LoadPathData pathdata in m_szAllLoadPaths)
        {
            if (pathdata.PackageName == path)
                return pathdata;
        }
        return null;
    }

    /// <summary>
    /// 查找AssetBundleData
    /// </summary>
    /// <param name="path">相对路径 /包级别/文件名</param>
    /// <returns>返回AssetBundleData,找不到时返回null</returns>
    public static AssetBundleData FindAssetBundleData(string path)
    {
        foreach (AssetBundleData a in m_szAssetBundle)
        {
            if (a.m_sPath == path)
                return a;
        }
        return null;
    }
    /// <summary>
    /// 从AssetBundleData中查找第一个T类型对象
    /// </summary>
    /// <param name="path">相对路径 /包级别/文件名</param>
    /// <returns>返回AssetBundleData,找不到时返回null</returns>
    public static T GetDataByAsset<T>(string path) where T : class
    {
        AssetBundleData data = FindAssetBundleData(path);
        if (data != null)
        {
            return GetDataByAsset<T>(data);
        }
        return default(T);
    }

    /// <summary>
    /// 从AssetBundleData中查找第一个T类型对象
    /// </summary>
    /// <param name="path">相对路径 /包级别/文件名</param>
    /// <returns>返回AssetBundleData,找不到时返回null</returns>
    public static T GetDataByAsset<T>(AssetBundleData data) where T : class
    {
        Object obj;
//        for (int i = 0; i < data.m_szObject.Count; i++)
//        {
//            obj = data.m_szObject[i];
//            if (obj is T)
//            {
//                return obj as T;
//            }
//        }
		for (int i = 0; i < data.m_Object.Length; i++)
        {
            obj = data.m_Object[i];
            if (obj is T)
            {
                return obj as T;
            }
        }
        return default(T);
    }

    #region 加载压缩资源->内存

    /// <summary>
    /// 加载所有压缩资源进缓存
    /// </summary>
    public static void LoadAllInMemory()
    {
        foreach (AssetBundleData a in m_szAssetBundle)
        {
            a.LoadAll();
        }
    }

    /// <summary>
    /// 加载压缩资源进缓存,并立刻回收压缩内存
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="paths">需要加载的路径列表 /包级别/文件名</param>
    /// <returns>加载成功的数量</returns>
    public static int LoadResInMemory(MonoBehaviour mono, List<string> paths)
    {
        return LoadResInMemory(mono, paths, true);
    }
    /// <summary>
    /// 加载压缩资源进缓存
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="paths">需要加载的路径列表 /包级别/文件名</param>
    /// <param name="unLoad">是否回收压缩缓存</param>
    /// <returns>加载成功的数量</returns>
    public static int LoadResInMemory(MonoBehaviour mono, List<string> paths, bool unLoad)
    {
        int num = 0;
        AssetBundleData assetbundledata;
        foreach (string path in paths)
        {
            assetbundledata = LoadResInMemory(mono, path, unLoad);
            if (assetbundledata != null)
                num++;
        }
        return num;
    }
    /// <summary>
    /// 加载一个压缩资源进缓存
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="paths"></param>
    /// <param name="unLoad">是否回收压缩缓存</param>
    /// <returns></returns>
    public static AssetBundleData LoadResInMemory(MonoBehaviour mono, string path, bool unLoad)
    {
        AssetBundleData assetbundledata = FindAssetBundleData(path);
        if (assetbundledata != null)
        {
            //加载资源
            if (assetbundledata.LoadAll().Length > 0)
            {
                if (unLoad)
                    assetbundledata.UnLoad(mono, false);//卸载压缩资源
            }
        }
        return assetbundledata;
    }

    /// <summary>
    /// 加载压缩资源进缓存,并立即回收压缩内存
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="paths">需要加载的路径列表 /包级别/文件名</param>
    /// <param name="unLoad">是否回收压缩缓存</param>
    /// <returns></returns>
    public static int LoadResInMemory(List<string> paths, bool unLoad)
    {
        int num = 0;
        AssetBundleData assetbundledata;
        foreach (string path in paths)
        {
            assetbundledata = LoadResInMemory(path, unLoad);
            if (assetbundledata != null)
                num++;
        }
        return num;
    }
    /// <summary>
    /// 加载一个压缩资源进缓存
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="paths"></param>
    /// <param name="unLoad">是否回收压缩缓存</param>
    /// <returns></returns>
    public static AssetBundleData LoadResInMemory(string path, bool unLoad)
    {
        AssetBundleData assetbundledata = FindAssetBundleData(path);
        if (assetbundledata != null)
        {
            //加载资源
            if (assetbundledata.LoadAll().Length > 0)
            {
                if (unLoad)
                    assetbundledata.UnLoad(false);//卸载压缩资源
            }
        }
        return assetbundledata;
    }
    #endregion

    #region 添加加载资源

    /// <summary>
    /// 根据LoadPathData列表添加加载资源
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="loaddata">LoadPathData对象列表</param>
    /// <returns>返回不需要异步加载的对象数量</returns>
    public static int LoadResByLoadPathDataList(MonoBehaviour mono, List<LoadPathData> loaddatalist, signal.osnet.signals.Signal.listen callback)
    {
        events.onLoadOk.addOnce(callback);
        int num = 0;
        foreach (LoadPathData loaddata in loaddatalist)
        {
            if (!LoadResByLoadPathData(mono, loaddata, false))
                ++num;
        }
        BeginLoad(mono);
        return num;
    }

    /// <summary>
    /// 根据path列表添加加载资源
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="paths">path列表</param>
    /// <returns>返回不需要异步加载的对象数量</returns>
    public static int LoadResByPathList(MonoBehaviour mono, List<string> paths, signal.osnet.signals.Signal.listen callback)
    {
        events.onLoadOk.addOnce(callback);
        int num = 0;
        foreach (string path in paths)
        {
            if (!LoadResByPath(mono, path, false))
                ++num;
        }
        BeginLoad(mono);
        return num;
    }

    /// <summary>
    /// 加载单个资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    public static void LoadResByPath(MonoBehaviour mono, string path, signal.osnet.signals.Signal.listen callback)
    {
        events.onLoadOk.addOnce(callback);
        LoadResByPath(mono, path, false);
        BeginLoad(mono);
    }
    /// <summary>
    /// 根据path添加加载资源
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="path">相对路径 /包级别/文件名</param>
    /// <returns>如果已存在,则返回true</returns>
    private static bool LoadResByPath(MonoBehaviour mono, string path, bool beginload)
    {
        //判断是否有重复加载的资源
        if (FindAssetBundleData(path) != null)
            return true;
        LoadPathData pathdata = FindLoadPathData(path);
        if (pathdata == null)
        {
            //扩展为单资源包加载
            pathdata = new LoadPathData(new AssetVersionItem(path, 1), AssetFromType.Local);
        }
        m_szLoadPaths.Add(pathdata);
        if (beginload)
            BeginLoad(mono);
        return false;
    }

    /// <summary>
    /// 根据LoadPathData添加加载资源
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="loaddata">LoadPathData对象</param>
    /// <returns>如果已存在,则返回true</returns>
    public static bool LoadResByLoadPathData(MonoBehaviour mono, LoadPathData loaddata)
    {
        if (FindAssetBundleData(loaddata.PackageName) != null)
            return true;
        m_szLoadPaths.Add(loaddata);
        BeginLoad(mono);
        return false;
    }

    /// <summary>
    /// 根据LoadPathData添加加载资源
    /// </summary>
    /// <param name="mono">MonoBehaviour</param>
    /// <param name="loaddata">LoadPathData对象</param>
    /// <param name="loaddata">是否立即开始加载</param>
    /// <returns>如果已存在,则返回true</returns>
    public static bool LoadResByLoadPathData(MonoBehaviour mono, LoadPathData loaddata, bool beginload)
    {
        if (FindAssetBundleData(loaddata.PackageName) != null)
            return true;
        m_szLoadPaths.Add(loaddata);
        if (beginload)
            BeginLoad(mono);
        return false;
    }

    #endregion

    #region 删除缓存对象
    /// <summary>
    /// 卸载压缩资源
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    public static void UnLoadAsset(List<string> paths)
    {
        AssetBundleData assetbundledata;
        foreach (string path in paths)
        {
            assetbundledata = FindAssetBundleData(path);
            assetbundledata.UnLoad(false);
        }
    }
    /// <summary>
    /// 删除所有已加载的缓存对象
    /// </summary>
    public static void DelAllAssetBundlg()
    {
        for (int i = m_szAssetBundle.Count - 1; i >= 0; i--)
        {
            AssetBundleData assetdata = m_szAssetBundle[i];
            assetdata.DelThis();
            assetdata = null;
        }
    }

    /// <summary>
    /// 根据相对路径 /包级别/文件名 删除已加载压缩缓存对象
    /// </summary>
    /// <param name="path"></param>
    public static void DelAssetBundlgByPath(string path)
    {
        AssetBundleData loaddata = FindAssetBundleData(path);
        if (loaddata != null)
            loaddata.DelThis();
    }
    /// <summary>
    /// 根据相对路径 /包级别/文件名 删除已加载压缩缓存对象
    /// </summary>
    /// <param name="path"></param>
    public static void DelAssetBundlgByPath(List<string> paths)
    {
        paths.ForEach(delegate(string path)
        {
            DelAssetBundlgByPath(path);
        });
    }


    /// <summary>
    /// 删除已加载压缩缓存对象,注意:由AssetBundleData内部DelThis调用
    /// 外部调用请调用DelAssetBundlgByPath
    /// </summary>
    /// <param name="loaddata">AssetBundleData对象</param>
    public static void DelAssetBundle(AssetBundleData assetdata)
    {
        m_szAssetBundle.Remove(assetdata);
    }

    #endregion

    #region 加载本地/网络资源->内存

    /// <summary>
    /// 开始加载
    /// </summary>
    private static void BeginLoad(MonoBehaviour mono)
    {
        if (m_bLoading)
            return;
        m_bLoading = true;
        LoadRes(mono);
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    private static void LoadRes(MonoBehaviour mono)
    {
        //如果等待加载资源列表为空,则结束加载
        if (m_szLoadPaths.Count == 0)
        {
            m_bLoading = false;
            m_Events.onLoadOk.dispatch(m_szAssetBundle);
            return;
        }

        if (mono == null)
        {
            Trace.LogWarning("LoadRes mono is null");
            m_szLoadPaths.Clear();
            return;
        }

        LoadPathData loaddata = m_szLoadPaths[0];
        m_szLoadPaths.Remove(loaddata);
        if (loaddata != null)
        {
            Trace.Log("loaddata" + loaddata.PackageName);
            switch (loaddata.LoadFromType)
            {
                //加载本地资源
                case AssetFromType.Local:
                    {
                        LoadStreamingAssets.LoadAssetData(DOC_NAME + loaddata.PackageName, mono, delegate(byte[] asset)
                        {
                            mono.StartCoroutine(GetAssetBundle(asset, loaddata, mono));
                        });
                    }
                    break;
                //加载网络/缓存资源
                case AssetFromType.Cache:
                    {
                        mono.StartCoroutine(GetWWWAssetBundle(WWW_NAME + loaddata.PackageName, loaddata, mono));
                    }
                    break;
                default:
                    {
                        LoadRes(mono);
                    }
                    break;
            }
        }
        else
            LoadRes(mono);
    }

    static IEnumerator GetWWWAssetBundle(string path, LoadPathData loaddata, MonoBehaviour mono)
    {
        WWW www = new WWW(path);
        yield return www;
        AssetBundleCreateRequest assetRequest = AssetBundle.CreateFromMemory(www.bytes);
        yield return assetRequest;
        AssetBundle assetBundle = assetRequest.assetBundle;
        if (assetBundle == null)
            m_Events.onLoadErr.dispatch(loaddata);
        else
        {
            AssetBundleData assetbundledata = new AssetBundleData(loaddata.PackageName, loaddata.LoadFromType, assetBundle);
            m_szAssetBundle.Add(assetbundledata);
        }
        LoadRes(mono);
    }

    static IEnumerator GetAssetBundle(byte[] asset, LoadPathData loaddata, MonoBehaviour mono)
    {
        AssetBundleCreateRequest assetRequest = AssetBundle.CreateFromMemory(asset);
        yield return assetRequest;
        AssetBundle assetBundle = assetRequest.assetBundle;
        if (assetBundle == null)
            m_Events.onLoadErr.dispatch(loaddata);
        else
        {
            AssetBundleData assetbundledata = new AssetBundleData(loaddata.PackageName, loaddata.LoadFromType, assetBundle);
            m_szAssetBundle.Add(assetbundledata);
        }
        LoadRes(mono);
    }

    //待废弃
    //     static IEnumerator GetSceneBundle(byte[] asset, LoadPathData loaddata)
    //     {
    //         AssetBundleCreateRequest assetRequest = AssetBundle.CreateFromMemory(asset);
    //         yield return assetRequest;
    //         AssetBundle assetBundle = assetRequest.assetBundle;
    //         if (assetBundle == null)
    //             m_Events.onLoadErr.dispatch(loaddata);
    //         else
    //         {
    //             AssetBundleData assetbundledata = new AssetBundleData(loaddata.m_sPath, loaddata.m_nType, assetBundle);
    //             m_szAssetBundle.Add(assetbundledata);
    //             if (loaddata.m_bDirectLoad)
    //                 assetbundledata.LoadAll();
    //         }
    //         LoadRes();
    //     }

    #endregion
}
