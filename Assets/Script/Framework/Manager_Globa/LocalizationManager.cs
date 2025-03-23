using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : SingleTon<LocalizationManager>,ISingleTon
{
    public Dictionary<SystemLanguage, Dictionary<string, string>> dic_Localization = new Dictionary<SystemLanguage, Dictionary<string, string>>();
    public SystemLanguage systemLanguage_Globa = SystemLanguage.Chinese;
    public void Init()
    {
        
    }
    public void SetLocalization(SystemLanguage language, string stringFlag, string targetString)
    {
        if(!dic_Localization.ContainsKey(language))
        {
            dic_Localization.Add(language, new Dictionary<string, string>());
        }
        else
        {
            dic_Localization[language].Add(stringFlag, targetString);
        }
    }
    public string GetLocalization(SystemLanguage language, string stringFlag)
    {
        if (dic_Localization.ContainsKey(language)&& dic_Localization[language].ContainsKey(stringFlag))
        {
            return dic_Localization[language][stringFlag];
        }
        else
        {
            return string.Empty;
        }
    }
}
