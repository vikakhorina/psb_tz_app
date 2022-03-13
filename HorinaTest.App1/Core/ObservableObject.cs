﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HorinaTest.App1.Core
{
    internal class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnSetNewValue<T>(
        ref T Property, T Value, [CallerMemberName] string propertyName = "")
        {
            Property = Value;
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
