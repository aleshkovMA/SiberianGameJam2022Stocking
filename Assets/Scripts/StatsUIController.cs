using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Загружает информацию в счётчики на GUI на основании данных в DataHolder
/// /<summary>
public class StatsUIController : MonoBehaviour
{
    /// <summary>
    /// UI-текст с количеством зрителей
    /// /<summary>
    [SerializeField] private Text _watchersCount;
    /// <summary>
    /// UI-текст с участниками группы
    /// /<summary>
    [SerializeField] private Text _bandCount;
    /// <summary>
    /// UI-текст с участниками группы
    /// /<summary>
    [SerializeField] private Image _watchersImage;

    /// <summary>
    /// Загружает информацию в счётчики на GUI
    /// /<summary>
    public void LoadCurrent()
    {
        _watchersCount.text = DataHolder.watchers.ToString();
        _bandCount.text = "Участники группы: " + DataHolder.bandCount.ToString();
    }

    public void ShowStats()
    {
        _watchersCount.enabled = true;
        _watchersImage.enabled = true;
        _bandCount.enabled = true;
    }

    void Start()
    {
        LoadCurrent();
    }
}