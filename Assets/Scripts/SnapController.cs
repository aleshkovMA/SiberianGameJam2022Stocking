using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapController : MonoBehaviour
{
    [SerializeField] Transform snapPoint;
    [SerializeField] List<Draggable> draggableObjects;
    [SerializeField] Text trashCounter;
    private float snapRange = 5.5f;
    private int itemsCollected = 0;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        foreach(Draggable draggable in draggableObjects)
        {
            draggable.dragEndedCallback = OnDragEnded;
        }
    }

    private void OnDragEnded(Draggable draggable)
    {
        float currentDistance = Vector2.Distance(draggable.transform.localPosition, snapPoint.localPosition);

        
        if(currentDistance <= snapRange && DataHolder.CanActive == true)
        {
            _audioSource.Play();
            draggable.gameObject.SetActive(false);
            itemsCollected++;
            trashCounter.text = $"Вещи в ящике {itemsCollected}/5";
            if(itemsCollected==5)
            {
                DataHolder.wasRoomCleaned = true;
            }
        }
    }

    public void ActivateTrashCounter()
    {
        trashCounter.enabled = true;
    }

    public void DeactivateTrashCounter()
    {
        trashCounter.enabled = false;
    }

    public void SetAllDraggablesInteractable()
    {
        foreach(Draggable draggable in draggableObjects)
        {
            draggable.isInteractable = true;
        }
    }

    public void SetAllDraggablesAninteractable()
    {
        foreach(Draggable draggable in draggableObjects)
        {
            draggable.isInteractable = false;
        }
    }
    
}
