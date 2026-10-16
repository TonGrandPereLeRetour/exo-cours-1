namespace Clicker.Workers
{
    // One behaviour of the worker. The machine calls Enter once on arrival, Tick every frame while
    // the state is current, and Exit once on departure: setup and cleanup live in one place.
    public interface IState
    {
        void Enter();

        void Tick(float deltaTime);

        void Exit();
    }
}
