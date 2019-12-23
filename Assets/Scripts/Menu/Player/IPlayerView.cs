using System;

interface IPlayerView
{
    event EventHandler<ChangeNameEventArgs> OnChangeName;
}