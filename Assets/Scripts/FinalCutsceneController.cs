using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Маленький скрипт, который только контроллирует последнюю сцену - загружает либо полный, либо пустой зал
/// и запускает разные timeline для хорошей и плохой концовки 
/// /<summary>
public class FinalCutsceneController : MonoBehaviour
{
    /// <summary>
    /// Timeline хорошей концовки
    /// /<summary>
    [SerializeField] private PlayableDirector _goodEnding;

    /// <summary>
    /// Timeline плохой концовки
    /// /<summary>
    [SerializeField] private PlayableDirector _badEnding;

    /// <summary>
    /// Компонент spriteRenderer у заднего фона сцены (будем менять фон)
    /// /<summary>
    [SerializeField] private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Массив спрайтов, на которые мы будем менять спрайт заднего фона
    /// /<summary>
    [SerializeField] private Sprite[] _sprites = new Sprite[2];
    [SerializeField] private GameObject _wifeRock;
    [SerializeField] private AudioSource _music;

    void Start()
    {
        if (DataHolder.watchers >= 20)
        {
            _music.Play();
            // Меняем спрайт
            _spriteRenderer.sprite = _sprites[0];
            // Отключаем другой объект Timeline
            _badEnding.gameObject.SetActive(false);
            // Запускаем наш timeline
            _goodEnding.Play();
            if (DataHolder.watchers == 30)
            {
                Instantiate(_wifeRock, new Vector3(-0.04f, -0.54f, 0), Quaternion.identity);
            }
        }
        else
        {
            // аналогично
            _spriteRenderer.sprite = _sprites[1];
            _goodEnding.gameObject.SetActive(false);
            _badEnding.Play();
        }
    }
}
