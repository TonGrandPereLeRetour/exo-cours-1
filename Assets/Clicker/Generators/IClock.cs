using System;

namespace Clicker.Generators
{
    // Lets tests choose the current time; DateTime.UtcNow cannot be controlled.
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
