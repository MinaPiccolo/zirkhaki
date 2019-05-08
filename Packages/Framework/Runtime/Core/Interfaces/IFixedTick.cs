namespace Revy.Framework
{
    public interface IFixedTick : IActiveable
    {
        /// <summary>
        /// Tick is called every frame, if the FObject is enabled.
        /// </summary>
        void FixedTick();
    }
}
