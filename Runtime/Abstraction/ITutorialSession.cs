using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWarriors.TutorialDomain.Abstraction
{
    public interface ITutorialSession
    {
        string TutorialKey { get; }

        event Action<ITutorialSession> OnTutorialEnd;
        void Initialization(IServiceProvider serviceProvider);
        IEnumerable<string> GetNextSessions();
    }
}
