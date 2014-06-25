using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// WWW辅助类，链式加载网络资源
/// </summary>
public class WWWHelp
{
    /// <summary>
    /// 下载列表资源主Url
    /// </summary>
    public string UrlAsset { get; set; }
    /// <summary>
    /// 链式加载协程父类引用
    /// </summary>
    public MonoBehaviour mono { get; set; }
    /// <summary>
    /// 获取当前使用的www信息,多用于进度获取
    /// </summary>
    public WWW www;
    /// <summary>
    /// 加载缓存内容
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadCacheAssetBundle(string File, int Version, FinishLoad OnFinishLoad)
    {
        www = WWW.LoadFromCacheOrDownload(File, Version);
        yield return www;
        Trace.Log(www.error + File);
        OnFinishLoad(www);
    }

    /// <summary>
    /// 加载新内容
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadWWW(string File, FinishLoad OnFinishLoad)
    {
        www = new WWW(File);
        yield return www;
        OnFinishLoad(www);
    }

    public WWWHelp()
    { }

    /// <summary>
    /// 链式加载构造函数
    /// </summary>
    /// <param name="UrlAsset"></param>
    /// <param name="mono"></param>
    public WWWHelp(string UrlAsset, MonoBehaviour mono, List<AssetVersionItem> DownLoadAssets)
    {
        this.UrlAsset = UrlAsset;
        this.mono = mono;
        this.downLoadAssets = DownLoadAssets;
        this.Count = DownLoadAssets.Count;
        this.NowCount = 0;
    }
    /// <summary>
    /// 当前下载的index
    /// </summary>
    public int NowCount { get; set; }
    /// <summary>
    /// 加载的资源总数
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// 当前下载项加载的进度(0,1)
    /// </summary>
    public float NowProgress { get { return www != null ? www.progress : 0f; } }

    /// <summary>
    /// 下载链的总进度(0,1)
    /// </summary>
    public float Progress
    {
        get
        {
            return (float)NowCount / (float)Count + NowProgress / (float)Count;
        }
    }
    /// <summary>
    /// 要加载的资源链
    /// </summary>
    public List<AssetVersionItem> downLoadAssets;

    /// <summary>
    /// 链式加载网络资源
    /// </summary>
    /// <param name="OnFinishLoad"></param>
    public void WWWLoadAsset(WWWHelp.FinishLoad OnFinishLoad)
    {
        if (!string.IsNullOrEmpty(UrlAsset) && mono != null)
        {
            AssetVersionItem assetVersion = this.downLoadAssets[NowCount];
            //判断缓存中是否存在
            if (!Caching.IsVersionCached(UrlAsset + assetVersion.PackageName, assetVersion.Version))
            {
                //未缓存则从网络端下载资源
                mono.StartCoroutine(LoadCacheAssetBundle(UrlAsset + assetVersion.PackageName, assetVersion.Version, delegate(WWW www)
                {
                    if (www.assetBundle != null)
                    {
                        www.assetBundle.Unload(false);
                        //StrMessage += (count + 1).ToString() + "," + assetVersion.PackageName + "更新完毕;\n\r";
                        NowCount++;
                        if (this.downLoadAssets.Count > NowCount)
                            WWWLoadAsset(OnFinishLoad);
                        else
                            OnFinishLoad(www);
                    }
                }));
            }
            else
            {
                //StrMessage += (count + 1).ToString() + "," + assetVersion.PackageName + "已缓存无需更新;\n\r";
                NowCount++;
                if (this.downLoadAssets.Count > NowCount)
                    WWWLoadAsset(OnFinishLoad);
                else
                    OnFinishLoad(null);
            }
        }
        else
            OnFinishLoad(null);
    }


    /// <summary>
    /// 加载内容完成
    /// </summary>
    public delegate void FinishLoad(WWW www);
}
