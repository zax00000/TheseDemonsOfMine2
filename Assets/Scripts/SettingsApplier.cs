using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsApplier : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer volumeMixer;

    [Header("Graphics")]
    [SerializeField] private Volume globalVolume;

    private const float minExposure = -2f;
    private const float maxExposure = 2f;

    private void Start()
    {
        ApplyVolumeSettings();
        ApplyBrightnessSetting();
    }

    private void ApplyVolumeSettings()
    {
        float master = PlayerPrefs.GetFloat("masterVolume", 1f);
        float music = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("sfxVolume", 1f);

        volumeMixer.SetFloat("MasterVol", LinearToDb(master));
        volumeMixer.SetFloat("MusicVol", LinearToDb(music));
        volumeMixer.SetFloat("SfxVol", LinearToDb(sfx));
    }

    private void ApplyBrightnessSetting()
    {
        if (globalVolume != null && globalVolume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            float brightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
            colorAdjustments.postExposure.value = Mathf.Lerp(minExposure, maxExposure, brightness);
        }
        else
        {
            Debug.LogWarning("ColorAdjustments not found in Volume Profile.");
        }
    }

    private float LinearToDb(float value)
    {
        return (value <= 0.0001f) ? -80f : 20f * Mathf.Log10(value);
    }
}
