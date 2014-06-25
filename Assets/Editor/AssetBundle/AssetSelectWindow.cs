using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public class AssetSelectWindow : EditorWindow
{
    /// <summary>
    /// 资源打包设置数据
    /// </summary>
    public List<AssetLevelItem> AssetLevels;
    /// <summary>
    /// 打开编译目录设置窗口
    /// </summary>
    [MenuItem("TOOL/Select and Create AssetBunldes")]
    public static void SelectAssetBunldes()
    {
        //打开窗口
        AssetSelectWindow assetSelectWindow = EditorWindow.GetWindow<AssetSelectWindow>(false, "设置编译目录", true);
        assetSelectWindow.init();
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    void init()
    {
        //初始化资源数据
        this.AssetLevels = AssetLevelItem.CreateDefaultLevel();
        //初始化数组
        this.mScroll = new Vector2[AssetLevels.Count];
    }
    /// <summary>
    /// 滚动条位置（用于GUI）
    /// </summary>
    Vector2[] mScroll = null;
    /// <summary>
    /// 删除包名标记（用于GUI）
    /// </summary>
    List<string> mDelNames = new List<string>();

    void OnSelectionChange() { mDelNames.Clear(); Repaint(); }

    /// <summary>
    /// 项目版本号
    /// </summary>
    public string ProjectVersion = "";

    /// <summary>
    /// GUI帧渲染
    /// </summary>
    void OnGUI()
    {
        //打包配置信息不为空
        if (AssetLevels != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("项目版本：", GUILayout.Width(60f));
            ProjectVersion = GUILayout.TextField(ProjectVersion, GUILayout.Width(150f));
            GUILayout.EndHorizontal();

            #region 级别遍历
            AssetLevels.ForEach(delegate(AssetLevelItem assetLevel)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Level" + assetLevel.Level + ":" + assetLevel.LevelName);
                GUILayout.EndHorizontal();
                //开启滚动试图
                mScroll[assetLevel.Level - 1] = GUILayout.BeginScrollView(mScroll[assetLevel.Level - 1]);
                //资源包遍历
                if (assetLevel.AssetPackageList != null)
                {
                    assetLevel.AssetPackageList.ForEach(delegate(AssetPackageItem assetPagkage)
                    {
                        #region 包名--删除
                        GUILayout.BeginHorizontal(GUILayout.MinHeight(20f));
                        GUILayout.Label("  包名: ", GUILayout.Width(50f));
                        assetPagkage.PackageName = GUILayout.TextField(assetPagkage.PackageName, GUILayout.Width(100f));

                        //删除按钮显示和数据处理
                        ShowBtnDlt(assetPagkage.PackageName, delegate()
                        {
                            assetLevel.AssetPackageList.Remove(assetPagkage);
                        });
                        //添加默认资源文件夹
                        if (GUILayout.Button("+ 文件夹", GUILayout.Width(70f)))
                            AddDefaultFile(assetPagkage);

                        //资源文件版本
                        GUILayout.Label("  版本号: ", GUILayout.Width(50f));
                        assetPagkage.PackageVersion = GUILayout.TextField(assetPagkage.PackageVersion, GUILayout.Width(50f));

                        GUILayout.Space(5f);

                        //是否单个打包
                        //GUILayout.Label("  单个打包: ", GUILayout.Width(50f));
                        assetPagkage.IsSingle = GUILayout.Toggle(assetPagkage.IsSingle, "单个打包", GUILayout.Width(80f));
                        GUILayout.EndHorizontal();
                        #endregion

                        #region 资源文件夹遍历--新建
                        if (assetPagkage.FilesList != null)
                        {
                            assetPagkage.FilesList.ForEach(delegate(AssetFileItem fileName)
                            {
                                GUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(20f));
                                GUILayout.Label("   Flie: ", GUILayout.Width(50f));
                                //文件夹设置
                                fileName.FilePath = GUILayout.TextField(fileName.FilePath);
                                //a = EditorGUILayout.ObjectField(a, System.Type.GetType("Object"), GUILayout.Width(150f));
                                //删除按钮显示和数据处理
                                ShowBtnDlt(fileName.FilePath, delegate()
                                {
                                    assetPagkage.FilesList.Remove(fileName);
                                });
                                if (GUILayout.Button("Mapping", GUILayout.Width(60f)))
                                {
                                    fileName.FilePath = AssetDatabase.GetAssetPath(Selection.activeObject);
                                }

                                GUILayout.EndHorizontal();
                            });
                        }

                        #endregion
                    });
                }
                GUILayout.BeginHorizontal(GUILayout.MinHeight(20f));
                GUILayout.Space(5f);
                //添加默认包
                if (GUILayout.Button("+ 包", GUILayout.Width(40f)))
                    AddDefaultPackage(assetLevel);

                GUILayout.EndHorizontal();
                GUILayout.EndScrollView();
            });
            #endregion

            //操作按钮
            GUILayout.BeginHorizontal(GUILayout.MinHeight(25f));
            GUILayout.Label("");
            //导入配置
           if (GUILayout.Button("Import...", GUILayout.ExpandWidth(false)))
            {
                string path = EditorUtility.OpenFilePanel("Select XML File", "", "xml");
                this.AssetLevels = XMLAssetConfig.LoadAssetConfig(path);
                if (AssetLevels != null && AssetLevels.Count > 0)
                {
                    ProjectVersion = AssetLevels[0].Version;
                }
            }
            GUILayout.Space(7f);
            //保存配置
            if (GUILayout.Button("Save...", GUILayout.ExpandWidth(false)))
            {
                if (this.AssetLevels != null && this.AssetLevels.Count > 0)
                {
                    string path = EditorUtility.SaveFilePanel("Save XML File", "", "AssetBunlidConfig.xml", "xml");
                    XMLAssetConfig.SaveAssetConfig(this.AssetLevels, ProjectVersion, path);
                    //保存版本信息
                    path = EditorUtility.SaveFilePanel("Save XML File", "", "AssetVersion.xml", "xml");
                    ProjectVersionItem projectVersion = new ProjectVersionItem();

                    projectVersion.Version = ProjectVersion;
                    //遍历组织资源包版本信息
                    List<AssetVersionItem> assetVersionItem = new List<AssetVersionItem>();

                    this.AssetLevels.ForEach(delegate(AssetLevelItem levelItem)
                    {
                        levelItem.AssetPackageList.ForEach(delegate(AssetPackageItem packItem)
                        {
                            if (!packItem.IsSingle)
                                assetVersionItem.Add(new AssetVersionItem(packItem.GetPackageFile(levelItem.Level), int.Parse(packItem.PackageVersion)));
                        });
                    });
                    projectVersion.packageVersionItem = assetVersionItem;
                    //保存
                    XMLAssetVersionConfig.SaveAssetVersionConfig(projectVersion, path);
                }
            }
            GUILayout.Space(7f);
            //打包
            if (GUILayout.Button("Bulid", GUILayout.ExpandWidth(false)))
            {
                BuildAssetBundlesFromDirectory.AssetBundlesByAssetLevelItem(AssetLevels);
            }
            GUILayout.Space(10f);
            GUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// 添加默认包
    /// </summary>
    /// <param name="assetLevel"></param>
    public void AddDefaultPackage(AssetLevelItem assetLevel)
    {
        if (assetLevel.AssetPackageList == null)
            assetLevel.AssetPackageList = new List<AssetPackageItem>();
        assetLevel.AssetPackageList.Add(new AssetPackageItem("", null, "", false));
    }
    /// <summary>
    /// 添加默认资源文件夹
    /// </summary>
    /// <param name="assetLevel"></param>
    public void AddDefaultFile(AssetPackageItem assetPackage)
    {
        if (assetPackage.FilesList == null)
            assetPackage.FilesList = new List<AssetFileItem>();
        assetPackage.FilesList.Add(new AssetFileItem(AssetDatabase.GetAssetPath(Selection.activeObject)));
    }
    /// <summary>
    /// 资源删除事件（用于GUI）
    /// </summary>
    public delegate void OnDelete();

    /// <summary>
    /// 删除按钮显示（用于GUI）
    /// </summary>
    /// <param name="KeyName">删除行按钮标记</param>
    /// <param name="onDelete">删除执行事件</param>
    public void ShowBtnDlt(string KeyName, OnDelete onDelete)
    {
        GUI.backgroundColor = Color.red;
        if (mDelNames.Contains(KeyName))
        {
            //删除包
            if (GUILayout.Button("Delete", GUILayout.Width(60f)))
                onDelete();

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("X", GUILayout.Width(22f)))
            {
                mDelNames.Remove(KeyName);
            }
        }
        else
        {
            if (GUILayout.Button("X", GUILayout.Width(22f))) mDelNames.Add(KeyName);
        }

        GUI.backgroundColor = Color.white;
    }
}
