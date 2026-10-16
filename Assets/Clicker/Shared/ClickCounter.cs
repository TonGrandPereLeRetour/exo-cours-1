using System;

namespace Clicker.Shared
{
    // Click total shared by the Click slice, which counts, and the Achievements slice, which watches.
    public sealed class ClickCounter
    {
        public long Total { get; private set; }

        public event Action<long> Clicked;

        public void Register()
        {
            Total++;
            Clicked?.Invoke(Total);
        }

        public void Restore(long total)
        {
            if (total < 0) throw new ArgumentOutOfRangeException(nameof(total));
            Total = total;
        }
    }
}
