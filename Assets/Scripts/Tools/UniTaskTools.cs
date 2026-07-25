using System.Threading;
using UnityEngine;

public static class UniTaskTools
{
    public static void KillToken(ref CancellationTokenSource token)
    {
        if (token == null)
            return;
        token.Cancel();
        token.Dispose();
        token = null;
    }
    public static void RenewToken(ref CancellationTokenSource token)
    {
        if (token != null)
        {
            token.Cancel();
            token.Dispose();
        }
        token = new();
    }
}
