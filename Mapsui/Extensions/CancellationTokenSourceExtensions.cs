﻿using System.Threading;
using System.Threading.Tasks;

namespace Mapsui.Extensions;
public static class CancellationTokenSourceExtensions
{
    #if !NET8_0_OR_GREATER
    /// <summary> Compatibility for NET 8 </summary>
    /// <param name="source">source</param>
    /// <returns>Task</returns>
    public static Task CancelAsync(CancellationTokenSource? source)
    {
        source?.Cancel(); 
        return Task.CompletedTask;
    }
    #endif
}
