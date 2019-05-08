namespace Revy.Framework
{
    public interface IInitializable
    {
        bool HasInitialized { get; set; }

        /// <summary>
        /// Initialize is used to initialize any variables or game state before the game starts.
        /// Initialize is called only once during the lifetime of the script instance.
        /// Use Initialize instead of the constructor for initialization.
        /// Initialize is called by coroutine.
        /// Always return true when initialization finished.
        /// </summary>
        /// <returns></returns>
        void Initialize();

        /// <summary>
        /// BeginPlay is called when the initialization and resource loading for all IObject is finished.
        /// BeginPlay is called only once during the lifetime of the script instance.
        /// Always return true at the end of method.
        /// BeginPlay is called by coroutine.
        /// </summary>
        /// <returns></returns>
        System.Threading.Tasks.Task BeginPlay();
    }
}