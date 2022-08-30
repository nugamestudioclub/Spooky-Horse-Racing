using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private Image background;

    [SerializeField]
    private float startingTimer;
    private float currentTimer;

    public bool IsRunning { get; private set; }

    public void BeginTimer()
    {
        IsRunning = true;
    }

    public void UpdateTimer()
    {
        currentTimer = Mathf.Max(currentTimer - Time.deltaTime, 0f);
        timerText.text = ((int)currentTimer).ToString();
        background.fillAmount = currentTimer / startingTimer;
        if (currentTimer <= 0)
        {
            print("Timer done");
            //Transition to game
        }
    }

    public void ResetTimer()
    {
        IsRunning = false;
        currentTimer = startingTimer;
        background.fillAmount = 1;
        timerText.text = ((int)currentTimer).ToString();
    }

}
