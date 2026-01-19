public interface IPoolable
{
    /// <summary>
    /// start Equivalent for pooled objects
    /// </summary>
    public abstract void OnPulledFromPool();

    /// <summary>
    /// Destroy equivalent for pooled objects
    /// </summary>
    public abstract void OnPushedToPool();
}
