using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;

[System.Serializable]
public class ModelData  {
	[XmlAttribute]
	public int id;
	[XmlAttribute]
	public int userId;
	[XmlAttribute]
	public string name;
	
	public string httpSourceUrl;
	public string serverBundlePath;

	/// <summary>
	/// bundle文件夹
	/// </summary>
	public string localBundlePath;
	public string prefabPath;
	public string outFbxPath;
	public string inFbxPath;

	[XmlAttribute]
	public bool HasPackaged = false;

	[XmlArray]
	public List<ModelRenderData> modelRenderDataList = new List<ModelRenderData>();
	[XmlArray]
	public List<ModelAnimationData> modelAnimationDataList = new List<ModelAnimationData> ();

	/// <summary>
	/// 模型预设
	/// </summary>
	[XmlIgnore]
	public GameObject prefab;

	[XmlIgnore]
	public ModelManager.ModelImportState improtState = ModelManager.ModelImportState.none;

	public List<string> bundlePaths = new List<string>();
	public List<string> GetBundlePath()
	{
		return bundlePaths;
	}

    public ModelData()
    {
    }

    public ModelData(ServerModelMessage serverMessage)
    {
        this.id = serverMessage.id;
        this.userId = serverMessage.userId;
        this.name = serverMessage.name;
        this.outFbxPath = serverMessage.outFbxPath;
    }

    public string getBundlePath(BuildTarget buildTarget)
    {

        switch (buildTarget)
        {
            case BuildTarget.WebPlayer: return this.serverBundlePath + ConfigPath.BUNDLE_WEB_EXTENTION;
            case BuildTarget.Android: return this.serverBundlePath + ConfigPath.BUNDLE_ANDROID_EXTENTION;
            case BuildTarget.iPhone: return this.serverBundlePath + ConfigPath.BUNDLE_IPHONE_EXTENTION;
        }

        return null;
    }
}

public class ModelRenderData
{
	public string meshPath;
	public string materialPath;
	public string shaderPath;
	public string name;
}

/// <summary>
/// 模型动作数据
/// </summary>
public class ModelAnimationData
{

}

[System.Serializable]
public class ServerModelMessage
{
    [XmlAttribute]
    public int id;
    [XmlAttribute]
    public int userId;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public bool hasRead = false;

    public string outFbxPath;


}