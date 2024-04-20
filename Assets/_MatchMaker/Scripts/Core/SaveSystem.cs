using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private string _savefileName;
    public SaveProfile LoadProfile()
    {
        return Load<SaveProfile>(_savefileName);
    }
    public void SaveProfile(SaveProfile profile)
    {
        Save(profile, _savefileName);
    }
    /// <summary>
    /// Generic Save method that can save any serializable objects into json file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="profile"></param>
    /// <param name="fileName"></param>
    public void Save<T>(T profile, string fileName)
    {
        JsonHandler.SaveToJson(profile, fileName);
    }
    /// <summary>
    /// Generic Load method to load any serializable objects, if file doesn't exisit it will create one
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="savefileName"></param>
    /// <returns></returns>
    public T Load<T>(string savefileName) where  T: new()
    {
        return JsonHandler.LoadFromJson<T>(savefileName);
    }
}
public class SaveProfile
{
    public int lastPlayedLevelIndex;
    public SaveProfile(int levelIndex)
    {
        lastPlayedLevelIndex = levelIndex;
    }
    public SaveProfile() {
        lastPlayedLevelIndex = 0;
    }
    //TODO: Can add list<Score> or streaks to track player stats
}
