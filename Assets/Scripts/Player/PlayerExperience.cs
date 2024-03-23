using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField] private List<PlayerLevel> _levels;

    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private RectTransform _expereinceStatusBar;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private PopupUI _popupUI;

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

        int curLevelIndex = Mathf.Clamp(_level - 1, 0, _levels.Count - 1);
        _experienceMax = _levels[curLevelIndex].expForNextLevel;
        GetComponent<FireballCaster>().damage = _levels[curLevelIndex].fireballDamage;

        GrenadeCaster gCaster = GetComponent<GrenadeCaster>();
        gCaster.damage = _levels[curLevelIndex].grenadeDamage;

        DrawPopup(curLevelIndex);

        if (_levels[curLevelIndex].grenadeDamage > 0)
            gCaster.enabled = true;
        else
            gCaster.enabled = false;

        _health.SetMaxValue(_levels[curLevelIndex].maxHealth);
        _health.TakeDamage(-_health.MaxValue);

        PlayerController playerController = _health.GetComponent<PlayerController>();
        playerController.runSpeed = _levels[curLevelIndex].runSpeed;
    }

    private void DrawUI()
    {
        _levelText.text = _level.ToString();
        _expereinceStatusBar.anchorMax = new(_experienceCurrent / _experienceMax, 1f);
    }

    private void DrawPopup(int levelIndex)
    {
        if (levelIndex <= 0) return;

        //Sprint level
        if (_levels[levelIndex - 1].runSpeed <= 0 && _levels[levelIndex].runSpeed > 0)
        {
            _popupUI.AddTextToQueue("Доступен бег на Shift", 4f);
        }

        // Grenade Level
        if (_levels[levelIndex - 1].grenadeDamage <= 0 && _levels[levelIndex].grenadeDamage > 0)
        {
            _popupUI.AddTextToQueue("Теперь ты можешь использовать гранаты на ПКМ", 4f);
        }
    }
}
