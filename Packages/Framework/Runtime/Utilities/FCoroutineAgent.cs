using System.Collections;
using System.Threading.Tasks;

namespace Revy.Framework
{
    public class FCoroutineAgent : FComponentSingleton<FCoroutineAgent>, IInitializable
    {
        public bool HasInitialized { get; set; }

        public void Initialize()
        {
            Persistent.MakePersist(this, PersistentSubCategories.UTILITIES);
        }

        public Task BeginPlay()
        {
           return Task.CompletedTask;
        }
    }
}