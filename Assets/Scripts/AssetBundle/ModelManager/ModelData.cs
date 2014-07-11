using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ModelData  {
	public int id;
	public string name;
	public string httpSourceUrl;
	public string serverBundlePath;
	public string localBundlePath;
	public string prefabPath;

	public List<ModelRenderData> modelRenderDataList = new List<ModelRenderData>();
	public List<ModelAnimationData> modelAnimationDataList = new List<ModelAnimationData> ();

	public List<string> fbxPathList = new List<string>();

	/// <summary>
	/// 模型预设
	/// </summary>
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