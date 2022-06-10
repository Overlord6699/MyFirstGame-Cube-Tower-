using UnityEngine.Advertisements;
using UnityEngine;
using System.Collections;

public class Ads : MonoBehaviour
{
    private string GameID = "4202249", type = "video";
    private bool TestMode = true;

    private void Start()
    {
        Advertisement.Initialize(GameID, TestMode);
        StartCoroutine(ShowAd());
    }

    IEnumerator ShowAd()
    {
        while (true)
        {
            if (Advertisement.IsReady(type))
            {
                Debug.Log("Ready");
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
