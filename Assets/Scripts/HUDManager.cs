using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance { get; private set; }

    [SerializeField] private Slider timeOutSlider;
    [SerializeField] private TextMeshProUGUI timeOutText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI resultText;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void UpdateTimeOutSlider(float time) {
        timeOutSlider.value = time;
    }

    public void UpdateTimeOutText(int time) {
        timeOutText.SetText(time.ToString());
    }

    public void ShowGameOverPanel(bool result) {
        resultText.SetText(result ? "YOU WIN!" : "YOU LOSE!");
        gameOverPanel.SetActive(true);
    }
}
