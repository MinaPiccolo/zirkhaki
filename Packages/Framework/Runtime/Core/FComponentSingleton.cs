namespace Revy.Framework
{
    public class FComponentSingleton<TBehaviour> : FComponent where TBehaviour : FComponent
    {
        #region Fields
        private static TBehaviour _instance;
        #endregion

        #region Properties
        public static TBehaviour Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new UnityEngine.GameObject(typeof(TBehaviour).ToString()).AddComponent<TBehaviour>();
                }

                return _instance;
            }
        }
        #endregion

        #region Methods
        protected override void Awake()
        {
            base.Awake();

            if (_instance == null)
            {
                _instance = this as TBehaviour;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion
    }
}