
/// <summary>
/// ��Ϸ����
/// </summary>
using UnityEngine;
[System.Serializable]
public class GameConfigData : ScriptableObject
{
    /// <summary>
    /// ������Դ���ļ���·��
    /// </summary>
    public string BackgroundPath = "/1/BGMainTexture";

    /// <summary>
    /// ��̬����,����ͼ�������������֮ǰ�п��ܼ��أ���Ԫ����ʱ����Ҫ��һ�¾�̬����
    /// </summary>
    public static string BGFile = "/1/BGMainTexture";

    /// <summary>
    /// Ԫ������
    /// </summary>
    public static string GoldName = "Ԫ��";
    /// <summary>
    /// �ʻ�����
    /// </summary>
    public static string FlowerName = "�ʻ�";
    /// <summary>
    /// ��������
    /// </summary>
    public static string StamName = "����";
    /// <summary>
    /// ��������
    /// </summary>
    public static string SilverName = "����";

    /// <summary>
    /// Ԫ��ICO
    /// </summary>
    public static string GoldIco = "88888";
    /// <summary>
    /// �ʻ�ICO
    /// </summary>
    public static string FlowerIco = "99999";
    /// <summary>
    /// ����ICO
    /// </summary>
    public static string StamIco = "88887";
    /// <summary>
    /// ����ICO
    /// </summary>
    public static string SilverIco = "88889";

    /// <summary>
    /// ����ÿ��ս�����������ܶ�
    /// </summary>
    public static int Bosom = 1;

    /// <summary>
    /// ������Դ���ļ���·��
    /// </summary>
    public string CardPath = "/1/Card";
    /// <summary>
    /// ��̬����
    /// </summary>
    //public static string CardFile = "/1/Card";
    /// <summary>
    /// ս��������Դ��·��
    /// </summary>
    public string BatterCardPath =  "/1/Card"; //"/1/BatterCard";
    /// <summary>
    /// ��̬����
    /// </summary>
    //public static string BatterCardFile = "/1/BatterCard";
    /// <summary>
    /// ��Ч��Դ��·��
    /// </summary>
    public string AudioPath = "/1/Audio";
    /// <summary>
    /// ������Դ��·��
    /// </summary>
    public string SoundPath = "/1/Sound";

    /// <summary>
    /// ���Ƴߴ�
    /// </summary>
    public Rect CardDimension = new Rect(0, 0, 512f, 512f);

    /// <summary>
    /// UVType
    /// </summary>
    public Rect[] UVTypeRect = new Rect[2];

    /// <summary>
    /// Ĭ����Ϸ�ٶ�
    /// </summary>
    public float GameTimeScale = 1f;

    /// <summary>
    /// Ĭ����Դ�ӳٻ���ʱ�䣬��λS
    /// </summary>
    public int UnLoadTime = 20;

    /// <summary>
    /// Ĭ�Ͽ�����Դ�ӳٻ���ʱ�䣬��λS
    /// </summary>
    public int UnLoadTimeCard = 50;

    /// <summary>
    /// Ĭ������ϵͳ�ӳٻ���ʱ�䣬��λS
    /// </summary>
    public int UnLoadTimeGuild = 60;

    /// <summary>
    /// Ĭ��������Ϣ��ѯʱ�䣬��λS
    /// </summary>
    public int LoadTimeChat = 8;

    /// <summary>
    /// ��������Ŷ�����������󻺴�����
    /// </summary>
    public int MaxCacheCount = 10;

    /// <summary>
    /// ����������󻺴�����
    /// </summary>
    public int MaxChatCacheCount = 30;

    /// <summary>
    /// 
    /// </summary>
    public GameConfigData()
    { }
}
