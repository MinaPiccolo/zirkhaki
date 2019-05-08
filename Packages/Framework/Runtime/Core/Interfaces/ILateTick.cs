namespace Revy.Framework
{
    public interface ILateTick : IActiveable
    {
        /// <summary>
        /// Tick is called every frame, if the FObject is enabled.
        /// </summary>
        void LateTick();
    }
}
