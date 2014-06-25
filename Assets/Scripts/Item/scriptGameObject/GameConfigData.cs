
/// <summary>
/// 游戏配置
/// </summary>
using UnityEngine;
[System.Serializable]
public class GameConfigData : ScriptableObject
{
    /// <summary>
    /// 背景资源包文件夹路径
    /// </summary>
    public string BackgroundPath = "/1/BGMainTexture";

    /// <summary>
    /// 静态备份,背景图在主场景类加载之前有可能加载，单元测试时，需要用一下静态备份
    /// </summary>
    public static string BGFile = "/1/BGMainTexture";

    /// <summary>
    /// 元宝名称
    /// </summary>
    public static string GoldName = "元宝";
    /// <summary>
    /// 鲜花名称
    /// </summary>
    public static string FlowerName = "鲜花";
    /// <summary>
    /// 体力名称
    /// </summary>
    public static string StamName = "体力";
    /// <summary>
    /// 银币名称
    /// </summary>
    public static string SilverName = "银币";

    /// <summary>
    /// 元宝ICO
    /// </summary>
    public static string GoldIco = "88888";
    /// <summary>
    /// 鲜花ICO
    /// </summary>
    public static string FlowerIco = "99999";
    /// <summary>
    /// 体力ICO
    /// </summary>
    public static string StamIco = "88887";
    /// <summary>
    /// 银币ICO
    /// </summary>
    public static string SilverIco = "88889";

    /// <summary>
    /// 好友每场战斗提升的亲密度
    /// </summary>
    public static int Bosom = 1;

    /// <summary>
    /// 卡牌资源包文件夹路径
    /// </summary>
    public string CardPath = "/1/Card";
    /// <summary>
    /// 静态备份
    /// </summary>
    //public static string CardFile = "/1/Card";
    /// <summary>
    /// 战斗卡牌资源包路径
    /// </summary>
    public string BatterCardPath =  "/1/Card"; //"/1/BatterCard";
    /// <summary>
    /// 静态备份
    /// </summary>
    //public static string BatterCardFile = "/1/BatterCard";
    /// <summary>
    /// 音效资源包路径
    /// </summary>
    public string AudioPath = "/1/Audio";
    /// <summary>
    /// 音乐资源包路径
    /// </summary>
    public string SoundPath = "/1/Sound";

    /// <summary>
    /// 卡牌尺寸
    /// </summary>
    public Rect CardDimension = new Rect(0, 0, 512f, 512f);

    /// <summary>
    /// UVType
    /// </summary>
    public Rect[] UVTypeRect = new Rect[2];

    /// <summary>
    /// 默认游戏速度
    /// </summary>
    public float GameTimeScale = 1f;

    /// <summary>
    /// 默认资源延迟回收时间，单位S
    /// </summary>
    public int UnLoadTime = 20;

    /// <summary>
    /// 默认卡牌资源延迟回收时间，单位S
    /// </summary>
    public int UnLoadTimeCard = 50;

    /// <summary>
    /// 默认明星系统延迟回收时间，单位S
    /// </summary>
    public int UnLoadTimeGuild = 60;

    /// <summary>
    /// 默认聊天信息轮询时间，单位S
    /// </summary>
    public int LoadTimeChat = 8;

    /// <summary>
    /// 其他玩家团队阵容数据最大缓存数量
    /// </summary>
    public int MaxCacheCount = 10;

    /// <summary>
    /// 聊天数据最大缓存数量
    /// </summary>
    public int MaxChatCacheCount = 30;

    /// <summary>
    /// 
    /// </summary>
    public GameConfigData()
    { }
}
