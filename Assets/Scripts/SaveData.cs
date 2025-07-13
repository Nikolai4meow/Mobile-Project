using System;
using System.Collections.Generic;

[System.Serializable] //  for Json
public class SaveData
{
    public string username;
    public int highestUnlockedLevel = 1;
    
    public int userScore = 0;

    public string lastLoginDate;
   
}
