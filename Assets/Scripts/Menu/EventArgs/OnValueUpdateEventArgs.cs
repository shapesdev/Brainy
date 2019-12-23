using System;

public class OnValueUpdateEventArgs : EventArgs {

    public int id;

    public OnValueUpdateEventArgs(int id)
    {
        this.id = id;
    }
}
