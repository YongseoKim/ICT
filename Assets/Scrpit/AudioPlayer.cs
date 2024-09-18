using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    public Transform buttonContainer; // 동적 버튼을 넣을 UI 컨테이너 (Vertical Layout Group)
    public Button buttonPrefab; // 버튼 프리팹
    public Button backButton;
    public AudioSource audioSource;

    void Start()
    {
        LoadAudioFiles();
        backButton.onClick.AddListener(GoToRecordScene);
    }

    void LoadAudioFiles()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.wav"); // 모든 .wav 파일 가져오기

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);

            // 버튼을 동적으로 생성
            Button newButton = Instantiate(buttonPrefab, buttonContainer);
            newButton.GetComponentInChildren<Text>().text = fileName; // 버튼의 텍스트를 파일 이름으로 설정
            newButton.onClick.AddListener(() => PlayAudio(file)); // 버튼 클릭 시 해당 파일 재생
        }
    }

    void PlayAudio(string filePath)
    {
        StartCoroutine(LoadAndPlayAudio(filePath));
    }

    IEnumerator LoadAndPlayAudio(string path)
    {
        using (WWW www = new WWW("file://" + path))
        {
            yield return www;

            audioSource.clip = www.GetAudioClip(false, false);
            audioSource.Play();
            Debug.Log("Playing audio: " + path);
        }
    }

    // Back 버튼 클릭 시 RecordScene으로 전환
    void GoToRecordScene()
    {
        SceneManager.LoadScene("RecordScene");
    }
}
