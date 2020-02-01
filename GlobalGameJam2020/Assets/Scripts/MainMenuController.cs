using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    
    [SerializeField] CanvasGroup m_mainCanvasGroup;
    [SerializeField] CanvasGroup m_creditsCanvasGroup;
    [SerializeField] float m_speedToChangeAlpha = 3;

    IEnumerator ChangeCanvasGroupAlpha(CanvasGroup canvas, float toAlpha, float delayToStart = 0)
    {
        yield return new WaitForSeconds(delayToStart);

        float fromAlpha = canvas.alpha;
        float fracJourney = 0;
        float distance = Mathf.Abs(fromAlpha - toAlpha);
        float actualAlpha = fromAlpha;

        while (actualAlpha != toAlpha)
        {
            fracJourney += (Time.deltaTime) * m_speedToChangeAlpha / distance;
            actualAlpha = Mathf.Lerp(fromAlpha, toAlpha, fracJourney);
            canvas.alpha = actualAlpha;
            yield return null;
        }
    }

    public void SwitchToMainCanvas()
    {
        StartCoroutine(ChangeCanvasGroupAlpha(m_mainCanvasGroup, 0));
        StartCoroutine(ChangeCanvasGroupAlpha(m_creditsCanvasGroup, 0, 1 / m_speedToChangeAlpha));
    }
    public void SwitchToCreditsCanvas()
    {
        StartCoroutine(ChangeCanvasGroupAlpha(m_creditsCanvasGroup, 0));
        StartCoroutine(ChangeCanvasGroupAlpha(m_mainCanvasGroup, 0, 1 / m_speedToChangeAlpha));
    }
    
}
