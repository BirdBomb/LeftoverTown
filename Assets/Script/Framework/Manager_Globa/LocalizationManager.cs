using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationManager : SingleTon<LocalizationManager>,ISingleTon
{
    public void Init()
    {
        
    }
    public string GetLocalization(string table, string entry)
    {
        if (LocalizationSettings.StringDatabase.GetTable(table).GetEntry(entry) != null)
        {
            return LocalizationSettings.StringDatabase.GetTable(table).GetEntry(entry).GetLocalizedString();
        }
        else
        {
            return "NoData";
        }
    }
}
