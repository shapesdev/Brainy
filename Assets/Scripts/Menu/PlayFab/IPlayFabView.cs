using System;
using System.Collections;
using System.Collections.Generic;

public interface IPlayFabView {

    event EventHandler<OnInternetEventArgs> OnInternet;

    void PrintText(string str);
}
