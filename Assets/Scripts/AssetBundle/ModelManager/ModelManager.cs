using UnityEngine;
using System.Collections;
using UnityEditor;

public class ModelManager {
	private static ModelManager _instance;
	public static ModelManager Instance
	{
		get {
			if(_instance==null) _instance = new ModelManager();
			return _instance; }
	}

	/// <summary>
	/// Editor 中获得模型路径或是从文件中读取
	/// </summary>
	public void ImportModel(ModelData data)
	{
		/**SeperateModeleSources(data);
		BuidAssetBundle (data);**/

		CreatePrefab (data);
		addAnimation (data);
		SeperateModeleSources (data);
		BuidlAssetBundle (data);
		SendToServerWebFloder (data);
	}


	/// <summary>
	/// 模型信息设置
	/// 增加碰撞体,
	/// 增加特效点
	/// 
	/// </summary>
	public void EditorModel(ModelData data)
	{

	}

	/// <summary>
	/// 分离模型和贴图
	/// 
	/// 
	/// </summary>
	public void SeperateModeleSources(ModelData data)
	{

	}

	/// <summary>
	/// 生成预设
	/// </summary>
	public void CreatePrefab(ModelData data)
	{

	}

	/// <summary>
	/// 打assetBundle包
	/// </summary>
	/// <param name="data">Data.</param>
	public void BuidlAssetBundle(ModelData data)
	{

	}

	/// <summary>
	/// 发送assetbundle 到服务器,通知服务器更新，memcach.
	/// </summary>
	/// <param name="data">Data.</param>
	public void SendToServerWebFloder(ModelData data)
	{

	}

	/// <summary>
	/// 增加动画
	/// </summary>
	public void addAnimation(ModelData data)
	{

	}
}
