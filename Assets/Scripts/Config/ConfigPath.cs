using UnityEngine;
using System.Collections;

public class ConfigPath {

	public const string RESOURCES_PATH = "Assets/Resources/";
	public const string RESOURCES_CONFIG_PATH = "Assets/Resources/Config/";
	public const string RESOURCES_CONFIG_SCENCEDATA = RESOURCES_CONFIG_PATH+"ScenceData/";
	
	public const string SCENCE_SAVE_PATH = "Resources/ScenceSave/";


	public const string MODEL_STORE_PATH = RESOURCES_PATH+"Model/";
	public const string MODEL_STORE_GLOBAL_PATH = Application.dataPath + "/Resources/Model/";




	/// <summary>
	/// 得到Resources文件夹下的路径，用于Resources.Load()
	/// </summary>
	/// <returns>The path_ resource.</returns>
	/// <param name="path">Path.</param>
	public static string TrimPath_Resource(string path)
	{
		string[] strings = {"Resources"};
		string[] splitResult =  path.Split (strings, System.StringSplitOptions.RemoveEmptyEntries);
		return splitResult [splitResult.Length - 1];
	}


	/// <summary>
	/// 去掉后缀
	/// </summary>
	/// <returns>The suffix.</returns>
	/// <param name="path">Path.</param>
	public static string TrimSuffix(string path)
	{
		return path.Split('.')[0];
	}
}
