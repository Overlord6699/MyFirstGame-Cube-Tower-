using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CunvusButtons : MonoBehaviour
{
    public Sprite MusicOn, MusicOff;

    public void RestartGame()
    {
        if(PlayerPrefs.GetString("Music") != "No")
        {
            GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Insta()
    {
        Application.OpenURL("https://www.instagram.com/magelich/");
        if (PlayerPrefs.GetString("Music") != "No")
        {
            GetComponent<AudioSource>().Play();
        }
    }

    private void Start()
    {
        if(PlayerPrefs.GetString("Music") == "No" && gameObject.name == "Music")
        {
            GetComponent<Image>().sprite = MusicOff;
        }
    }

    public void MusicWork()
    {
        if (PlayerPrefs.GetString("Music") == "No")
        {
            GetComponent<AudioSource>().Play();
            PlayerPrefs.SetString("Music", "Yes");
            GetComponent<Image>().sprite = MusicOn;
        }
        else
        {
            PlayerPrefs.SetString("Music", "No");
            GetComponent<Image>().sprite = MusicOff;
        }
    }

    public void LoadShop()
    {
        if (PlayerPrefs.GetString("Music") != "No")
        {
            GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene("Shop");
    }

    public void CloseShop()
    {
        if (PlayerPrefs.GetString("Music") != "No")
        {
            GetComponent<AudioSource>().Play();
        }
        SceneManager.LoadScene("Main");
    }
}
