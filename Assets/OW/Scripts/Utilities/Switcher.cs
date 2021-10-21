
using UnityEngine.PlayerLoop;

namespace OW.Scripts.Utilities
{
    public class Switcher<T>
    {
        private T prev;
        private T next;

        public Switcher(T initialValue)
        {
            prev = next = initialValue;
        }

        public bool Update(T now)
        {
            prev = next;
            next = now;

            return !prev.Equals(next);
        }
    }
}
