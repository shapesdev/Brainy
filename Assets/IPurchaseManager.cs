using System;

interface IPurchaseManager
{
    event EventHandler<OnPurchaseSuccesful> OnSuccess;
}