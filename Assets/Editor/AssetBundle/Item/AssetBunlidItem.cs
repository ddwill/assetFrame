using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 打包资源级别配置实体
/// </summary>
public class AssetLevelItem
{
    /// <summary>
    /// 级别
    /// </summary>
    public int Level;
    /// <summary>
    /// 级别名称
    /// </summary>
    public string LevelName;

    /// <summary>
    /// 项目版本
    /// </summary>
    public string Version;
    /// <summary>
    /// 当前级别资源包列表
    /// </summary>
    public List<AssetPackageItem> AssetPackageList { get; set; }

    /// <summary>
    /// 全参构造
    /// </summary>
    /// <param name="Level">级别</param>
    /// <param name="LevelName">级别名称</param>
    /// <param name="AssetPackageList">资源包列表</param>
    public AssetLevelItem(int Level, string LevelName, List<AssetPackageItem> AssetPackageList)
    {
        this.Level = Level;
        this.LevelName = LevelName;
        this.AssetPackageList = AssetPackageList;
    }
    /// <summary>
    /// 创建默认打包级别，三级
    /// </summary>
    /// <returns></returns>
    public static List<AssetLevelItem> CreateDefaultLevel()
    {
        List<AssetLevelItem> assetLevels = new List<AssetLevelItem>();
        //assetLevels.Add(new AssetLevelItem(1, "图片,音乐,Shader", null));
        //assetLevels.Add(new AssetLevelItem(2, "Mesh,Materials,Prefabs", null));
       // assetLevels.Add(new AssetLevelItem(3, "场景", null));
		assetLevels.Add(new AssetLevelItem(1, "资源", null));
        return assetLevels;
    }
}
/// <summary>
/// 资源包
/// </summary>
public class AssetPackageItem
{
    /// <summary>
    /// 打包文件名称
    /// </summary>
    public string PackageName { get; set; }
    /// <summary>
    /// 资源包版本号
    /// </summary>
    public string PackageVersion { get; set; }

    /// <summary>
    /// 是否单个打包
    /// </summary>
    public bool IsSingle { get; set; }

    /// <summary>
    /// 打包文件夹路径列表
    /// </summary>
    public List<AssetFileItem> FilesList { get; set; }
    /// <summary>
    /// 全参构造
    /// </summary>
    /// <param name="PackageName">打包文件名称</param>
    /// <param name="FilesList">打包文件夹路径列表</param>
    public AssetPackageItem(string PackageName, List<AssetFileItem> FilesList, string PackageVersion, bool IsSingle)
    {
        this.PackageName = PackageName;
        this.FilesList = FilesList;
        this.PackageVersion = PackageVersion;
        this.IsSingle = IsSingle;
    }

    /// <summary>
    /// 打包文件名称
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public string GetPackageFile(int level)
    {
        return "/" + level + "/" + PackageName + Global.AssetPackageSuffix;
    }

}

/// <summary>
/// 资源文件夹
/// </summary>
public class AssetFileItem
{
    /// <summary>
    /// 文件夹
    /// </summary>
    public const int FILETYPE_DOC = 1;
    /// <summary>
    /// 普通文件
    /// </summary>
    public const int FILETYPE_NORMAL = 2;
    /// <summary>
    /// sence文件
    /// </summary>
    public const int FILETYPE_SENCE = 3;

    public string FilePath;
    public AssetFileItem(string FilePath)
    {
        this.FilePath = FilePath;
    }

    public int GetFileType()
    {
        if (FilePath.IndexOf(".") == -1)
            return FILETYPE_DOC;
        return FilePath.EndsWith(".unity") ? FILETYPE_SENCE : FILETYPE_NORMAL;
    }
}