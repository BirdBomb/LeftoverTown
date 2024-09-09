using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffConfigData : MonoBehaviour
{
    public static BuffConfig GetBuffConfig(int ID)
    {
        return buffConfigs.Find((x) => { return x.Buff_ID == ID; });
    }
    public readonly static List<BuffConfig> buffConfigs = new List<BuffConfig>()
    {
        new BuffConfig(){Buff_ID=9000,Buff_Cost=1,Buff_Name="��ǿ��׳",Buff_Desc="����+1"},
        new BuffConfig(){Buff_ID=9001,Buff_Cost=1,Buff_Name="���ͷ",Buff_Desc="����+2������-1"},
        new BuffConfig(){Buff_ID=9002,Buff_Cost=1,Buff_Name="ȫ���ע",Buff_Desc="רע+1"},
        new BuffConfig(){Buff_ID=9003,Buff_Cost=1,Buff_Name="�����",Buff_Desc="����+2��רע-1"},
        new BuffConfig(){Buff_ID=9004,Buff_Cost=1,Buff_Name="��������",Buff_Desc="����+1"},
        new BuffConfig(){Buff_ID=9005,Buff_Cost=2,Buff_Name="�ʹ�л",Buff_Desc="�������½�����"},
        new BuffConfig(){Buff_ID=9006,Buff_Cost=2,Buff_Name="������",Buff_Desc="������½�����"},
        new BuffConfig(){Buff_ID=9007,Buff_Cost=2,Buff_Name="Ƥ�����",Buff_Desc="��ʼ����+1"},
        new BuffConfig(){Buff_ID=9008,Buff_Cost=3,Buff_Name="��ʳƷ����",Buff_Desc="ʳ���ṩ������仯����"},
        new BuffConfig(){Buff_ID=9009,Buff_Cost=3,Buff_Name="����սʿ",Buff_Desc="��ɱ�ظ�����ֵ"},
        new BuffConfig(){Buff_ID=9010,Buff_Cost=4,Buff_Name="�質��",Buff_Desc="���Գ���"},
        new BuffConfig(){Buff_ID=9011,Buff_Cost=4,Buff_Name="������",Buff_Desc="����ֵ���۸���"},
        new BuffConfig(){Buff_ID=9012,Buff_Cost=3,Buff_Name="���ǹ���",Buff_Desc="����һ���Ϸ�����"},
        new BuffConfig(){Buff_ID=9013,Buff_Cost=4,Buff_Name="�����ΰ���",Buff_Desc="����һ�������ΰ���"},
        new BuffConfig(){Buff_ID=9014,Buff_Cost=6,Buff_Name="���ǹ���",Buff_Desc="����һ�������ϵĹ���"},
        new BuffConfig(){Buff_ID=9015,Buff_Cost=6,Buff_Name="��ͽ",Buff_Desc="��������ĳ����ʦ��ͽ��,�Դ�һЩ�������"},
        new BuffConfig(){Buff_ID=9016,Buff_Cost=6,Buff_Name="�Ҿ���ʵ",Buff_Desc="�������˻�����һЩ���"},
        new BuffConfig(){Buff_ID=9017,Buff_Cost=6,Buff_Name="�������ʦ",Buff_Desc="�㿪���ɹ���������"},
        new BuffConfig(){Buff_ID=9018,Buff_Cost=2,Buff_Name="̼��ˮ������",Buff_Desc="��ʳ��̼�����ϻ�ö�������ֵ�Ͷ�������"},
        new BuffConfig(){Buff_ID=9019,Buff_Cost= 1,Buff_Name="�����",Buff_Desc="���������Ʒʱ����ֵ����"},

        new BuffConfig(){Buff_ID=8000,Buff_Cost=-1,Buff_Name="��������",Buff_Desc="����-1"},
        new BuffConfig(){Buff_ID=8001,Buff_Cost=-1,Buff_Name="С����",Buff_Desc="����-2������+1"},
        new BuffConfig(){Buff_ID=8002,Buff_Cost=-1,Buff_Name="��ɢ˼ά",Buff_Desc="רע-2������+1"},
        new BuffConfig(){Buff_ID=8003,Buff_Cost=-1,Buff_Name="���ֱ���",Buff_Desc="����-2"},
        new BuffConfig(){Buff_ID=8004,Buff_Cost=-2,Buff_Name="�ߴ�л",Buff_Desc="�������½����"},
        new BuffConfig(){Buff_ID=8005,Buff_Cost=-2,Buff_Name="����֢",Buff_Desc="������½����"},
        new BuffConfig(){Buff_ID=8006,Buff_Cost=-2,Buff_Name="��ʳ",Buff_Desc="ʳ���ṩ��������淭��"},
        new BuffConfig(){Buff_ID=8007,Buff_Cost=-2,Buff_Name="ζ��ʧ��",Buff_Desc="ʳ���ṩ�������������"},
        new BuffConfig(){Buff_ID=8008,Buff_Cost=-2,Buff_Name="��������",Buff_Desc="��ɱ����ֵ�½�"},
        new BuffConfig(){Buff_ID=8009,Buff_Cost=-3,Buff_Name="������",Buff_Desc="ż��������"},
        new BuffConfig(){Buff_ID=8010,Buff_Cost=-3,Buff_Name="�������",Buff_Desc="����ֵ���۸���"},
        new BuffConfig(){Buff_ID=8011,Buff_Cost=-4,Buff_Name="ͨ����",Buff_Desc="����һ��ͨ����"},
        new BuffConfig(){Buff_ID=8012,Buff_Cost=-4,Buff_Name="С����",Buff_Desc="һ����ʷ��ʹ����������"},
        new BuffConfig(){Buff_ID=8013,Buff_Cost=-6,Buff_Name="��ծ����",Buff_Desc="�������˻�����һЩǷ��"},
        new BuffConfig(){Buff_ID=8014,Buff_Cost= -3,Buff_Name="������",Buff_Desc="���������Ʒʱ����ֵ�½�"},
  };
}
public struct BuffConfig
{
    public short Buff_ID;
    public string Buff_Name;
    public string Buff_Desc;
    public int Buff_Cost;
}
