using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Yapoml.Playwright.Components;

/// <summary>
/// Non-generic root of every condition object, owning the chain that its steps are built into.
/// </summary>
public abstract class BaseConditions
{
    /// <summary>The chain these conditions build their steps into.</summary>
    internal Chain Chain { get; set; } = new Chain();

    /// <summary>
    /// Makes the child conditions build into the same chain as these conditions.
    /// </summary>
    protected T Share<T>(T child) where T : BaseConditions
    {
        child.Chain = Chain;

        return child;
    }
}

/// <summary>
/// Base class for all awaitable condition objects, providing shared timeout and polling interval configuration.
/// </summary>
/// <typeparam name="TSelf">The concrete conditions type for fluent chaining.</typeparam>
public abstract class BaseConditions<TSelf> : BaseConditions
{
    /// <summary>The concrete conditions instance for fluent chaining.</summary>
    protected TSelf _self;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseConditions{TSelf}"/> class.
    /// </summary>
    /// <param name="timeout">The maximum duration to wait for conditions.</param>
    /// <param name="pollingInterval">The interval between condition checks.</param>
    protected BaseConditions(TimeSpan timeout, TimeSpan pollingInterval)
    {
        Timeout = timeout;
        PollingInterval = pollingInterval;
    }

    /// <summary>Gets the maximum duration to wait for conditions.</summary>
    protected TimeSpan Timeout { get; }
    /// <summary>Gets the interval between condition checks.</summary>
    protected TimeSpan PollingInterval { get; }

    /// <summary>Enqueues a condition to be evaluated when the chain is awaited.</summary>
    protected TSelf Enqueue(Func<Task> condition)
    {
        Chain.Add(condition);

        return _self;
    }

    /// <summary>Executes the pending steps and returns the owner of these conditions.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public TaskAwaiter<TSelf> GetAwaiter()
    {
        return AwaitChainAsync().GetAwaiter();
    }

    private async Task<TSelf> AwaitChainAsync()
    {
        await Chain.RunAsync().ConfigureAwait(false);

        return _self;
    }
}
