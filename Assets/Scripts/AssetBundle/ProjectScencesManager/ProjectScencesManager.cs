using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;

public class ProjectScencesManager  {

	public static string[] GetBuildSceneNames()
	{
		EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
		Debug.Log (scenes.Length);
		string[] sceneNames;
		sceneNames = scenes.Select(x=>AsSpacedCamelCase(x.path)).ToArray();
		return sceneNames;
	}


	public static string GetSceneByName(string name)
	{
		foreach (EditorBuildSettingsScene ebss in EditorBuildSettings.scenes) 
		{
			if(Path.GetFileNameWithoutExtension(ebss.path).Equals(name))
			{
				return ebss.path;
			}
		}
		return null;
	}


	[MenuItem("TOOL/Open Scence/ModelScene")]
	public static void OpenModelScene()
	{
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		string path =  GetSceneByName ("ModelView");
		EditorApplication.OpenScene (path);
		EditorApplication.isPlaying = true;
	}

	/// <summary>
	/// 将字符串中的空格去掉变成驼峰格式
	/// </summary>
	/// <returns>The spaced camel case.</returns>
	/// <param name="text">Text.</param>
	public static 	string AsSpacedCamelCase(string text) {
		System.Text.StringBuilder sb = new System.Text.StringBuilder(text.Length*2);
		sb.Append(char.ToUpper(text[0]));
		for(int i=1; i<text.Length;i++) {
			if ( char.IsUpper(text[i]) && text[i-1] != ' ' )
				sb.Append(' ');
			sb.Append (text[i]);
		}
		return sb.ToString();
	}
}
