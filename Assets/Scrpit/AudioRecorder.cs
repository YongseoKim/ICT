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
    private Image buttonImage; // ��ư�� �̹��� ������Ʈ�� ����Ͽ� ������ ����

    void Start()
    {
        recordButton.onClick.AddListener(ToggleRecording);
        nextButton.onClick.AddListener(GoToListenScene); // Next ��ư Ŭ�� �̺�Ʈ �߰�
        buttonImage = recordButton.GetComponent<Image>();
        buttonImage.color = Color.white;

        nextButton.interactable = false; // ���� �Ϸ� ������ ��Ȱ��ȭ
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
            recordedClip = Microphone.Start(null, false, 60, 44100); // �ִ� 60�� ����
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

            // ������ �Ϸ�Ǹ� Next ��ư Ȱ��ȭ
            nextButton.interactable = true;
        }
    }

    void SaveRecording()
    {
        string fileName = "recordedAudio_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".wav";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        SavWav.Save(filePath, recordedClip);
        PlayerPrefs.SetString("LastSavedAudioPath", filePath); // ���� ��� ����
        PlayerPrefs.Save();
    }

    // Next ��ư Ŭ�� �� ListenScene���� ��ȯ
    void GoToListenScene()
    {
        SceneManager.LoadScene("ListenScene");
    }
}
