using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ApplyResetNotification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private CanvasGroup notificationCanvasGroup;
    [SerializeField] private Color applyColor;
    [SerializeField] private Color resetColor;

    [SerializeField] private Button applyButton;
    [SerializeField] private Button resetButton;

    private const string appliedText = "Applied!";
    private const string resetedText = "Reseted!";
    private WaitForSeconds notificationFadeDelay = new WaitForSeconds(.5f);
    private float fadeTime = .25f;//sec

    private UnityAction applyAction;
    private UnityAction resetAction;

    private void Awake()
    {
        applyAction = new UnityAction(() =>
        {
            ButtonPressed(appliedText, applyColor);
        });
        resetAction = new UnityAction(() =>
        {
            ButtonPressed(resetedText, resetColor);
        });
        notificationCanvasGroup.alpha = 0;
    }

    private void OnEnable()
    {
        applyButton.onClick.AddListener(applyAction);
        resetButton.onClick.AddListener(resetAction);
    }

    private void OnDisable()
    {
        ResetNotification();
        applyButton.onClick.RemoveListener(applyAction);
        resetButton.onClick.RemoveListener(resetAction);
    }

    private void ButtonPressed(string text, Color color)
    {
        ResetNotification();
        notificationText.text = text;
        notificationText.color = color;
        StartCoroutine(FadeNotification());
    }

    private void ResetNotification()
    {
        StopCoroutine(FadeNotification());
        notificationCanvasGroup.alpha = 0;
    }

    IEnumerator FadeNotification()
    {
        notificationCanvasGroup.alpha = 1;

        yield return notificationFadeDelay;

        float percent = 1;
        float fadeSpeed = 1 / fadeTime;

        while (percent > 0)
        {
            percent -= Time.deltaTime * fadeSpeed;

            notificationCanvasGroup.alpha = percent;

            yield return null;
        }
    }
}
