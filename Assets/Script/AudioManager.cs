using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("🎵 Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;

    [Range(0f, 1f)] [SerializeField] private float menuMusicVolume = 0.8f;
    [Range(0f, 1f)] [SerializeField] private float gameplayMusicVolume = 0.6f;

    [Header("🔊 Sound Effects")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip poolerClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip explodeClip;
    [SerializeField] private AudioClip collectStarClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // === MUSIC ===
    private void PlayMusic(AudioClip clip, float volume)
    {
        if (clip == null) return;

        if (musicSource.clip != clip)
        {
            musicSource.clip = clip;
        }

        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayMenuMusic() => PlayMusic(menuMusic, menuMusicVolume);
    public void PlayGameplayMusic() => PlayMusic(gameplayMusic, gameplayMusicVolume);

    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }

    // === SOUND EFFECTS ===
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayPooler() => PlaySFX(poolerClip);
    public void PlayHit() => PlaySFX(hitClip);
    public void PlayExplosion() => PlaySFX(explodeClip);
    public void PlayCollectStar() => PlaySFX(collectStarClip);
}
