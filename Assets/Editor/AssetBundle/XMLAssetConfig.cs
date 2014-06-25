using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System;
using UnityEngine;
/// <summary>
/// XML：Asset打包配置保存
/// </summary>
public class XMLAssetConfig
{

    /// <summary>
    /// 保存Asset打包配置
    /// </summary>
    /// <param name="assetLevelItems"></param>
    /// <param name="File"></param>
    /// <returns></returns>
    public static bool SaveAssetConfig(List<AssetLevelItem> assetLevelItems, string ProjectVersion, string File)
    {
        try
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement xmlelem;

            //加入XML的声明段落,<?xml version="1.0" encoding="utf-8" ?>
            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmldoc.AppendChild(xmldecl);

            //加入根元素
            xmlelem = xmldoc.CreateElement("", "AssetLevels", "");
            xmlelem.SetAttribute("Version", ProjectVersion);//创建属性：Version
            xmldoc.AppendChild(xmlelem);

            XmlNode root = xmldoc.SelectSingleNode("AssetLevels");//查找<AssetLevels> 
            XmlElement xe1, xe2, xe3;
            //遍历级别
            assetLevelItems.ForEach(delegate(AssetLevelItem assetLevelItem)
            {
                xe1 = xmldoc.CreateElement("AssetLevel");//创建一个<Node>节点
                xe1.SetAttribute("Level", assetLevelItem.Level.ToString());//创建属性：Level
                xe1.SetAttribute("LevelName", assetLevelItem.LevelName);//LevelName

                //遍历包名
                if (assetLevelItem.AssetPackageList != null)
                {
                    assetLevelItem.AssetPackageList.ForEach(delegate(AssetPackageItem assetPackageItem)
                    {
                        xe2 = xmldoc.CreateElement("AssetPackage");//创建一个<AssetPackage>节点
                        xe2.SetAttribute("PackageName", assetPackageItem.PackageName);//创建属性：PackageName
                        xe2.SetAttribute("Version", assetPackageItem.PackageVersion);//Version
                        xe2.SetAttribute("IsSingle", assetPackageItem.IsSingle ? "1" : "0");//IsSingle

                        //遍历文件名
                        if (assetPackageItem.FilesList != null)
                        {
                            assetPackageItem.FilesList.ForEach(delegate(AssetFileItem assetFileItem)
                            {
                                xe3 = xmldoc.CreateElement("AssetFile");//创建一个<AssetFile>节点
                                xe3.InnerXml = "<![CDATA[" + assetFileItem.FilePath + "]]>";
                                xe2.AppendChild(xe3);
                            });
                        }
                        xe1.AppendChild(xe2);
                    });
                }
                root.AppendChild(xe1);
            });
            xmldoc.Save(File);
            return true;
        }
        catch (Exception ex) { Trace.Log(ex.Message); }
        return false;
    }

    /// <summary>
    /// 加载Asset打包配置
    /// </summary>
    /// <param name="File"></param>
    /// <returns></returns>
    public static List<AssetLevelItem> LoadAssetConfig(string File)
    {
        try
        {
            XmlDocument xmldoc = new XmlDocument();
            //读取Asset打包配置
            xmldoc.Load(File);
            List<AssetLevelItem> assetLevels = null;
            XmlNode xmlRoot = xmldoc.SelectSingleNode("AssetLevels");
            //获取级别
            XmlNodeList xmlAssets = xmldoc.SelectNodes("AssetLevels/AssetLevel");
            //版本
            string Version = xmlRoot.Attributes["Version"].Value;
            if (xmlAssets != null && xmlAssets.Count > 0)
            {
                assetLevels = new List<AssetLevelItem>();
                List<AssetPackageItem> assetPackageItems = null;
                List<AssetFileItem> assetFileItems = null;
                XmlNodeList xmlAssetPackage, xmlAssetFile;
                AssetLevelItem assetLevelItem;
                //遍历级别
                for (int i = 0; i < xmlAssets.Count; i++)
                {
                    assetPackageItems = new List<AssetPackageItem>();//置空上一个列表
                    xmlAssetPackage = xmlAssets[i].SelectNodes("AssetPackage");
                    Trace.Log(xmlAssets[i].Attributes["LevelName"].Value);
                    //遍历包
                    for (int j = 0; j < xmlAssetPackage.Count; j++)
                    {
                        assetFileItems = new List<AssetFileItem>();//置空上一个列表
                        xmlAssetFile = xmlAssetPackage[j].SelectNodes("AssetFile");

                        //Debug.Log(xmlAssetPackage[j].Attributes["PackageName"].Value);
                        //遍历文件夹
                        for (int k = 0; k < xmlAssetFile.Count; k++)
                        {
                            assetFileItems.Add(new AssetFileItem(xmlAssetFile[k].InnerText));//文件夹名
                        }

                        assetPackageItems.Add(new AssetPackageItem(xmlAssetPackage[j].Attributes["PackageName"].Value, assetFileItems,
                            xmlAssetPackage[j].Attributes["Version"].Value, xmlAssetPackage[j].Attributes["IsSingle"].Value == "1" ? true : false));//包，文件夹列表
                    }
                    assetLevelItem = new AssetLevelItem(int.Parse(xmlAssets[i].Attributes["Level"].Value),
                        xmlAssets[i].Attributes["LevelName"].Value, assetPackageItems);
                    assetLevelItem.Version = Version;
                    assetLevels.Add(assetLevelItem);//级别，包，文件夹列表
                }
            }

            return assetLevels;
        }
        catch (Exception ex) { Trace.Log(ex.Message); }
        return null;
    }
}
