using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AudioPlayer : MonoBehaviour
{
    public Button playButton;
    public AudioSource audioSource;
    private string filePath;

    void Start()
    {
        // PlayerPrefs에서 저장된 오디오 파일 경로 불러오기
        filePath = PlayerPrefs.GetString("SavedAudioPath", "");

        if (File.Exists(filePath))
        {
            // 오디오 파일을 로드하고 재생할 준비
            StartCoroutine(LoadAudio(filePath));
        }
        else
        {
            Debug.LogWarning("No audio file found at: " + filePath);
        }

        playButton.onClick.AddListener(PlayAudio);
    }

    IEnumerator LoadAudio(string path)
    {
        using (WWW www = new WWW("file://" + path))
        {
            yield return www;

            audioSource.clip = www.GetAudioClip(false, false);
            Debug.Log("Audio loaded from: " + path);
        }
    }

    void PlayAudio()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
            Debug.Log("Playing audio...");
        }
        else
        {
            Debug.LogWarning("No audio clip loaded to play.");
        }
    }
}
