using System;

public class OnUnlockSkinEventArgs : EventArgs {

    public int skinNr;

    public OnUnlockSkinEventArgs(int nr)
    {
        skinNr = nr;
    }
}
