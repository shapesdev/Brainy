using System;

public class OnSelectSkinEventARgs : EventArgs
{
    public int skinNr;

    public OnSelectSkinEventARgs(int nr)
    {
        skinNr = nr;
    }
}
