using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoiseLevel : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Text infoField;
    [SerializeField] private GameObject levelIcon;
    [SerializeField] private Sprite startLevelSprite;
    [SerializeField] private Sprite activeLevelSprite;
    [SerializeField] private Sprite hoveredLevelSprite;

    private void Start()
    {
        if (levelIcon.name == DataHolder.lastLevel)
        {
            levelIcon.GetComponent<SpriteRenderer>().sprite = activeLevelSprite;
        }
    }
    private void OnMouseDown()
    {
        levelIcon.GetComponent<SpriteRenderer>().sprite = startLevelSprite;
        DataHolder.lastLevel = levelName;
        SceneManager.LoadScene(levelName);
    }
    private void OnMouseEnter()
    {
        canvas.SetActive(true);
        levelIcon.GetComponent<SpriteRenderer>().sprite = hoveredLevelSprite;
        switch (levelName)
        {
            case "HeroHouse":
                infoField.text = "����� ��������� ����� � ���������� ���� � ���";
                break;
            case "Bar":
                infoField.text = "��� ��� � ��� ���������� ����� ���� �����������";
                break;
            case "FriendHouse":
                infoField.text = "��� ��� �����";
                break;
        }
    }
    private void OnMouseExit()
    {
        canvas.SetActive(false);
    }
}
