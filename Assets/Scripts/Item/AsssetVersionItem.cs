using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 资源项目版本
/// </summary>
public class ProjectVersionItem
{
    private string version;
    /// <summary>
    /// 版本号
    /// </summary>
    public string Version
    {
        get { return this.version; }
        set
        {
            this.version = value;
            SetVersion();
        }
    }
    /// <summary>
    /// 当前级别资源包列表
    /// </summary>
    public List<AssetVersionItem> packageVersionItem { get; set; }

    /// <summary>
    /// 第一版本号，强制安装版本
    /// </summary>
    public int FirstVersion;
    /// <summary>
    /// 第二版本号，资源包安装版本
    /// </summary>
    public int SecondVersion;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="Version"></param>
    /// <param name="packageVersionItem"></param>
    public ProjectVersionItem(string Version, List<AssetVersionItem> packageVersionItem)
    {
        this.Version = Version;
        this.packageVersionItem = packageVersionItem;
    }
    public ProjectVersionItem() { }
    /// <summary>
    /// 拆分版本号
    /// </summary>
    /// <param name="Version">版本号</param>
    public void SetVersion()
    {
        string[] Versions = this.Version.Split('.');
        FirstVersion = int.Parse(Versions[0]);
        SecondVersion = int.Parse(Versions[1]);
    }

    /// <summary>
    /// 获得需要更新的包名,返回Null,需要大版本更新
    /// </summary>
    /// <param name="item">新项目版本文件</param>
    /// <returns>包名list</returns>
    public List<AssetVersionItem>[] CheckPackage(ProjectVersionItem item)
    {
        List<AssetVersionItem> packagelist = new List<AssetVersionItem>();
        //版本相同
        if (this.Version == item.Version)
            return new List<AssetVersionItem>[] { packagelist, null };
        //大版本更新
        else if (this.FirstVersion < item.FirstVersion)
            return null;

        bool hasfind = false;
        //新加版本列表
        List<AssetVersionItem> newAssetVersions = new List<AssetVersionItem>();
        for (int i = 0; i < item.packageVersionItem.Count; i++)
        {
            AssetVersionItem newpackage = item.packageVersionItem[i];
            hasfind = false;
            //旧版本是否过期
            foreach (AssetVersionItem oldpackage in packageVersionItem)
            {
                if (newpackage.PackageName == oldpackage.PackageName)
                {
                    hasfind = true;
                    if (oldpackage.CheckVersion(newpackage.Version))
                    {
                        packagelist.Add(newpackage);
                        break;
                    }
                }
            }
            //如果在旧版本中未找到，则为新加版本，直接更新
            if (!hasfind)
            {
                packagelist.Add(newpackage);
                //新加版本
                newAssetVersions.Add(newpackage);
            }
        }
        return new List<AssetVersionItem>[] { packagelist, newAssetVersions };
    }

}


/// <summary>
/// 资源包版本实体
/// </summary>
public class AssetVersionItem
{
    /// <summary>
    /// 打包文件名称
    /// </summary>
    public string PackageName { get; set; }
    /// <summary>
    /// 打包文件版本号
    /// </summary>
    public int Version { get; set; }


    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="PackageName"></param>
    /// <param name="Version"></param>
    public AssetVersionItem(string PackageName, int Version)
    {
        this.PackageName = PackageName;
        this.Version = Version;
    }

    /// <summary>
    /// 检测版本
    /// </summary>
    /// <param name="newVersion">新版本号</param>
    /// <returns>如果版本号较新，返回true</returns>
    public bool CheckVersion(int newVersion)
    {
        return Version < newVersion;
    }
}
