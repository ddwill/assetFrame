using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System;
using System.Text;

public class XMLAssetVersionConfig
{
    /// <summary>
    /// 保存Asset打包配置
    /// </summary>
    /// <param name="assetLevelItems"></param>
    /// <param name="File"></param>
    /// <returns></returns>
    public static bool SaveAssetVersionConfig(ProjectVersionItem projectVersionItem, string File)
    {
        try
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement xmlelem;
            Trace.Log("File");
            //加入XML的声明段落,<?xml version="1.0" encoding="utf-8" ?>
            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmldoc.AppendChild(xmldecl);

            //加入根元素
            xmlelem = xmldoc.CreateElement("", "ProjectVersion", "");
            xmlelem.SetAttribute("Version", projectVersionItem.Version);//创建属性：Version

            XmlElement xe1;
            Trace.Log(projectVersionItem.packageVersionItem.Count);
            //遍历级别
            projectVersionItem.packageVersionItem.ForEach(delegate(AssetVersionItem assetVersionItem)
            {
				Debug.Log("!!!!!!!!!!!!!!!!!!!!!!");
                xe1 = xmldoc.CreateElement("AssetPackage");//创建一个<Node>节点
                xe1.SetAttribute("PackageName", assetVersionItem.PackageName);//创建属性：PackageName
                xe1.SetAttribute("Version", assetVersionItem.Version.ToString());//Version
                Trace.Log(assetVersionItem.PackageName);
                xmlelem.AppendChild(xe1);
            });

            xmldoc.AppendChild(xmlelem);
            xmldoc.Save(File);
            return true;
        }
        catch (Exception ex) { Trace.Log(ex.Message + "  " + ex.Source); }
        return false;
    }
    /// <summary>
    /// 根据字节数组加载Asset版本配置
    /// </summary>
    /// <param name="File"></param>
    /// <returns></returns>
    public static ProjectVersionItem LoadAssetVersionConfig(Byte[] bytes)
    {
        try
        {
            if (bytes != null && bytes.Length != 0)
            {
                //Encoding uft8 = new UTF8Encoding(false);
                XmlDocument xmldoc = new XmlDocument();

                System.IO.StringReader stringReader = new System.IO.StringReader(Encoding.UTF8.GetString(bytes));
                stringReader.Read(); // 跳过 BOM 
                /*System.Xml.XmlReader reader =*/
                System.Xml.XmlReader.Create(stringReader);
                //读取Asset版本配置
                xmldoc.LoadXml(stringReader.ReadToEnd());

                return LoadAssetVersoin(xmldoc);
            }
            else
                return null;
        }
        catch (Exception ex) { Trace.Log(ex.Message + "   " + ex.StackTrace); }
        return null;
    }

    /// <summary>
    /// 根据文本路径加载Asset版本配置
    /// </summary>
    /// <param name="File"></param>
    /// <returns></returns>
    public static ProjectVersionItem LoadAssetVersionConfig(string File)
    {
        try
        {
            XmlDocument xmldoc = new XmlDocument();
            //读取Asset版本配置
            xmldoc.Load(File);
            return LoadAssetVersoin(xmldoc);
        }
        catch (Exception ex) { Trace.Log(ex.Message); }
        return null;
    }

    /// <summary>
    /// 根据XML加载项目版本信息
    /// </summary>
    /// <param name="xmldoc"></param>
    /// <returns></returns>
    private static ProjectVersionItem LoadAssetVersoin(XmlDocument xmldoc)
    {
        ProjectVersionItem projectVersion = new ProjectVersionItem();
        //获取项目版本号
        XmlNode xmlProject = xmldoc.SelectSingleNode("ProjectVersion");
        projectVersion.Version = xmlProject.Attributes["Version"].Value;

        XmlNodeList xmlAssets = xmlProject.SelectNodes("AssetPackage");
        if (xmlAssets != null && xmlAssets.Count > 0)
        {
            List<AssetVersionItem> assetVersionItems = new List<AssetVersionItem>();
            //遍历级别
            for (int i = 0; i < xmlAssets.Count; i++)
            {
                assetVersionItems.Add(new AssetVersionItem(xmlAssets[i].Attributes["PackageName"].Value,
                    int.Parse(xmlAssets[i].Attributes["Version"].Value)));//级别，包，文件夹列表
            }
            projectVersion.packageVersionItem = assetVersionItems;
        }

        return projectVersion;
    }
}
