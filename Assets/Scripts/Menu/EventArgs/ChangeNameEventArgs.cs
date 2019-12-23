using System;

public class ChangeNameEventArgs : EventArgs {

    public string name;

    public ChangeNameEventArgs(string name)
    {
        this.name = name;
    }
}
