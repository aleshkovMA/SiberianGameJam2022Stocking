using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

/// <summary>
/// Контролирует перемещение персонажа между сценами
/// /<summary>
public class ScineChanger : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private Image fadeImg;
    // Timeline с текстом лора, который запускается во время перехода между сценами.
    [SerializeField] private PlayableDirector lore;
    [SerializeField] private AudioMixerGroup _audioMixer;
    [SerializeField] private RPGTalkSwitcher _rpgTalkSwitcher;
    [SerializeField] private Character _characterScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (levelName)
        {
            case "HeroHouse":
                if (other.CompareTag("Player"))
                {
                    _rpgTalkSwitcher.TurnOnCurrentTurnOffOthers(lore.gameObject.GetComponent<RPGTalk>());
                    StartCoroutine(ChangeColor(fadeImg, Color.clear, Color.black, 0.5f));
                    _characterScript.SetInactive();
                }
                break;
            case "Bar":
                if (other.CompareTag("Player") && DataHolder.barabanshikAdded == true)
                {
                    // Вместо того, чтобы начинать раннер, мы сначала говорим с ментом
                    _rpgTalkSwitcher.TurnOnCurrentTurnOffOthers(lore.gameObject.GetComponent<RPGTalk>());
                    StartCoroutine(ChangeColor(fadeImg, Color.clear, Color.black, 0.5f));
                    _characterScript.SetInactive();
                }
                break;
            case "FriendHouse":
                if (other.CompareTag("Player") && DataHolder.gitaristAdded == true)
                {
                    _rpgTalkSwitcher.TurnOnCurrentTurnOffOthers(lore.gameObject.GetComponent<RPGTalk>());
                    StartCoroutine(ChangeColor(fadeImg, Color.clear, Color.black, 0.5f));
                    _characterScript.SetInactive();
                }
                break;
        }
    }
    
    /// <summary>
    /// Загружает локацию раннера. Вынес в отдельную функцию, чтобы добавить в callback к rpgtalk-у на сцене с разговором мента
    /// /<summary>
    public void LoadRunner()
    {
        StartCoroutine(ChangeColor(fadeImg, Color.clear, Color.black, 0.5f));
    }

    /// <summary>
    /// Загружает последнюю локацию. Вынес в отдельную функцию, чтобы добавить в callback к rpgtalk-у на сцене с разговором сторожа
    /// /<summary>
    public void LoadFinal()
    {
        DataHolder.AddPionist();
        StartCoroutine(ChangeColor(fadeImg, Color.clear, Color.black, 0.5f));
    }

    private IEnumerator ChangeColor(Image image, Color from, Color to, float duration)
    {
        int volumeFrom = -80;
        int volumeTo = 0;
        if (from == Color.clear)
        {
            volumeFrom = 0;
            volumeTo = -80;
        }

        float timeElapsed = 0.0f;
        float t = 0.0f;
        while (t < 1.0f)
        {
            timeElapsed += Time.deltaTime;

            t = timeElapsed / duration;

            image.color = Color.Lerp(from, to, t);
            _audioMixer.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(volumeFrom, volumeTo, t));

            yield return null;
        }
        if (lore != null)
        {
            lore.Play();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("DK");
        }
    }

    public void WaitThenLoadScene(string sceneName)
    {
        StartCoroutine(WaitThenLoadScene(0.5f, sceneName));
    }

    private IEnumerator WaitThenLoadScene(float duration, string sceneName)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(sceneName);
    }
}
