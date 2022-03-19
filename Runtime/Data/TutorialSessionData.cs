using System;
using System.Collections.Generic;
using GameWarriors.TutorialDomain.Abstraction;
using UnityEngine;

namespace GameWarriors.TutorialDomain.Data
{
    //[CreateAssetMenu(fileName = "TutorialItemData", menuName = "Tutorial/Create Tutorial Item Data")]
    public abstract class TutorialSessionData : ScriptableObject, ITutorialSession
    {
        public event Action<ITutorialSession> OnTutorialEnd;
        public event Action<ITutorialSession> OnTutorialTrigger;

        [SerializeField]
        protected string [] _nextSessionName;

        public abstract string TutorialKey { get; }

        public abstract void Initialization(IServiceProvider serviceProvider);

        protected virtual void OnDone()
        {
            OnTutorialEnd?.Invoke(this);
        }

        protected virtual void OnTriggerd()
        {
            OnTutorialTrigger?.Invoke(this);
        }

        public IEnumerable<string> GetNextSessions()
        {
            return _nextSessionName;
        }


    }
}