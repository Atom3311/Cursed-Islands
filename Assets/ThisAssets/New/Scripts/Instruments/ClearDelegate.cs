using System;
public static class ClearDelegate
{
    public static void Execute(Delegate targetDelegate)
    {
        if (targetDelegate == null)
            return;

        Delegate[] allMethods = targetDelegate.GetInvocationList();
        Array.ForEach(allMethods, method =>
        {
            Delegate.Remove(targetDelegate, method);
        });
    }
}