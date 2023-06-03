using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public delegate void DragEndedDelegate(Draggable draggableObject);

    public DragEndedDelegate dragEndedCallback;

    public bool isInteractable = true;

    private bool isDragged = false;
    private Vector3 mouseDragStartPosition;
    private Vector3 spriteDragStartPosition;
    [SerializeField] AudioClip _trashClip;
    private AudioSource _audioSource;

    private void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (isInteractable)
        {
            _audioSource.PlayOneShot(_trashClip);
            isDragged = true;
            mouseDragStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spriteDragStartPosition = transform.localPosition;
        }
    }
    private void OnMouseDrag()
    {
        if (isInteractable)
        {
            if(isDragged)
            {
                if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 36 
                && Camera.main.ScreenToWorldPoint(Input.mousePosition).x > -53 
                && Camera.main.ScreenToWorldPoint(Input.mousePosition).y < 9.6 
                && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > -6)
                {
                    transform.localPosition = spriteDragStartPosition + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseDragStartPosition);
                }

            }
        }
    }
    private void OnMouseUp()
    {
        if (isInteractable)
        {
            isDragged = false;
            dragEndedCallback(this);
        }
    }
}
