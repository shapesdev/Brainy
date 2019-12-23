using System.Collections;
using System.Collections.Generic;

public class ShopModel
{
    bool[] allSkins;
    public int mySkin;
    private List<ShopItem> shopItems = new List<ShopItem>();

    public void SetShopItems(List<ShopItem> items)
    {
        shopItems = items;
    }

    public List<ShopItem> GetShopItems()
    {
        return shopItems;
    }

    public void SetAllSkinsFromPlayfab(string skinsIn)
    {
        allSkins = new bool[skinsIn.Length];

        for (int i = 0; i < skinsIn.Length; i++)
        {
            if (int.Parse(skinsIn[i].ToString()) > 0)
            {
                allSkins[i] = true;
            }
            else
            {
                allSkins[i] = false;
            }
        }
    }

    public void SetAllSkins(int length)
    {
        allSkins = new bool[length];
        allSkins[0] = true;
    }

    public string GetAllSkinsToString()
    {
        string toString = "";

        for (int i = 0; i < allSkins.Length; i++)
        {
            if (allSkins[i] == true)
            {
                toString += "1";
            }
            else
            {
                toString += "0";
            }
        }
        return toString;
    }

    public bool[] GetAllSkins()
    {
        return allSkins;
    }

    public void SetSkinTrue(int number)
    {
        allSkins[number] = true;
    }

    public void SetMySkin(string mySkinIn)
    {
        int skin = int.Parse(mySkinIn.ToString());
        mySkin = skin;
        PlayerConfig.instance.currentSkin = mySkin;
    }

    public string GetMySkinToString()
    {
        return mySkin.ToString();
    }
}
