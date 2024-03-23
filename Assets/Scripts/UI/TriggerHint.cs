using System.Collections.Generic;
using UnityEngine;

public class TriggerHint : MonoBehaviour
{
    [SerializeField] private List<TextDuration> _texts = new();
    [SerializeField] private bool _infinity;

    private bool _used;

    private void OnTriggerEnter(Collider other)
    {
        if (_used || _infinity) return;
        if (!other.TryGetComponent(out PlayerController playerController)) return;

        Show(playerController.PopupUI);
    }

    private void Show(PopupUI popupUI)
    {
        foreach (TextDuration text in _texts)
        {
            popupUI.AddTextToQueue(text.text, text.duration);
        }

        _used = true;

        if (!_infinity)
            gameObject.SetActive(false);
    }
}
