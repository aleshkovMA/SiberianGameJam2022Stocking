using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Bleacher : MonoBehaviour
{
    private Image _imgToBleach;
    [SerializeField] private bool _isBleachOnStart;
    [SerializeField] private StatsUIController _statsUIController;
    [SerializeField] private AudioMixerGroup _audioMixer;

    void Start()
    {
        _imgToBleach = gameObject.GetComponent<Image>();
        if (_isBleachOnStart)
        {
            BleachImageDescending();
        }
    }

    public void BleachImageAscending(string levelName)
    {
        StartCoroutine(ChangeColor(_imgToBleach, Color.clear, Color.black, 1f, true, levelName));
       
    }

    public void OnlyBleachMusic()
    {
        StartCoroutine(ChangeColor(_imgToBleach, _imgToBleach.color, _imgToBleach.color, 1f, false, ""));
    }

    public void BleachImageDescending()
    {
       StartCoroutine(ChangeColor(_imgToBleach, Color.black, Color.clear, 1f, false, null));
    }

    private IEnumerator ChangeColor(Image image, Color from, Color to, float duration, bool needToChangeLevel, string levelName)
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
        if (needToChangeLevel==true)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}
