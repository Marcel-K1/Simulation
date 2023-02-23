/*******************************************************************************
* Project: Simulation
* File   : VolumeManager
* Date   : 09.02.2022
* Author : Marcel Klein
*
* Synchronizes player input for volume settings and saves it for further play sessions in player prefs.
* 
* History:
*    12.02.2022    MK    Created
*******************************************************************************/


using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private AudioMixer m_masterMixer = null;

    [SerializeField]
    private Slider m_masterVolumeSlider = null;

    [SerializeField]
    private Slider m_musicVolumeSlider = null;

    [SerializeField]
    private Slider m_effectsVolumeSlider = null;

    private float m_masterVolume = 0.5f;
    private float m_musicVolume = 0.5f;
    private float m_effectsVolume = 0.5f;

    #endregion

    #region Methods

    public void Start()
    {
        SetMasterVolumeSlider();
        SetMusicVolumeSlider();
        SetEffectsVolumeSlider();
    }

    //Button methods and saving the values in player prefs
    public void ChangeMasterVolume(float _value)
    {
        PlayerPrefs.SetFloat("PlayerMasterVolume", _value);
        PlayerPrefs.Save();
        m_masterMixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("PlayerMasterVolume", 0f));
    }
    public void SetMasterVolumeSlider()
    {
        m_masterVolumeSlider.value = PlayerPrefs.GetFloat("PlayerMasterVolume", 0f);
    }
    public void ChangeMusicVolume(float _value)
    {
        PlayerPrefs.SetFloat("PlayerMusicVolume", _value);
        PlayerPrefs.Save();
        m_masterMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("PlayerMusicVolume", 0f));
        m_musicVolumeSlider.value = PlayerPrefs.GetFloat("PlayerMusicVolume", 0f);
    }
    public void SetMusicVolumeSlider()
    {
        m_musicVolumeSlider.value = PlayerPrefs.GetFloat("PlayerMusicVolume", 0f);
    }
    public void ChangeEffectVolume(float _value)
    {
        PlayerPrefs.SetFloat("PlayerEffectsVolume", _value);
        PlayerPrefs.Save();
        AudioSource walkingEffect = gameObject.GetComponent<AudioSource>();
        walkingEffect.Play();
        m_masterMixer.SetFloat("EffectsVolume", PlayerPrefs.GetFloat("PlayerEffectsVolume", 8f));
        m_effectsVolumeSlider.value = PlayerPrefs.GetFloat("PlayerEffectsVolume", 0f);
    }
    public void SetEffectsVolumeSlider()
    {
        m_effectsVolumeSlider.value = PlayerPrefs.GetFloat("PlayerEffectsVolume", 8f);
    }
    public void MuteMaster(bool _mute)
    {
        if (_mute)
        {
            if (m_masterMixer.GetFloat("MasterVolume", out float volume))
            {
                m_masterVolume = volume;
            }
            m_masterMixer.SetFloat("MasterVolume", -80.0f);
        }
        else
        {
            m_masterMixer.SetFloat("MasterVolume", m_masterVolume);
        }
    }
    public void MuteMusic(bool _mute)
    {
        if (_mute)
        {
            if (m_masterMixer.GetFloat("MusicVolume", out float volume))
            {
                m_musicVolume = volume;
            }
            m_masterMixer.SetFloat("MusicVolume", -80.0f);
        }
        else
        {
            m_masterMixer.SetFloat("MusicVolume", m_musicVolume);
        }
    }
    public void MuteEffects(bool _mute)
    {
        if (_mute)
        {
            if (m_masterMixer.GetFloat("EffectsVolume", out float volume))
            {
                m_effectsVolume = volume;
            }
            m_masterMixer.SetFloat("EffectsVolume", -80.0f);
        }
        else
        {
            m_masterMixer.SetFloat("EffectsVolume", m_effectsVolume);
        }
    }

    #endregion
}
