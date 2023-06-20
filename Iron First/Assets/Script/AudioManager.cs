using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(0.1f, 3f)]
        public float pitch = 1f;

        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }

    public Sound[] sounds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("bg");
    }

    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, s => s.name == name);
        s.source.pitch = s.pitch;
        s.source.Play();
    }

    public void PlayRandomPitch(string name)
    {
        Sound s = System.Array.Find(sounds, s => s.name == name);
        s.source.pitch = Random.Range(0.7f, 1.3f);
        s.source.Play();
    }
    public AudioSource GetAudioSource(string name)
    {
        Sound s = System.Array.Find(sounds, s => s.name == name);
        return s.source;
    }

}
