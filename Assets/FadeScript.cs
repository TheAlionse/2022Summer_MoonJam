using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class FadeScript : MonoBehaviour
{
    public VisualElement fade_screen;
    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        fade_screen = root.Q<VisualElement>("fade_screen");
    }

    public IEnumerator changeAlpha(float alpha, float time)
    {
        StyleColor startColorA = fade_screen.style.backgroundColor;
        for (float t = 0f; t < time; t += Time.deltaTime)
        {
            float normalizedTime = t / time;
            float newA = Mathf.Lerp(startColorA.value.a, alpha, normalizedTime);
            fade_screen.style.backgroundColor = new StyleColor(new Color(fade_screen.style.backgroundColor.value.r, fade_screen.style.backgroundColor.value.g, fade_screen.style.backgroundColor.value.b, newA));
            yield return null;
        }
    }
}
