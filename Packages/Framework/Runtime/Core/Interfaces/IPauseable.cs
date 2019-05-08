namespace Revy.Framework
{

    public interface IPauseable
    {
        /// <summary>
        /// Sent to all IFBase objects when the game paused.
        /// </summary>
        void OnPause();

        /// <summary>
        /// Sent to all IFBase objects when the game resumed.
        /// </summary>
        void OnResume();
    }

}