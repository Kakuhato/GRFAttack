using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : RegulatorSingleton<GameDataManager>
{
    public MusicData musicData;
    public DollData dollData = new DollData();

    public void SaveMusicData()
    {
        JSManager.Instance.SaveData(musicData, "MusicData");
    }

    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        musicData = JSManager.Instance.LoadData<MusicData>("MusicData");
        
    }
}
