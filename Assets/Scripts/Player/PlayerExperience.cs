using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField] private List<PlayerLevel> _levels;

    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private RectTransform _expereinceStatusBar;
    [SerializeField] private PlayerHealth _health;

    private int _level = 1;

    private float _experienceCurrent;
    private float _experienceMax = 100f;

    private void Awake()
    {
        DrawUI();
    }

    private void Start()
    {
        SetLevel(_level);
    }

    public void AddExp(float value)
    {
        _experienceCurrent += value;

        if (_experienceCurrent >= _experienceMax)
        {
            SetLevel(++_level);
            _experienceCurrent = 0;
        }

        DrawUI();
    }

    private void SetLevel(int value)
    {
        _level = value;

        int curLevel = Mathf.Clamp(_level - 1, 0, _levels.Count - 1);
        _experienceMax = _levels[curLevel].expForNextLevel;
        GetComponent<FireballCaster>().damage = _levels[curLevel].fireballDamage;

        GrenadeCaster gCaster = GetComponent<GrenadeCaster>();
        gCaster.damage = _levels[curLevel].grenadeDamage;

        if (_levels[curLevel].grenadeDamage > 0)
            gCaster.enabled = true;
        else
            gCaster.enabled = false;

        _health.SetMaxValue(_levels[curLevel].maxHealth);
        _health.TakeDamage(-_health.MaxValue);
    }

    private void DrawUI()
    {
        _levelText.text = _level.ToString();
        _expereinceStatusBar.anchorMax = new(_experienceCurrent / _experienceMax, 1f);
    }
}
