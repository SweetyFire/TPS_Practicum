using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class TextDuration
{
    public string text;
    public float duration;

    public TextDuration(string text, float duration)
    {
        this.text = text;
        this.duration = duration;
    }
}

public class PopupUI : MonoBehaviour
{
    [SerializeField] private RectTransform _window;
    [SerializeField] private Vector2 _firstWindowPosition;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private LeanTweenType _animType;
    [SerializeField] private float _animSpeed = 0.4f;

    private bool _animated;
    private Vector2 _animatedWindowPosition;
    private List<TextDuration> _textQueue = new();

    private void Awake()
    {
        _animatedWindowPosition = _window.anchoredPosition;
        _window.anchoredPosition = _firstWindowPosition;
    }

    public void AddTextToQueue(string text, float duration)
    {
        if (!_animated)
        {
            TryShowText(text, duration);
        }
        else
        {
            _textQueue.Add(new(text, duration));
        }
    }

    public bool TryShowText(string text, float duration)
    {
        if (_animated) return false;

        _textQueue.Add(new(text, duration));
        Animate(text, duration);
        return true;
    }

    private void Animate(string text, float duration)
    {
        _animated = true;
        _text.text = string.Empty;
        _window.gameObject.SetActive(_animated);

        LeanTween.move(_window, _animatedWindowPosition, _animSpeed).setEase(_animType).setOnComplete(() =>
        {
            StartCoroutine(WriteTextIE());
        });
    }

    private void AnimateToDefault()
    {
        LeanTween.move(_window, _firstWindowPosition, _animSpeed).setEase(_animType).setOnComplete(() =>
        {
            _animated = false;
            _window.gameObject.SetActive(_animated);
        });
    }

    private IEnumerator WriteTextIE()
    {
        while (_textQueue.Count > 0)
        {
            int charIndex = 0;
            float writeSpeed = CalculateWriteSpeed(_textQueue[0].text.Length);
            _text.text = string.Empty;

            TextDuration curText = _textQueue[0];

            while (charIndex < curText.text.Length)
            {
                if (curText.text[charIndex] == '<')
                {
                    string addText = string.Empty;
                    while (charIndex < curText.text.Length && curText.text[charIndex] != '>')
                    {
                        addText += curText.text[charIndex];
                        charIndex++;
                    }

                    if (curText.text[charIndex] == '>')
                    {
                        addText += curText.text[charIndex];
                        charIndex++;
                    }

                    _text.text += addText;
                    continue;
                }

                yield return new WaitForSeconds(writeSpeed);
                _text.text += curText.text[charIndex];
                charIndex++;
            }

            yield return new WaitForSeconds(curText.duration);

            while (_text.text.Length > 0)
            {
                if (_text.text[0] == '<')
                {
                    int startIndex = 0;
                    while (startIndex < _text.text.Length && _text.text[startIndex] != '>')
                    {
                        startIndex++;
                    }

                    if (_text.text[startIndex] == '>')
                    {
                        startIndex++;
                    }

                    _text.text = _text.text[startIndex..];
                    continue;
                }

                yield return new WaitForSeconds(writeSpeed);
                _text.text = _text.text[1..];
            }

            _textQueue.RemoveAt(0);
        }

        AnimateToDefault();
    }

    private float CalculateWriteSpeed(int textLength)
    {
        float baseSpeed = 0.1f;
        int maxTextLength = 55;

        float lengthPerc = 1f - Mathf.Clamp01((float)textLength / maxTextLength);
        return Mathf.Clamp(baseSpeed * lengthPerc, 0.02f, baseSpeed);
    }
}
