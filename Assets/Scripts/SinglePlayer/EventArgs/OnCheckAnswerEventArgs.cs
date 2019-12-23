using System;

public class OnCheckAnswerEventArgs : EventArgs {

    public string pickedOption;

    public OnCheckAnswerEventArgs(string option)
    {
        this.pickedOption = option;
    }
}
