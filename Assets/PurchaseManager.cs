using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using PlayFab;
using PlayFab.ClientModels;

public class PurchaseManager : MonoBehaviour, IStoreListener {

    public static PurchaseManager Instance { get; set; }

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    List<ShopItem> shopItems = new List<ShopItem>();
    public ShopView shopView;
    public static string pack1 = "";
    public static string pack2 = "";
    public static string pack3 = "";

    int money;


    private void Awake()
    {
        Instance = this;
        LoadShopItems();
        pack1 = shopItems[0].itemId;
        pack2 = shopItems[1].itemId;
        pack3 = shopItems[2].itemId;

    }
    private void LoadShopItems()
    {
        ShopItem[] objects = Resources.LoadAll<ShopItem>("SO/Shop Items");

        foreach (ShopItem obj in objects)
        {
            shopItems.Add(obj);
        }

        shopItems = shopItems.OrderBy(x => x.amount).ToList();
    }

    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(pack1, ProductType.Consumable);
        builder.AddProduct(pack2, ProductType.Consumable);
        builder.AddProduct(pack3, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void BuyPack1()
    {
        BuyProductID(pack1);
    }
    public void BuyPack2()
    {
        BuyProductID(pack2);
    }
    public void BuyPack3()
    {
        BuyProductID(pack3);
    }


    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, pack1, StringComparison.Ordinal))
        {
            UpdateVC(shopItems[0].amount);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, pack2, StringComparison.Ordinal))
        {
            UpdateVC(shopItems[1].amount);
        }
        else if (string.Equals(args.purchasedProduct.definition.id, pack3, StringComparison.Ordinal))
        {
            UpdateVC(shopItems[2].amount);
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }
        return PurchaseProcessingResult.Complete;
    }

    private void UpdateVC(int amount)
    {
        money = amount;
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "AddCurrency",
            FunctionParameter = new { money = amount },
            GeneratePlayStreamEvent = true
        }, OnModifyVirtualCurrency, OnModifyVirtualCurrencyFailed);
    }

    private void OnModifyVirtualCurrencyFailed(PlayFabError obj)
    {

    }

    private void OnModifyVirtualCurrency(ExecuteCloudScriptResult result)
    {
        GetPlayerVC();
    }

    private void GetPlayerVC()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (GetUserInventoryResult result) =>
        {
            OnGetCurrency(result);
        }, OnGetCurrencyError);
    }

    private void OnGetCurrency(GetUserInventoryResult result)
    {
        var dmd = int.Parse(result.VirtualCurrency["DM"].ToString());
        PlayerPrefs.SetInt("Dmd", dmd);
        shopView.SuccessfulPurchase(money);
    }

    private void OnGetCurrencyError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        Debug.Log("COULDNT GET VC");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}
