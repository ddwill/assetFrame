using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ModelData  {
	public int id;
	public string name;
	public string url;

	public string prefabPath;

	public List<ModelRenderData> modelRenderDataList = new List<ModelRenderData>();
}

public class ModelRenderData
{
	public string meshPath;
	public string materialPath;
	public string shaderPath;
}
