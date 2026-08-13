using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yapoml.Framework.Options;

namespace Yapoml.Playwright.Components;

/// <summary>
/// An ordered queue of pending asynchronous steps produced by a fluent chain.
/// Steps are enqueued while the chain is being built and executed when it is awaited.
/// </summary>
internal sealed class Chain
{
    private readonly Queue<Func<Task>> _steps = new Queue<Func<Task>>();

    /// <summary>
    /// Gets the chain shared by every page, component and list of the space, registering it when accessed first.
    /// </summary>
    public static Chain Resolve(ISpaceOptions spaceOptions)    {
        if (spaceOptions.Services.TryGet<Chain>(out var chain))
        {
            return chain;
        }

        chain = new Chain();

        spaceOptions.Services.Register(chain);

        return chain;
    }

    /// <summary>Gets a value indicating whether the chain has steps that were not executed yet.</summary>
    public bool HasPending => _steps.Count > 0;

    /// <summary>Enqueues a step to be executed when the chain is awaited.</summary>
    public void Add(Func<Task> step)
    {
        _steps.Enqueue(step ?? throw new ArgumentNullException(nameof(step)));
    }

    /// <summary>Executes the pending steps one by one in the order they were enqueued.</summary>
    public async Task RunAsync()
    {
        while (_steps.Count > 0)
        {
            await _steps.Dequeue()().ConfigureAwait(false);
        }
    }
}
