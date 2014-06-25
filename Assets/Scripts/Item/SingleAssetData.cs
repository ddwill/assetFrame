/// <summary>
/// 资源类型
/// </summary>
using UnityEngine;
public enum AssetType
{
    /// <summary>
    /// 卡牌
    /// </summary>
    Card = 0,
    /// <summary>
    /// 背景
    /// </summary>
    Background,
    ///// <summary>
    ///// 卡牌头像
    ///// </summary>
    BatterCard,
    ///// <summary>
    ///// 卡牌头像
    ///// </summary>
    //CardHand,
    ///// <summary>
    ///// 主角卡牌
    ///// </summary>
    //CardMain,
    /// <summary>
    /// 音乐
    /// </summary>
    Sound,
    /// <summary>
    /// 音效
    /// </summary>
    Audio,
    Count
}

/// <summary>
/// 单资源实体
/// </summary>
public class SingleAssetData
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public AssetType type;

    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
        get { return m_Object != null ? m_Object.name : ""; }
    }

    /// <summary>
    /// AssetBundle压缩资源
    /// </summary>
    public AssetBundle m_AssetBundle;

    /// <summary>
    /// 解压缩资源
    /// </summary>
    public Object m_Object = new Object();

    /// <summary>
    /// 回收时间
    /// </summary>
    public System.DateTime UnLoadTime;

    /// <summary>
    /// 删除压缩文件
    /// </summary>
    /// <param name="unloadall">是否删除所有</param>
    public void UnLoad(bool unloadall)
    {
        if (m_AssetBundle != null)
        {
            m_AssetBundle.Unload(unloadall);
            m_AssetBundle = null;
        }
    }

    /// <summary>
    /// 删除压缩文件
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="unloadall"></param>
    public void UnLoad(MonoBehaviour mono, bool unloadall)
    {
        if (m_AssetBundle != null)
        {
            GTimer.AddTimer(mono, Global.GetInstance().UnLoadAssetTime, 1, delegate()
            {
                UnLoad(unloadall);
                m_AssetBundle = null;
                //(data as GTimer).Stop();
            }, true);
        }
    }

    /// <summary>
    /// 删除内存资源
    /// </summary>
    public void UnLoadMemory()
    {
        m_Object = null;
    }

    /// <summary>`
    /// 全参数构造
    /// </summary>
    /// <param name="m_AssetBundle"></param>
    /// <param name="m_Object"></param>
    /// <param name="type"></param>
    public SingleAssetData(AssetBundle m_AssetBundle, Object m_Object, AssetType type)
    {
        this.m_AssetBundle = m_AssetBundle;
        this.m_Object = m_Object;
        this.type = type;
    }
}

public class SALoadItem
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public AssetType type;

    /// <summary>
    /// 资源名称
    /// </summary>
    public string name;

    public SALoadItem(AssetType type, string name)
    {
        this.type = type;
        this.name = name;
    }
}
