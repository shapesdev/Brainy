using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour, IShopView {

    public Image[] skins;
    public GameObject[] buttonLocks;
    public Button[] unlockedButtons;
    public Collider2D col;
    public Image[] pages;
    public Text[] itemPrices;
    public Text[] itemAmount;
    public Image currentSkinImage;
    public Image notificationNotEnoughDiamonds;
    public Text balanceText;
    public Text addedDmdText;
    public GameObject purchaseMadePanel;
    public GameObject confirmPanel;

    public GameObject loadingPurchase;

    public Image skinDisplay;

    int pageNr;
    int lastSkin;
    int selectedSkin;

    int skinToBuy;

    public Sprite currentPage;
    public Sprite notCurrentPage;

    public event EventHandler<OnUnlockSkinEventArgs> OnUnlock = (sender, e) => { };
    public event EventHandler<OnSelectSkinEventARgs> OnSelect = (sender, e) => { };

    void OnEnable()
    {
        UpdateBalance();

        for (int i = 0; i < skins.Length; i++)
        {
            if(skins[i].gameObject.activeInHierarchy)
            {
                lastSkin = i;
            }
            skins[i].sprite = PlayerConfig.instance.skins[i];
        }
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
    }

    void OnDisable()
    {
        SwipeDetector.OnSwipe -= SwipeDetector_OnSwipe;
    }

    public void UpdateBalance()
    {
        balanceText.text = "Your balance: " + PlayerPrefs.GetInt("Dmd");
        loadingPurchase.SetActive(false);
    }

    public void SuccessfulPurchase(int amount)
    {
        loadingPurchase.SetActive(false);
        addedDmdText.text = "Added " + amount + " diamonds to your balance!";
        purchaseMadePanel.SetActive(true);
        UpdateBalance();
        Invoke("TurnOffPurchasePanel", 2f);
    }

    private void TurnOffPurchasePanel()
    {
        purchaseMadePanel.SetActive(false);
    }

    public void ShowShopItems(List<ShopItem> items)
    {
        if (items.Count == itemAmount.Length && items.Count == itemPrices.Length)
        {
            for (int i = 0; i < items.Count; i++)
            {
                itemPrices[i].text = items[i].itemPrice.ToString() + "$";
                itemAmount[i].text = items[i].amount.ToString();
            }
        }
    }

    public int ReturnSkinsLength()
    {
        return skins.Length;
    }

    private void SwipeDetector_OnSwipe(SwipeData obj)
    {
        if(obj.Direction == SwipeDirection.Right && col.bounds.Contains(obj.StartPosition) && col.bounds.Contains(obj.EndPosition))
        {
            PreviousSkins();
        }
        else if(obj.Direction == SwipeDirection.Left && col.bounds.Contains(obj.StartPosition) && col.bounds.Contains(obj.EndPosition))
        {
            NextSkins();
        }
    }

    public void NextSkins()
    {
        if(pageNr + 1 < pages.Length)
        {
            pages[pageNr].sprite = notCurrentPage;
            pageNr += 1;
            pages[pageNr].sprite = currentPage;
        }

        if (lastSkin + 3 < skins.Length)
        {
            skins[lastSkin].gameObject.SetActive(false);
            skins[lastSkin - 1].gameObject.SetActive(false);
            skins[lastSkin - 2].gameObject.SetActive(false);

            var n = lastSkin + 1;

            for(int i = n; i <= n + 2; i++)
            {
                skins[i].gameObject.SetActive(true);
                lastSkin = i;
            }
        }
        UpdateSelectedImage();
    }

    public void PreviousSkins()
    {
        if (pageNr - 1 >= 0)
        {
            pages[pageNr].sprite = notCurrentPage;
            pageNr -= 1;
            pages[pageNr].sprite = currentPage;
        }

        if (lastSkin - 3 >= 0)
        {
            skins[lastSkin].gameObject.SetActive(false);
            skins[lastSkin - 1].gameObject.SetActive(false);
            skins[lastSkin - 2].gameObject.SetActive(false);

            lastSkin = lastSkin - 3;

            for (int i = lastSkin; i >= lastSkin - 2; i--)
            {
                skins[i].gameObject.SetActive(true);
            }
        }
        UpdateSelectedImage();
    }

    private void UpdateSelectedImage()
    {
        if (skins[selectedSkin].gameObject.activeInHierarchy == true)
        {
            if (currentSkinImage.gameObject.activeInHierarchy == false)
            {
                currentSkinImage.gameObject.SetActive(true);
            }
            currentSkinImage.transform.localPosition = new Vector3(skins[selectedSkin].transform.localPosition.x, currentSkinImage.transform.localPosition.y,
                currentSkinImage.transform.localPosition.z);
        }
        else if (skins[selectedSkin].gameObject.activeInHierarchy == false)
        {
            currentSkinImage.gameObject.SetActive(false);
        }
    }

    public void CloseShop()
    {
        this.gameObject.SetActive(false);
    }

    public void OpenShop(bool[] allSkins, int skinid)
    {
        selectedSkin = skinid;

        for (int i = 0; i < allSkins.Length; i++)
        {
            buttonLocks[i].SetActive(!allSkins[i]);
            unlockedButtons[i].interactable = allSkins[i];
        }
        this.gameObject.SetActive(true);

        if (skins[skinid].gameObject.activeInHierarchy == true)
        {
            currentSkinImage.gameObject.SetActive(true);
            currentSkinImage.transform.localPosition = new Vector3(skins[skinid].transform.localPosition.x, currentSkinImage.transform.localPosition.y,
                currentSkinImage.transform.localPosition.z);
        }
    }

    public void UnlockSkin(int skinId)
    {
        if(PlayerPrefs.GetInt("Dmd") != 0)
        {
            confirmPanel.SetActive(true);
            skinDisplay.sprite = skins[skinId].sprite;
            skinToBuy = skinId;
        }
        else
        {
            notificationNotEnoughDiamonds.gameObject.SetActive(true);
            Invoke("CloseNotification", 1.5f);
        }
    }

    public void BuySkin()
    {
        if (PlayerPrefs.GetInt("Dmd") >= 50)
        {
            loadingPurchase.SetActive(true);
            confirmPanel.SetActive(false);
            var eventArgs = new OnUnlockSkinEventArgs(skinToBuy);
            OnUnlock(this, eventArgs);
        }
        else
        {
            confirmPanel.SetActive(false);
            notificationNotEnoughDiamonds.gameObject.SetActive(true);
            Invoke("CloseNotification", 2f);
        }
    }

    public void ClosePurchase()
    {
        confirmPanel.SetActive(false);
    }

    private void CloseNotification()
    {
        notificationNotEnoughDiamonds.gameObject.SetActive(false);
    }

    public void UpdateSkin(int skinId)
    {
        buttonLocks[skinId].SetActive(false);
        unlockedButtons[skinId].interactable = true;
    }

    public void SelectSkin(int skinId)
    {
        selectedSkin = skinId;
        if(currentSkinImage.gameObject.activeInHierarchy == false)
        {
            currentSkinImage.gameObject.SetActive(true);
        }
        currentSkinImage.transform.localPosition = new Vector3(skins[skinId].transform.localPosition.x, currentSkinImage.transform.localPosition.y,
            currentSkinImage.transform.localPosition.z);
        var eventArgs = new OnSelectSkinEventARgs(skinId);
        OnSelect(this, eventArgs);
    }

    public void PrintTxt(string str)
    {
        Debug.Log(str);
    }
}
