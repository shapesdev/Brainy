using System;

interface ILevelSelectionView
{
    event EventHandler<OnValueUpdateEventArgs> OnValueUpdate;
}