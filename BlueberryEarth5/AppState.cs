using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueberryEarth5
{
    public enum ResourceType
    {
        BlueberryMush, BlueberryBrick
    }

    public class AppState
    {
        protected Dictionary<ResourceType, long> resources = new();

        public event Action OnChange;

        private bool Dirty = false;
        public void MarkDirty() { Dirty = true; }

        public AppState()
        {
            foreach (ResourceType t in Enum.GetValues(typeof(ResourceType)))
            {
                resources[t] = 0;
            }
        }

        public void RedrawIfNeeded()
        {
            if (Dirty)
            {
                OnChange?.Invoke();
            }
            Dirty = false;
        }

        public long GetResourceValue(ResourceType rt)
        {
            return resources[rt];
        }

        public void ModifyResourceValue(ResourceType rt, long amount)
        {
            Console.WriteLine($"Incrementing {rt.ToString()} by {amount}");
            resources[rt] += amount;
            MarkDirty();
        }
    }
}
