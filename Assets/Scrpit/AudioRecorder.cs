using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioRecorder : MonoBehaviour
{
    public Button recordButton;
    public Button nextButton;
    public AudioSource audioSource;
    private bool isRecording = false;
    private AudioClip recordedClip;
    private Image buttonImage; // 버튼의 이미지 컴포넌트를 사용하여 색상을 변경

    void Start()
    {
        recordButton.onClick.AddListener(ToggleRecording);
        nextButton.onClick.AddListener(GoToListenScene); // Next 버튼 클릭 이벤트 추가
        buttonImage = recordButton.GetComponent<Image>();
        buttonImage.color = Color.white;

        nextButton.interactable = false; // 녹음 완료 전까지 비활성화
    }

    void ToggleRecording()
    {
        if (isRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
    }

    void StartRecording()
    {
        if (Microphone.devices.Length > 0)
        {
            isRecording = true;
            recordedClip = Microphone.Start(null, false, 60, 44100); // 최대 60초 녹음
            buttonImage.color = Color.red;
        }
        else
        {
            Debug.LogWarning("No microphone detected!");
        }
    }

    void StopRecording()
    {
        if (isRecording)
        {
            Microphone.End(null);
            isRecording = false;
            SaveRecording();
            buttonImage.color = Color.white;

            // 녹음이 완료되면 Next 버튼 활성화
            nextButton.interactable = true;
        }
    }

    void SaveRecording()
    {
        string fileName = "recordedAudio_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".wav";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        SavWav.Save(filePath, recordedClip);
        PlayerPrefs.SetString("LastSavedAudioPath", filePath); // 파일 경로 저장
        PlayerPrefs.Save();
    }

    // Next 버튼 클릭 시 ListenScene으로 전환
    void GoToListenScene()
    {
        SceneManager.LoadScene("ListenScene");
    }
}
