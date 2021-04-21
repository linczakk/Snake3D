using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip PickUpSound, DeadSound, BombSound, BoomSound, StartGameCounterTick;

    private void Awake()
    {
        MakeInstance();
    }

    private void MakeInstance()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayPickUpSound()
    {
        AudioSource.PlayClipAtPoint(PickUpSound, transform.position);
    }

    public void PlayDeadSound()
    {
        AudioSource.PlayClipAtPoint(DeadSound, transform.position);
    }

    public void RespawnBombSound()
    {
        AudioSource.PlayClipAtPoint(BombSound, transform.position);
    }

    public void DetonateBombSound()
    {
        AudioSource.PlayClipAtPoint(BoomSound, transform.position);
    }

    public void MakeTickSound()
    {
        AudioSource.PlayClipAtPoint(StartGameCounterTick, transform.position);
    }

    public void ShouldTurnOffSound(bool shouldTurnOff)
    {
        if (shouldTurnOff)
        {
            TurnSoundOff();
        }
        else
        {
            TurnSoundOn();
        }
    }

    private void TurnSoundOn()
    {
        AudioListener.volume = 1;
        AudioController._audioOff = true;
    }
    private void TurnSoundOff()
    {
        AudioListener.volume = 0;
        AudioController._audioOff = false;
    }

}

public  class AudioController
{
    public static bool _audioOff = true;
}