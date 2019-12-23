using System;
interface IShopView
{
    event EventHandler<OnUnlockSkinEventArgs> OnUnlock;
    event EventHandler<OnSelectSkinEventARgs> OnSelect;
}