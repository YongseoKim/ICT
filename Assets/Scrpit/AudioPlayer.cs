using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    public Transform buttonContainer; // ���� ��ư�� ���� UI �����̳� (Vertical Layout Group)
    public Button buttonPrefab; // ��ư ������
    public Button backButton;
    public AudioSource audioSource;

    void Start()
    {
        LoadAudioFiles();
        backButton.onClick.AddListener(GoToRecordScene);
    }

    void LoadAudioFiles()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.wav"); // ��� .wav ���� ��������

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);

            // ��ư�� �������� ����
            Button newButton = Instantiate(buttonPrefab, buttonContainer);
            newButton.GetComponentInChildren<Text>().text = fileName; // ��ư�� �ؽ�Ʈ�� ���� �̸����� ����
            newButton.onClick.AddListener(() => PlayAudio(file)); // ��ư Ŭ�� �� �ش� ���� ���
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

    // Back ��ư Ŭ�� �� RecordScene���� ��ȯ
    void GoToRecordScene()
    {
        SceneManager.LoadScene("RecordScene");
    }
}
