using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class ArcadeMovement : MonoBehaviour
{
    [SerializeField] private Image fadeImg;
    [SerializeField] private Text bottleScore;
    [SerializeField] private float speed;
    [SerializeField] private GameObject Bottle;
    [SerializeField] private List<GameObject> lives;
    [SerializeField] private List<AudioClip> _audioClipsForSteps;
    [SerializeField] private AudioSource hitAudioSource;
    [SerializeField] private List<AudioClip> _audioClipsForHits;
    [SerializeField] private PlayableDirector _winTimeline;
    [SerializeField] private PlayableDirector _loseTimeline;
    [SerializeField] private RPGTalkSwitcher _rpgTalkSwitcher;
    private AudioSource _audioSource;
    // Нужен для того, чтобы делать разные звуки шагов для каждой ноги
    private int _currentStepIndex = 1;
    private int currentLives = 4;
    public Animator animator;
    private Rigidbody2D _RB2D;
    private Transform _character;
    private int BottlesCount = 0;
    private bool _isLoseTextShowing = false;
    [SerializeField] private AudioMixerGroup _audioMixer;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 1;
        _character = GetComponent<Transform>();
        _RB2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        animator.Play("StockingWalk");
        float direction = Input.GetAxis("Vertical");
        _RB2D.MovePosition(_RB2D.position + new Vector2(speed * Time.fixedDeltaTime, speed * direction * Time.fixedDeltaTime));

    }

    private void Update()
    {
        if(Input.GetKeyDown("space") && BottlesCount>0)
        {
            Instantiate(Bottle, _character.position + new Vector3(0,2,0), Quaternion.identity);
            BottlesCount--;
            bottleScore.text = "Бутылки: " + BottlesCount;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BottleOnGround")
        {
            BottlesCount++;
            bottleScore.text = "Бутылки: " + BottlesCount;
            Destroy(other.gameObject);
        }
        else if(other.tag == "Finish")
        {
            if (!_isLoseTextShowing)
            {
                StartCoroutine(ChangeColorAndGoToScene(fadeImg, Color.clear, Color.black, 0.5f, "StorojDialog", true));
                // Отключаем звуки шагов, а то персонаж-то продолжает идти
                _audioSource.volume = 0;
            }
        }
        else if(other.tag == "Target" && currentLives>1)
        {
            hitAudioSource.PlayOneShot(_audioClipsForHits[Random.Range(0, _audioClipsForHits.Count - 1)]);
            foreach(GameObject l in lives)
            {
                if (l.activeSelf == true)
                {
                    l.SetActive(false);
                    currentLives--;
                    break;
                }
            }
        }
        else if (other.tag == "Target" && currentLives <= 1 && !_isLoseTextShowing)
        {
            hitAudioSource.PlayOneShot(_audioClipsForHits[Random.Range(0, _audioClipsForHits.Count - 1)]);
            StartCoroutine(ChangeColorAndGoToScene(fadeImg, Color.clear, Color.black, 0.5f, "ArcadeRunner", false));
            _audioSource.volume = 0;
            _isLoseTextShowing = true;
        }
    }
    

    private IEnumerator ChangeColorAndGoToScene(Image image, Color from, Color to, float duration, string levelToLoad, bool levelComplite)
    {
        float timeElapsed = 0.0f;

        float t = 0.0f;
        while (t < 1.0f)
        {
            timeElapsed += Time.deltaTime;

            t = timeElapsed / duration;

            image.color = Color.Lerp(from, to, t);

            yield return null;
        }
        if(levelComplite ==true)
        {
            _rpgTalkSwitcher.TurnOnCurrentTurnOffOthers(_winTimeline.gameObject.GetComponent<RPGTalk>());
            _winTimeline.Play();
        }
        else
        {
            _rpgTalkSwitcher.TurnOnCurrentTurnOffOthers(_loseTimeline.gameObject.GetComponent<RPGTalk>());
            _loseTimeline.Play();
        }

        timeElapsed = 0.0f;
        t = 0.0f;
        while (t < 1.0f)
        {
            timeElapsed += Time.deltaTime;

            t = timeElapsed / 3f;

            _audioMixer.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(0, -80, t));

            yield return null;
        }

        _isLoseTextShowing = false;
        SceneManager.LoadScene(levelToLoad);
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
