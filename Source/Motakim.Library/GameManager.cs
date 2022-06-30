namespace Motakim
{
    public abstract class GameManager
    {
        internal protected virtual void OnInitializing() {}
        internal protected virtual void OnceInitialized() {}
        internal protected virtual void OnSceneStarting() {}
        internal protected virtual void OnceSceneStarted() {}
        internal protected virtual void OnUpdating() {}
        internal protected virtual void OnceUpdated() {}
        internal protected virtual void OnRendering() {}
        internal protected virtual void OnceRendered() {}
        internal protected virtual void OnSceneEnding() {}
        internal protected virtual void OnceSceneEnded() {}
        internal protected virtual void OnFinalizing() {}
        internal protected virtual void OnceFinalized() {}
    }
}
