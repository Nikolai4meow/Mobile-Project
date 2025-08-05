using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip Slide;
    public AudioClip WallHit;
    public AudioClip GirlDeath;
    public AudioClip GirlHit;
    public AudioClip GirlWin;
    public AudioClip Zombie1Death;
    public AudioClip Zombie2Death;
    public AudioClip ZombieHit;
    public AudioClip Zombies;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
