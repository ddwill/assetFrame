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


}

public class ModelRenderData
{
	public string meshPath;
	public string materialPath;
	public string shaderPath;
}

/// <summary>
/// 模型动作数据
/// </summary>
public class ModelAnimationData
{

}