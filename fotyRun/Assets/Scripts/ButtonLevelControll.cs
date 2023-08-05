using UnityEngine;
using UnityEngine.UI;

public class ButtonLevelControll : MonoBehaviour
{
    public int LevelNumber;
    public GameObject CloseImage;
    private Button button;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("LevelData") >= LevelNumber)
        {
            CloseImage.SetActive(false);
            button.interactable = true;
        }else
            button.interactable = false;
    }
}
