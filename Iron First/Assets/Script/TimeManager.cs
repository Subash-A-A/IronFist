using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float bgPitchTransitionValue = 5f;
    private float slowDownFactor = .1f;
    public static TimeManager Instance;

    private float originalTimeScale;
    private AudioSource bgSource;

    private float currentBgPitch = 1f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        originalTimeScale = Time.timeScale;
    }

    private void Start()
    {
        slowDownFactor = .1f;
        bgSource = AudioManager.Instance.GetAudioSource("bg");
    }

    private void Update()
    {
        bgSource.pitch = Mathf.Lerp(bgSource.pitch, currentBgPitch, bgPitchTransitionValue * Time.unscaledDeltaTime);
    }

    public void StartSlowMotion()
    {
        currentBgPitch = slowDownFactor;
        EffectManager.Instance.SetEffects(1f, -100f);
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    public void StopSlowMotion()
    {
        currentBgPitch = 1f;
        EffectManager.Instance.SetEffects(0f, 0f);
        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = 0.02F;
    }

    public void IncrementSlowDownFactor(float val)
    {
        slowDownFactor += val;
        Debug.Log("Slowdown Factor = " + slowDownFactor);
    }
}
