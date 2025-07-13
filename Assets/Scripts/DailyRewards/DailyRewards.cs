using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

namespace DailyRewardsSystem { }

public class DailyRewards : MonoBehaviour
{

    [Header("Reward UI")]
    [SerializeField] GameObject rewards_Panel;

    [SerializeField] Button openButton;
    [SerializeField] Button closeButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    
    void Initialize()
    {
        openButton.onClick.RemoveAllListeners();
        openButton.onClick.AddListener(OnOpenButtonClick);

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(OnOpenButtonClick);
    }


   public void OnOpenButtonClick()
    {
        rewards_Panel.SetActive(true);
    }
  public  void OnCloseButtonClick()
    {
        rewards_Panel.SetActive(false);
    }

}
