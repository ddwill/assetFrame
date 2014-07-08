using UnityEngine;
using System.Collections;
using System.IO;

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

	/// <summary>
	/// 全局路径到Assets的相对路径
	/// </summary>
	/// <returns>The path_ assets.</returns>
	/// <param name="path">Path.</param>
	public static string TrimPath_Assets(string path)
	{
		int index = path.IndexOf ("Assets");
		if (index < 0) return null;
		return path.Substring (index);

	}

	/// <summary>
	/// 检测路径指定的文件是否存在，若存在则返回 标示后加数字的路径。
	/// 如：
	/// globalPath= c://aaa/qq.txt c='#' 如果文件存在则返回 c://aaa/qq#1.txt
	/// </summary>
	/// <returns>The un exist path by global path.</returns>
	/// <param name="globalPath">Global path.</param>
	/// <param name="c">C.</param>
	public static string GetUnExistPathByAssetPath(string assetPath,char c)
	{
		string path = Application.dataPath +"/"+ assetPath;
		
		return ConfigPath.TrimPath_Assets( GetUnExistPathByGlobalPath (path));
	}
	
	/// <summary>
	/// 检测路径指定的文件是否存在，若存在则返回 标示后加数字的路径。
	/// 如：
	/// globalPath= c://aaa/qq.txt c='#' 如果文件存在则返回 c://aaa/qq#1.txt
	/// </summary>
	/// <returns>The un exist path by global path.</returns>
	/// <param name="globalPath">Global path.</param>
	/// <param name="c">C.</param>
	public static string GetUnExistPathByGlobalPath(string globalPath, char c)
	{
		if (!File.Exists (globalPath))
		{
			return globalPath;
		}
		
		string fileName =  Path.GetFileNameWithoutExtension (globalPath);
		
		
		string returnName;
		string[] paths = fileName.Split[c];
		
		if (paths.Length>1) 
		{	
			returnName = paths[0]+c+(int.Parse(paths[1])+1);
		}else
		{
			returnName = paths[0]+c+1;
		}
		
		return   GetUnExistPathByGlobalPath( Path.GetDirectoryName (globalPath)
		                                    + returnName  + Path.GetExtension (globalPath));
	}
}
