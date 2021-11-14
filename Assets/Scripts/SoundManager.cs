using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip ac)
    {
        sfxAudioSource.clip = ac;
        sfxAudioSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        sfxAudioSource.pitch = randomPitch;
        sfxAudioSource.clip = clips[randomIndex];
        sfxAudioSource.Play();
    }


}
