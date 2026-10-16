using System;
using System.Collections.Generic;
using UnityEngine;

namespace Clicker.Workers
{
    // Holds the current state and every transition between states. The states no longer know each
    // other: the whole graph reads in one place, where it is declared.
    public sealed class StateMachine
    {
        private readonly string owner;
        private readonly Dictionary<IState, List<Transition>> transitions = new Dictionary<IState, List<Transition>>();
        private readonly List<Transition> anyTransitions = new List<Transition>();
        private IState interruption;
        private Func<bool> interruptionStarts;
        private Func<bool> interruptionEnds;
        private IState suspended;

        public StateMachine(string owner)
        {
            this.owner = owner;
        }

        public IState Current { get; private set; }

        public bool IsInterrupted => suspended != null;

        // Declares a state that can pause any other one. The paused state is suspended, not left: no
        // Exit when the interruption starts, no Enter when it ends, so its progress survives.
        public void SetInterruption(IState state, Func<bool> starts, Func<bool> ends)
        {
            interruption = state ?? throw new ArgumentNullException(nameof(state));
            interruptionStarts = starts ?? throw new ArgumentNullException(nameof(starts));
            interruptionEnds = ends ?? throw new ArgumentNullException(nameof(ends));
        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (!transitions.TryGetValue(from, out List<Transition> list))
            {
                list = new List<Transition>();
                transitions.Add(from, list);
            }
            list.Add(new Transition(to, condition));
        }

        // Checked before the transitions of the current state, from whatever state the machine is in.
        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            anyTransitions.Add(new Transition(to, condition));
        }

        // Exit before Enter: the old state cleans up before the new one sets itself up.
        public void ChangeState(IState next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            Current?.Exit();
            Debug.Log($"{owner}: {NameOf(Current)} -> {NameOf(next)}");
            Current = next;
            Current.Enter();
        }

        // At most one change per frame, then the current state runs. While interrupted, the ordinary
        // transitions are frozen: only the end of the interruption is watched.
        public void Tick(float deltaTime)
        {
            if (IsInterrupted)
            {
                if (interruptionEnds()) Resume();
            }
            else if (interruption != null && interruptionStarts())
            {
                Suspend();
            }
            else
            {
                Transition transition = FindTransition();
                if (transition != null) ChangeState(transition.To);
            }
            Current?.Tick(deltaTime);
        }

        private void Suspend()
        {
            suspended = Current;
            Debug.Log($"{owner}: {NameOf(suspended)} suspended -> {NameOf(interruption)}");
            Current = interruption;
            Current.Enter();
        }

        private void Resume()
        {
            Current.Exit();
            Current = suspended;
            suspended = null;
            Debug.Log($"{owner}: {NameOf(interruption)} -> {NameOf(Current)} resumed");
            (Current as IResumable)?.Resume();
        }

        private Transition FindTransition()
        {
            foreach (Transition any in anyTransitions)
            {
                // Without this check, an "any" transition re-enters its own target every frame.
                if (any.To != Current && any.Condition()) return any;
            }
            if (Current == null || !transitions.TryGetValue(Current, out List<Transition> list)) return null;
            foreach (Transition transition in list)
            {
                if (transition.Condition()) return transition;
            }
            return null;
        }

        private static string NameOf(IState state) => state == null ? "none" : state.GetType().Name;

        private sealed class Transition
        {
            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }

            public IState To { get; }

            public Func<bool> Condition { get; }
        }
    }
}
