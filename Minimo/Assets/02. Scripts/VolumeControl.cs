using UnityEngine;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    // UI ���
    public Toggle bgmMuteToggle; // BGM ���Ұ� ��� ��ư
    public Toggle sfxMuteToggle; // SFX ���Ұ� ��� ��ư
    public Slider bgmVolumeSlider; // BGM ���� ���� �����̴�
    public Slider sfxVolumeSlider; // SFX ���� ���� �����̴�

    // Wwise RTPC �̸�
    private const string BGM_RTPC = "BGMVolume"; // BGM ���� RTPC
    private const string SFX_RTPC = "SFXVolume"; // SFX ���� RTPC

    void Start()
    {
        // RTPC �� �ʱ�ȭ
        AkSoundEngine.SetRTPCValue(BGM_RTPC, bgmVolumeSlider.value);
        AkSoundEngine.SetRTPCValue(SFX_RTPC, sfxVolumeSlider.value);

        // �̺�Ʈ ������ �ʱ�ȭ
        bgmMuteToggle.onValueChanged.AddListener(SetBGMMute); // BGM ���Ұ� ��� ���� �� ȣ��
        sfxMuteToggle.onValueChanged.AddListener(SetSFXMute); // SFX ���Ұ� ��� ���� �� ȣ��
        bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume); // BGM ���� �����̴� ���� �� ȣ��
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume); // SFX ���� �����̴� ���� �� ȣ��

        // �ʱ� ���� ����
        bgmVolumeSlider.value = 100f; // �⺻��: �ִ� ����
        sfxVolumeSlider.value = 100f; // �⺻��: �ִ� ����
    }

    // BGM ���Ұ� ����
    void SetBGMMute(bool isMuted)
    {
        // ���Ұ� ���¿� ���� RTPC ���� 0 �Ǵ� �����̴� ������ ����
        AkSoundEngine.SetRTPCValue(BGM_RTPC, isMuted ? 0f : bgmVolumeSlider.value);
    }

    // SFX ���Ұ� ����
    void SetSFXMute(bool isMuted)
    {
        // ���Ұ� ���¿� ���� RTPC ���� 0 �Ǵ� �����̴� ������ ����
        AkSoundEngine.SetRTPCValue(SFX_RTPC, isMuted ? 0f : sfxVolumeSlider.value);
    }

    // BGM ���� ����
    void SetBGMVolume(float volume)
    {
        // BGM�� ���Ұ� ���°� �ƴ� ���� RTPC �� ����
        if (!bgmMuteToggle.isOn)
        {
            AkSoundEngine.SetRTPCValue(BGM_RTPC, volume);
        }
    }

    // SFX ���� ����
    void SetSFXVolume(float volume)
    {
        // SFX�� ���Ұ� ���°� �ƴ� ���� RTPC �� ����
        if (!sfxMuteToggle.isOn)
        {
            AkSoundEngine.SetRTPCValue(SFX_RTPC, volume);
        }
    }
}
