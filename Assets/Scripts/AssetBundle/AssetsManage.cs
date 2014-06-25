/// <summary>
/// 资源管理基类
/// </summary>
public class AssetsManage
{
    /// <summary>
    /// 本地资源包路径
    /// </summary>
    protected static string DOC_NAME
    {
        get { return Global.GetInstance().docFile; }
    }

    /// <summary>
    /// 网络资源包路径
    /// </summary>
    protected static string WWW_NAME
    {
        get { return Global.GetInstance().urlVersion; }
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    protected static LoadEvents m_Events = new LoadEvents();

    /// <summary>
    /// 单资源事件处理
    /// </summary>
    protected static LoadEvents singleAssetEvents = new LoadEvents();

    /// <summary>
    /// 消息对象,监听消息方法events.onLoadOk(OnLoadOk)
    /// private void OnLoadOk(obj:Object)   obj = m_szAssetBundle
    /// 当有文件加载失败时,回推送onLoadErr , obj = 资源加载属性类
    /// </summary>
    public static LoadEvents events
    {
        get
        {
            return m_Events;
        }
    }
}
