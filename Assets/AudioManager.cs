using System.Collections;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioClip[] MusicTracks;
    public AudioSource MusicPlayer;
    public AudioSource EffectPlayer;
    private int musicClipIndex = 0;
    public AudioClip HoverSound0;
    public AudioClip ClickSound0;
    public void PlayHoverSound()
    {
        MusicPlayer.PlayOneShot(HoverSound0);
    }
    public void PlayClickSound()
    {
        MusicPlayer.PlayOneShot(ClickSound0);
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Instance.MusicPlayer = new GameObject("MainAudioPlayer").AddComponent<AudioSource>();
        Instance.MusicPlayer.gameObject.transform.parent = transform;
        DontDestroyOnLoad(Instance.MusicPlayer.gameObject);

        Instance.EffectPlayer = new GameObject("MainAudioPlayer").AddComponent<AudioSource>();
        Instance.EffectPlayer.gameObject.transform.parent = transform;
        DontDestroyOnLoad(Instance.EffectPlayer.gameObject);

        DontDestroyOnLoad(gameObject);

        Instance.MusicPlayer.GetComponent<AudioSource>().minDistance = 1;
        Instance.MusicPlayer.GetComponent<AudioSource>().maxDistance = 1;

        Instance.EffectPlayer.GetComponent<AudioSource>().minDistance = 1;
        Instance.EffectPlayer.GetComponent<AudioSource>().maxDistance = 1;

        Instance.MusicTracks = Resources.LoadAll<AudioClip>("Sounds/Music");
        StartCoroutine(PlayMusic());
    }
    private IEnumerator PlayMusic()
    {
        while (true)
        {
            AudioClip currentClip = MusicTracks[musicClipIndex];
            MusicPlayer.clip = currentClip;
            MusicPlayer.Play();
            yield return new WaitForSecondsRealtime(musicClipIndex == 0 ? currentClip.length - 27 : currentClip.length + 2);
            MusicPlayer.Stop();
            musicClipIndex++;
            musicClipIndex = musicClipIndex >= MusicTracks.Length ? 0 : musicClipIndex;
        }
    }
    public static void SetMusicVolume(float volume)
    {
        SettingsHandler.settings.MusicVolume = volume;
        Instance.MusicPlayer.volume = volume;
    }
    public static void SetEffectsVolume(float volume)
    {
        SettingsHandler.settings.EffectsVolume = volume;
        Instance.EffectPlayer.volume = volume;
    }
    public static void SaveSettings()
    {
        SettingsHandler.SaveSettings();
    }
}