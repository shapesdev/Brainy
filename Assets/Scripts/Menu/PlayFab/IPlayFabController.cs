using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayFabController {

    event EventHandler<LoggedInEventArgs> OnLoggedIn;
}
