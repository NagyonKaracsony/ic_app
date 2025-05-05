using System.Collections;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioClip[] MusicTracks;
    private AudioSource MusicPlayer;
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
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Instance.MusicPlayer = new GameObject("MainAudioPlayer").AddComponent<AudioSource>();
        Instance.MusicPlayer.gameObject.transform.parent = transform;
        DontDestroyOnLoad(Instance.MusicPlayer.gameObject);
        DontDestroyOnLoad(gameObject);

        // Load audio clips
        Instance.MusicPlayer.GetComponent<AudioSource>().minDistance = 1;
        Instance.MusicPlayer.GetComponent<AudioSource>().maxDistance = 1;
        Instance.MusicTracks = Resources.LoadAll<AudioClip>("Sounds/Music");
        StartCoroutine(PlayMusic());
    }
    private IEnumerator PlayMusic()
    {
        while (true)
        {
            AudioClip currentClip = MusicTracks[musicClipIndex];
            MusicPlayer.clip = currentClip;
            MusicPlayer.volume = 0.75f;
            MusicPlayer.Play();
            yield return new WaitForSecondsRealtime(musicClipIndex == 0 ? currentClip.length - 27 : currentClip.length + 2);
            MusicPlayer.Stop();
            musicClipIndex++;
            musicClipIndex = musicClipIndex >= MusicTracks.Length ? 0 : musicClipIndex;
        }
    }
}