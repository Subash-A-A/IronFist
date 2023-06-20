using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    
    private Volume volume;
    private ChromaticAberration aberration;
    private ColorAdjustments colorAdjustments;

    private float currentAberration = 0f;
    private float currentSaturation = 0f;

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
    }

    private void Update()
    {
        EffectLerp();
    }

    private void Start()
    {

        volume = GetComponent<Volume>();

        if (volume.profile.TryGet<ChromaticAberration>(out var chr))
        {
            aberration = chr;
        }

        if (volume.profile.TryGet<ColorAdjustments>(out var adj))
        {
            colorAdjustments = adj;
        }
    }

    private void EffectLerp()
    {
        aberration.intensity.value = Mathf.Lerp(aberration.intensity.value, currentAberration, 20 * Time.unscaledDeltaTime);
        colorAdjustments.saturation.value = Mathf.Lerp(colorAdjustments.saturation.value, currentSaturation, 10 * Time.unscaledDeltaTime);
    }

    public void SetEffects(float aberration, float saturation)
    {
        currentAberration = aberration;
        currentSaturation = saturation;
    }

}
