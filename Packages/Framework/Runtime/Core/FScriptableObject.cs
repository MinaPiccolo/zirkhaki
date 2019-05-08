namespace Revy.Framework
{
    public abstract class FScriptableObject : UnityEngine.ScriptableObject
    {
        #region Fields
        [UnityEngine.SerializeField] protected string _name;
        #endregion

        #region Properties
        public string Name { get { return _name; } }
        #endregion
    }
}