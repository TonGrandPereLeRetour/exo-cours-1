using System;
using Clicker.Shared;

namespace Clicker.Workers
{
    // Context shared by the states, and the only place where the transitions are declared: reading
    // the constructor gives the whole behaviour of the worker.
    public sealed class Worker
    {
        private readonly StateMachine machine;

        public Worker(IWorkerBody body, Wallet wallet, WorkerSettings settings, string name = "Worker")
        {
            Body = body ?? throw new ArgumentNullException(nameof(body));
            Wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            CurrentMine = new Mine(Settings.MinePoint, int.MaxValue);
            machine = new StateMachine(name);
            Idle = new IdleState();
            RecupCaisse = new RecupCaisseState(this);
            GoToCaisse = new GoToCaisseState(this);
            CurrentCaisse = new Caisse();

            machine.AddTransition(Idle, GoToCaisse, () => IsAssigned);
            machine.AddTransition(GoToCaisse, RecupCaisse, () => Body.HasArrived);
            machine.AddTransition(RecupCaisse, Idle, () => true);



            machine.AddTransition(Idle, GoToMine, () => IsAssigned);
            machine.AddTransition(GoToMine, Mining, () => Body.HasArrived);
            machine.AddTransition(Mining, ReturnToBase, () => CarriedOre >= Settings.Capacity || CurrentMine.IsEmpty);
            machine.AddTransition(ReturnToBase, Deposit, () => Body.HasArrived);
            // Checked before the rest: an empty mine ends the work, whatever the trip count says.
            machine.AddTransition(Deposit, Unemployed, () => CurrentMine.IsEmpty);
            machine.AddTransition(Deposit, Rest, () => Deposit.RestDue);
            machine.AddTransition(Deposit, Idle, () => true);
            machine.AddTransition(Rest, Idle, () => Rest.IsOver);
            machine.AddTransition(Unemployed, GoToMine, () => IsAssigned && !CurrentMine.IsEmpty);
            machine.AddAnyTransition(ReturnToBase, () => RecallRequested);
            machine.SetInterruption(TakeCover, () => AlertActive, () => !AlertActive);

            machine.ChangeState(Idle);
        }

        public IWorkerBody Body { get; }

        public RecupCaisseState RecupCaisse {get; }

        public Caisse CurrentCaisse { get; private set; }

        public Wallet Wallet { get; }

        public WorkerSettings Settings { get; }

        public IdleState Idle { get; }
        
        public GoToCaisseState GoToCaisse { get; }

        public GoToMineState GoToMine { get; }

        public MiningState Mining { get; }

        public ReturnToBaseState ReturnToBase { get; }

        public DepositState Deposit { get; }

        public RestState Rest { get; }

        public UnemployedState Unemployed { get; }

        public TakeCoverState TakeCover { get; }

        public IState CurrentState => machine.Current;

        public Mine CurrentMine { get; private set; }

        public int CarriedOre { get; internal set; }

        public int Trips { get; internal set; }

        public bool IsAssigned { get; private set; }

        public bool AlertActive { get; private set; }

        // Set by Recall, cleared by ReturnToBase on entry: the order is obeyed once.
        internal bool RecallRequested { get; set; }

        // The mine of the settings, which never runs out.
        public void AssignMine() => AssignMine(new Mine(Settings.MinePoint, int.MaxValue));

        public void AssignMine(Mine mine)
        {
            CurrentMine = mine ?? throw new ArgumentNullException(nameof(mine));
            IsAssigned = true;
        }


        // Brings the worker home with what it carries, and keeps it there until it is assigned again.
        public void Recall()
        {
            IsAssigned = false;
            RecallRequested = true;
        }

        // Freezes the worker where it is; EndAlert gives it back the state it was in, progress included.
        public void RaiseAlert() => AlertActive = true;

        public void EndAlert() => AlertActive = false;

        public void Tick(float deltaTime) => machine.Tick(deltaTime);

        public void AssignCaisse()
        {
            throw new NotImplementedException();
        }
    }

}
