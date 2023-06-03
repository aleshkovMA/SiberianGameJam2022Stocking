using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Обрабатывает соприкосновения коллайдера чулка с триггерами, и, в зависимости от тэга триггера, вызывает действия
/// </summary>
public class CollidingManager : MonoBehaviour
{
    /// <summary>
    /// Скрипт Character. Через него запрещают/разрешают двигаться персонажу.
    /// </summary>
    [SerializeField]
    private Character _characterScript;

    /// <summary>
    /// Скрипт RPGTalkSwitcher. Он синхронизирует несколько находящихся на сцене RPGTalk-компонентов.
    /// </summary>
    /* Дело в том, что RPGTalk очень волшебная штука, и он не позволяет того, чтобы на сцене во время активного диалога находились
    ещё активные компоненты RPGTalk. Поэтому, для того, чтобы решить это, я сделал скрипт, который перед началом диалога отключается все (кроме текущего)
    RPGTalk-и, а после завершения диалога - включает обратно*/
    [SerializeField]
    private RPGTalkSwitcher _rpgTalkSwitcher;

    /// <summary>
    /// Строка, содержащая тэг текущего колладируещего объекта.
    /// </summary>
    private string _currentTag;

    /// <summary>
    /// GameObject текущего колладируещего объекта.
    /// </summary>
    private GameObject _colidingObject;

    /// <summary>
    /// Текущий RPGTalk (из колладирующего объекта)
    /// </summary>
    private RPGTalk _currentRPGTalk;

    /// <summary>
    /// Булевая переменная, которая нужна для того, чтобы нельзя было много раз активировать диалог.
    /// </summary>
    private bool _isDialogPlaying = false;

    /// <summary>
    /// Скрипт StatsUIController, который обновляет данные на UI в соответствии с хранимыми в DataHolder значениями.
    /// </summary>
    [SerializeField]
    public StatsUIController _statsUIController;


    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    public SnapController _snapController;

    /// <summary>
    /// Обработка нажатия на E на триггере и соответствущие действия
    /// </summary>

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_isDialogPlaying)
        {
            // Отключаем персонажа, чтобы нельзя было ходить во время диалогов
            DialogStarted();
            _characterScript.SetInactive();
            switch (_currentTag)
            {
                // Замечу, что сейчас мы не можем разговаривать ни с одним игровым персонажем дважды (после завершения первого RPGTalk)
                case "Friend":
                    {
                        _snapController.SetAllDraggablesAninteractable();
                        _rpgTalkSwitcher.TurnOnCurrentTurnOffOthers(_currentRPGTalk);
                        if (!DataHolder.wasFirstFriendDialog)
                        {
                            _colidingObject.GetComponent<DialogManager>().PlayDialog("First");
                            DataHolder.wasFirstFriendDialog = true;
                        }
                        else if (!DataHolder.wasWifeSatisfacted)
                        {
                            _colidingObject.GetComponent<DialogManager>().PlayDialog("Not ready");
                        }
                        else
                        {
                            _colidingObject.GetComponent<DialogManager>().PlayDialog("Second");
                            if (!DataHolder.gitaristAdded)
                            {
                                DataHolder.AddGuitarist();
                                _currentRPGTalk.callback.AddListener(_statsUIController.LoadCurrent);
                                _currentRPGTalk.callback.AddListener(_colidingObject.GetComponent<AudioSource>().Play);
                            }
                            else
                            {
                                _currentRPGTalk.callback.RemoveListener(_colidingObject.GetComponent<AudioSource>().Play);
                            }
                        } 
                        break;
                    }
                case "Wife":
                    {
                        _snapController.SetAllDraggablesAninteractable();
                        // Мы не можем поговорить с женой, пока не поговорили с другом
                        if (DataHolder.wasFirstFriendDialog)
                        {
                            DialogManager dialogManager = _colidingObject.GetComponent<DialogManager>();
                            _rpgTalkSwitcher.TurnOnCurrentTurnOffOthers(_currentRPGTalk);
                            switch (dialogManager._currentDialogIndex)
                            {
                                case 0:
                                {
                                    dialogManager.PlayDialog();
                                    break;
                                }
                                case 1:
                                {
                                    dialogManager.PlayDialog();
                                    break;
                                }
                                case 2:
                                {
                                    if (!DataHolder.wasRoomCleaned)
                                    {
                                        dialogManager.PlayDialog();
                                        // Здесь делаем ящик, куда складывать вещи, активным
                                        if (!DataHolder.CanActive)
                                        {
                                            DataHolder.CanActive = true;
                                            _currentRPGTalk.callback.AddListener(_snapController.ActivateTrashCounter);
                                        }
                                        dialogManager._currentDialogIndex = 2;
                                    }
                                    else
                                    {
                                        _currentRPGTalk.callback.RemoveListener(_snapController.ActivateTrashCounter);
                                        _snapController.DeactivateTrashCounter();
                                        DataHolder.wasWifeSatisfacted = true;
                                        dialogManager._currentDialogIndex ++;
                                        dialogManager.PlayDialog();
                                    }
                                    break;
                                }
                                case 3:
                                {
                                    dialogManager.PlayDialog();
                                    break;
                                }
                                case 4:
                                {
                                    if (!DataHolder.wasFinalWifeDialog)
                                    { 
                                        DataHolder.wasFinalWifeDialog = true;
                                        DataHolder.AddWatchers();
                                        _currentRPGTalk.callback.AddListener(_statsUIController.LoadCurrent);
                                        _currentRPGTalk.callback.AddListener(_colidingObject.GetComponent<AudioSource>().Play);
                                        dialogManager.PlayDialog();
                                    }
                                    else
                                    {
                                        // С женой не можем снова поговорить, просто ничего не делаем
                                        DialogEnded();
                                        _characterScript.SetActive();
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            goto default;
                        }
                        break;
                    }
                case "Sanich":
                {
                    _rpgTalkSwitcher.TurnOnCurrentTurnOffOthers(_currentRPGTalk);
                    // Если не было диалога  с санычем - запускаем его. Иначе - ничего не делаем
                    if (!DataHolder.wasSanichDialog)
                    {
                        _colidingObject.GetComponent<DialogManager>().PlayDialog();
                        DataHolder.wasSanichDialog = true;
                    }
                    else
                    {
                        goto default;
                    }
                    break;
                }
                case "Barman":
                {
                    _rpgTalkSwitcher.TurnOnCurrentTurnOffOthers(_currentRPGTalk);
                    // Аналогично санычу
                    if (!DataHolder.wasBarmanDialog)
                    {
                        _colidingObject.GetComponent<DialogManager>().PlayDialog();
                        DataHolder.AddBarabanshik();
                        _currentRPGTalk.callback.AddListener(_statsUIController.LoadCurrent);
                        DataHolder.wasBarmanDialog = true;
                    }
                    else
                    {
                        goto default;
                    }
                    break;
                }
                case "Normal workers":
                {
                    _rpgTalkSwitcher.TurnOnCurrentTurnOffOthers(_currentRPGTalk);
                    // Аналогично бармену
                    if (!DataHolder.wasWorkersDialog)
                    {
                        _colidingObject.GetComponent<DialogManager>().PlayDialog();
                        DataHolder.wasWorkersDialog = true;
                    }
                    else
                    {
                        goto default;
                    }
                    break;
                }
                default:
                {
                    DialogEnded();
                    _characterScript.SetActive();
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Показывает, что диалог завершился, и можно снова считывать нажатие E
    /// </summary>
    public void DialogEnded()
    {
        _isDialogPlaying = false;
    }

    /// <summary>
    /// Показывает, что диалог начался, и нажатие E не обрабатывается
    /// </summary>
    public void DialogStarted()
    {
        _isDialogPlaying = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _colidingObject = col.gameObject;
        _currentTag = col.tag;
        _currentRPGTalk = _colidingObject.GetComponent<RPGTalk>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        // То есть, если мы стоим на триггере, но почему-то диалог не начался, то сохраняем значения колладирующего объекта, а в update уже 
        // начнётся диалог
        if (_isDialogPlaying == false)       
        {
            _colidingObject = col.gameObject;
            _currentTag = col.tag;
            _currentRPGTalk = _colidingObject.GetComponent<RPGTalk>();
        }
    }

    /// <summary>
    /// Завершаем диалог и обнуляем тэг после выхода из коллайдера-триггера
    /// </summary>
    void OnTriggerExit2D(Collider2D col)
    {
        DialogEnded();
        _currentTag = "None";
    }
}
