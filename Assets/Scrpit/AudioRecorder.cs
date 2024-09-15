using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    public AudioSource audioSource; // ������ ����� �� ����� AudioSource
    private AudioClip recordedClip;
    private string filePath;

    void Start()
    {
        // ������ ������ ��� ���� (�ȵ���̵忡���� Application.persistentDataPath ���)
        filePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
    }

    // ���� ����
    public void StartRecording()
    {
        if (Microphone.devices.Length > 0)
        {
            recordedClip = Microphone.Start(null, false, 10, 44100); // 10�� ����, ���� ����Ʈ 44100Hz
        }
        else
        {
            Debug.LogWarning("No microphone found.");
        }
    }

    // ���� ���� �� ���� ����
    public void StopRecording()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);

            SaveAudioClip(recordedClip, filePath);
            Debug.Log("Recording saved at: " + filePath);
        }
    }

    // ������ ���� ���
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

    // ����� Ŭ�� ���� (WAV ����)
    private void SaveAudioClip(AudioClip clip, string path)
    {
        // WAV �������� �����ϴ� Ŀ���� �޼��� (������ ���� SavWav Ŭ������ �ʿ���)
        SavWav.Save(path, clip);
    }

    // ������ �о���� ���
    private System.Collections.IEnumerator LoadAudioAndPlay(string path)
    {
        WWW www = new WWW("file://" + path);
        yield return www;

        AudioClip audioClip = www.GetAudioClip(false, true, AudioType.WAV);
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
