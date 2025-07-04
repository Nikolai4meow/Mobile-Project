using System.Collections.Generic;

[System.Serializable] //  for Json
public class SaveData
{
    public string username;
    public int highestUnlockedLevel = 1;
    
    public Dictionary<string,int> levelScores = new Dictionary<string,int>();
   
}
