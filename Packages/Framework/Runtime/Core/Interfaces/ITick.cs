namespace Revy.Framework
{
    public interface ITick : IActiveable
    {
        /// <summary>
        /// Tick is called every frame, if the FObject is enabled.
        /// </summary>
        void Tick();
    }
}
