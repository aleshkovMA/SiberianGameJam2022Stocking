using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Events;
using RPGTALK.Timeline;


public class RegistrationForm : MonoBehaviour
{
    private GameObject _regisrationForm;

    [SerializeField]
    private RPGTalk _rPGTalkColya;

    [SerializeField]
    private PlayableDirector _timelineAfterRegistration;

    [SerializeField]
    private Character _character;

    private UnityEvent _showBlackImg = new UnityEvent();

    private void Start()
    {
        _showBlackImg.AddListener(ShowBlackImg);
        _regisrationForm = gameObject;
        _regisrationForm.SetActive(false);
    }

    public void StartWaitingForInput()
    {
        _regisrationForm.SetActive(true);
    }
    public void StopWaitingForInput()
    {
        _regisrationForm.SetActive(false);
    }

    public void OnButtonClick()
    {
        _timelineAfterRegistration.Play();
        StopWaitingForInput();
    }

    void ShowBlackImg()
    {
        GameObject.Find("Black image").GetComponent<Bleacher>().BleachImageAscending("HeroHouse");
    }

}
