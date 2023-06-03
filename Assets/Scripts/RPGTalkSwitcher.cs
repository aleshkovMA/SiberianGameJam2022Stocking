using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Синхронизирует разные компоненты RPGTalk, чтобы они не могли одновременно быть включенными
/// /<summary>
public class RPGTalkSwitcher : MonoBehaviour
{
    /// <summary>
    /// Список всех RPGTalk-ов на сцене
    /// /<summary>
    public List<RPGTalk> rpgTalks;

    /// <summary>
    /// Индекс текущего RPGTalk в списке
    /// /<summary>
    private int _currentIndexOfRPGTalk = 0;

    /// <summary>
    /// Включает компонент RPGTalk у текущего объекта, и выключает у всех остальных. Необходимо потому, что RPGTalk не поддверживает сразу несколько активных компонентов в одной сцене.
    /// </summary>
    public void TurnOnCurrentTurnOffOthers(RPGTalk currentRPGTalk)
    {
        foreach (RPGTalk rpgTalk in rpgTalks)
        {
            if (rpgTalk != currentRPGTalk)
            {
                rpgTalk.enabled = false;
            }
            else
            {
                rpgTalk.enabled = true;
            }
        }
    }

    /// <summary>
    /// Включает все RPGTalk компоненты. Запускается после окончания диалогов в компоненте RPGTalk.
    /// </summary>
    public void TurnOnAll()
    {
        foreach (RPGTalk rpgTalk in rpgTalks)
        {
            rpgTalk.enabled = true;
        }
    }

    /// <summary>
    /// Выключает компонент RPGTalk у текущего объекта в списке rpgTalks, и включает у следующего.
    /// </summary>
    public void TurnOffCurrentTurnOnOther()
    {
        rpgTalks[_currentIndexOfRPGTalk].enabled = false;
        _currentIndexOfRPGTalk++;
        rpgTalks[_currentIndexOfRPGTalk].enabled = true;
    }
}
