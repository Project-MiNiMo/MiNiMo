using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    // UI 요소
    public Toggle bgmMuteToggle; // BGM 음소거 토글 버튼
    public Toggle sfxMuteToggle; // SFX 음소거 토글 버튼
    public Slider bgmVolumeSlider; // BGM 볼륨 조절 슬라이더
    public Slider sfxVolumeSlider; // SFX 볼륨 조절 슬라이더

    // Wwise RTPC 이름
    private const string BGM_RTPC = "BGMVolume"; // BGM 볼륨 RTPC
    private const string SFX_RTPC = "SFXVolume"; // SFX 볼륨 RTPC

    void Start()
    {
        // RTPC 값 초기화
        AkSoundEngine.SetRTPCValue(BGM_RTPC, bgmVolumeSlider.value);
        AkSoundEngine.SetRTPCValue(SFX_RTPC, sfxVolumeSlider.value);

        // 이벤트 리스너 초기화
        bgmMuteToggle.onValueChanged.AddListener(SetBGMMute); // BGM 음소거 토글 변경 시 호출
        sfxMuteToggle.onValueChanged.AddListener(SetSFXMute); // SFX 음소거 토글 변경 시 호출
        bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume); // BGM 볼륨 슬라이더 변경 시 호출
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume); // SFX 볼륨 슬라이더 변경 시 호출

        // 초기 볼륨 설정
        bgmVolumeSlider.value = 100f; // 기본값: 최대 볼륨
        sfxVolumeSlider.value = 100f; // 기본값: 최대 볼륨
    }

    // BGM 음소거 설정
    void SetBGMMute(bool isMuted)
    {
        // 음소거 상태에 따라 RTPC 값을 0 또는 슬라이더 값으로 설정
        AkSoundEngine.SetRTPCValue(BGM_RTPC, isMuted ? 0f : bgmVolumeSlider.value);
    }

    // SFX 음소거 설정
    void SetSFXMute(bool isMuted)
    {
        // 음소거 상태에 따라 RTPC 값을 0 또는 슬라이더 값으로 설정
        AkSoundEngine.SetRTPCValue(SFX_RTPC, isMuted ? 0f : sfxVolumeSlider.value);
    }

    // BGM 볼륨 설정
    void SetBGMVolume(float volume)
    {
        // BGM이 음소거 상태가 아닐 때만 RTPC 값 설정
        if (!bgmMuteToggle.isOn)
        {
            AkSoundEngine.SetRTPCValue(BGM_RTPC, volume);
        }
    }

    // SFX 볼륨 설정
    void SetSFXVolume(float volume)
    {
        // SFX가 음소거 상태가 아닐 때만 RTPC 값 설정
        if (!sfxMuteToggle.isOn)
        {
            AkSoundEngine.SetRTPCValue(SFX_RTPC, volume);
        }
    }
}
