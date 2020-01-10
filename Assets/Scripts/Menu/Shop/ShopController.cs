using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController {

    public ShopModel model;
    public ShopView view;

    int skinToUpdate;
    
    public ShopController(ShopModel model, ShopView view, List<ShopItem> shopItems)
    {
        this.model = model;
        this.view = view;
        model.SetShopItems(shopItems);

        GetSkinData(PlayerConfig.instance.playerId);
        view.OnUnlock += HandleOnUnlockSkin;
        view.OnSelect += OnSelectSkin;
        MenuUIManager.allActions["Shop"] += HandleOnShop;
    }

    private void OnSelectSkin(object sender, OnSelectSkinEventARgs e)
    {
        model.SetMySkin(e.skinNr.ToString());
        SetMySkinData(model.GetMySkinToString());
    }

    private void HandleOnShop(bool obj)
    {
        if (obj == true)
        {
            view.OpenShop(model.GetAllSkins(), model.mySkin);
            view.ShowShopItems(model.GetShopItems());
        }
        else
        {
            view.CloseShop();
        }
    }

    private void HandleOnUnlockSkin(object sender, OnUnlockSkinEventArgs e)
    {
        skinToUpdate = e.skinNr;

        if(e.skinNr < model.GetAllSkins().Length / 2)
        {
            UpdateVC();
        }
        else if(e.skinNr > model.GetAllSkins().Length / 2 - 1)
        {
            UpdateVC2();
        }
    }

    private void UpdateVC()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdateCurrency",
            FunctionParameter = new { Testing = "im a test" },
            GeneratePlayStreamEvent = true
        }, OnModifyVirtualCurrency, OnModifyVirtualCurrencyFailed );
    }

    private void UpdateVC2()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdateCurrency2",
            FunctionParameter = new { Testing = "im a test" },
            GeneratePlayStreamEvent = true
        }, OnModifyVirtualCurrency, OnModifyVirtualCurrencyFailed);
    }

    private void OnModifyVirtualCurrencyFailed(PlayFabError obj)
    {
        //Debug.Log("Modification failed");
        //Debug.Log(obj.ErrorDetails);
    }

    private void OnModifyVirtualCurrency(ExecuteCloudScriptResult result)
    {
        GetPlayerVC();
        model.SetSkinTrue(skinToUpdate);
        view.UpdateSkin(skinToUpdate);
        SetSkinsData(model.GetAllSkinsToString());
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
        view.UpdateBalance();
    }

    private void OnGetCurrencyError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        Debug.Log("COULDNT GET VC");
    }

    private void GetSkinData(string id)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = id,
            Keys = null
        }, OnGetSkinData, GetSkinDataError);

    }

    private void GetSkinDataError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        //Debug.Log("COULDNT GET SKIN DATA");
    }

    private void OnGetSkinData(GetUserDataResult result)
    {
        if (result.Data == null || !result.Data.ContainsKey("Skins") && !result.Data.ContainsKey("MySkin"))
        {
            //Debug.Log("Skins not set");
            model.SetMySkin("0");
            model.SetAllSkins(view.ReturnSkinsLength());

            SetMySkinData(model.GetMySkinToString());
        }
        else
        {
            model.SetAllSkinsFromPlayfab(result.Data["Skins"].Value);
            model.SetMySkin(result.Data["MySkin"].Value);
        }
    }

    private void SetSkinsData(string skinsData)
    {
        Debug.Log(skinsData);
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"Skins", skinsData}
                }
        }, SetDataSuccess, SetSkinsError);
    }

    private void SetMySkinData(string mySkinData)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"MySkin", mySkinData}
                }
        }, SetMySkinSuccess, SetMySkinError);
    }

    private void SetDataSuccess(UpdateUserDataResult result)
    {
        Debug.Log(result.DataVersion);
        //Debug.Log("SET DATA SUCCESS");
    }

    private void SetMySkinSuccess(UpdateUserDataResult result)
    {
        SetSkinsData(model.GetAllSkinsToString());
    }

    private void SetMySkinError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        //Debug.Log("COULDNT SET MY SKIN");
    }

    private void SetSkinsError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        //Debug.Log("COULDNT SET SKINS");
    }
}
