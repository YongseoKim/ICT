using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    public AudioSource audioSource; // 녹음을 재생할 때 사용할 AudioSource
    private AudioClip recordedClip;
    private string filePath;

    void Start()
    {
        // 파일을 저장할 경로 설정 (안드로이드에서는 Application.persistentDataPath 사용)
        filePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
    }

    // 녹음 시작
    public void StartRecording()
    {
        if (Microphone.devices.Length > 0)
        {
            recordedClip = Microphone.Start(null, false, 10, 44100); // 10초 제한, 샘플 레이트 44100Hz
        }
        else
        {
            Debug.LogWarning("No microphone found.");
        }
    }

    // 녹음 종료 및 파일 저장
    public void StopRecording()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);

            SaveAudioClip(recordedClip, filePath);
            Debug.Log("Recording saved at: " + filePath);
        }
    }

    // 녹음된 파일 재생
    public void PlayRecordedAudio()
    {
        if (File.Exists(filePath))
        {
            StartCoroutine(LoadAudioAndPlay(filePath));
        }
        else
        {
            Debug.LogWarning("No recorded file found.");
        }
    }

    // 오디오 클립 저장 (WAV 포맷)
    private void SaveAudioClip(AudioClip clip, string path)
    {
        // WAV 포맷으로 저장하는 커스텀 메서드 (이전에 만든 SavWav 클래스가 필요함)
        SavWav.Save(path, clip);
    }

    // 파일을 읽어오고 재생
    private System.Collections.IEnumerator LoadAudioAndPlay(string path)
    {
        WWW www = new WWW("file://" + path);
        yield return www;

        AudioClip audioClip = www.GetAudioClip(false, true, AudioType.WAV);
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
