namespace Clicker.Workers
{
    // A state that has something to restart when the machine gives it back after an interruption.
    // Resume replaces Enter there: Enter would also reset the progress the interruption must keep.
    public interface IResumable
    {
        void Resume();
    }
}
