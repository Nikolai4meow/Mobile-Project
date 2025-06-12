using UnityEngine;

[CreateAssetMenu(fileName = "New Zombie", menuName = "Zombies/Zombie Data")]
public class ZombieData : ScriptableObject
{
    public string zombieName;
    public int hitsToKill = 3; // How many swipes needed to kill
  

}
