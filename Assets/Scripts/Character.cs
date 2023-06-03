using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public Animator animator;
    public bool playable = true;
    public float speed;

    private Rigidbody2D _RB2D;
    private Transform _character;
    private bool _isFacingRight = false;

    [SerializeField]
    private List<AudioClip> _audioClipsForSteps;
    private AudioSource _audioSource;
    private int _currentStepIndex = 1;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _character = GetComponent<Transform>();
        _RB2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (playable)
        {
            float direction = Input.GetAxis("Horizontal");

            if (direction > 0 && !_isFacingRight)
            {
                _isFacingRight = true;
                Flip();

            }
            else if (direction < 0 && _isFacingRight)
            {
                _isFacingRight = false;
                Flip();
            }
            else if (direction == 0)
            {
                animator.Play("StockingIdle");
            }
            else
            {
                animator.Play("StockingWalk");
            }

            _RB2D.MovePosition(_RB2D.position + new Vector2(speed * direction, 0.0f));
        }
    }

    void Flip()
    {
        Vector3 theScale = _character.localScale;
        theScale.x *= -1;
        _character.localScale = theScale;
    }

    public void SetInactive()
    {
        playable = false;
    }

    public void SetActive()
    {
        playable = true;
    }

    public void PlayRandomStepSound()
    {
        if (_currentStepIndex == 1)
        {
            _audioSource.PlayOneShot(_audioClipsForSteps[Random.Range(0, 2)]);
            _currentStepIndex = 2;
        }
        else
        {
            _audioSource.PlayOneShot(_audioClipsForSteps[Random.Range(2, 4)]);
            _currentStepIndex = 1;
        }
    }
}
