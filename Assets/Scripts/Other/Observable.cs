using System;

/// <summary>
/// A simple generic observable property.
/// </summary>
public class Observable<T>
{
    private T internalValue;

    public class ChangedEventArgs : EventArgs
    {
        public T OldValue;
        public T NewValue;
    }

    public EventHandler<ChangedEventArgs> Changed;

    public T Value
    {
        get => internalValue;
        set
        {
            if (value == null || !value.Equals(internalValue))
            {
                var oldValue = internalValue;
                internalValue = value;
                var handler = Changed;
                handler?.Invoke(this, new ChangedEventArgs {OldValue = oldValue, NewValue = value});
            }
        }
    }
}