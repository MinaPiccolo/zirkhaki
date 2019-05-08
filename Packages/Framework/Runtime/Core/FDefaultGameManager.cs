using System;

namespace Revy.Framework
{
    [CDisableAutoInstantiation]
    public class FDefaultGameManager : IGameManager, IDefaultGameManager
    {
        public Type ServiceType => typeof(IDefaultGameManager);
    }
}