using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownUI : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownDuration = 3f;
    //public AudioSource countdownBeep;
    //public AudioSource countdownFinalBeep;

    public IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            //if (countdownBeep != null) countdownBeep.Play();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "START!";
        //if (countdownFinalBeep != null) countdownFinalBeep.Play();

        yield return new WaitForSeconds(0.5f);
        countdownText.gameObject.SetActive(false);
    }
}