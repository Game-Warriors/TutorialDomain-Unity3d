using System;
using System.Collections.Generic;
using GameWarriors.TutorialDomain.Abstraction;

namespace GameWarriors.TutorialDomain.Core
{
    public class TutorialSystem : ITutorial
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITutorialResourceLoader _resourceLoader;
        private readonly Dictionary<string, ITutorialSession> _tutorialTable;
        private readonly Dictionary<string, int> _doneTutorials;

        public event Action<ITutorialSession> OnTutorialSetup;
        public event Action<ITutorialSession> OnTutorialDone;
        public event Action<ITutorialSession> OnNextTutorialSelect;

        [UnityEngine.Scripting.Preserve]
        public TutorialSystem(IServiceProvider serviceProvider, ITutorialResourceLoader resourceLoader)
        {
            _serviceProvider = serviceProvider;
            _resourceLoader = resourceLoader;
            if (_resourceLoader == null)
                _resourceLoader = new DefaultResourceLoader();
            _tutorialTable = new Dictionary<string, ITutorialSession>();
            _doneTutorials = new Dictionary<string, int>();
        }

        public void SetDoneTutorials(IEnumerable<string> doneItems)
        {
            _doneTutorials.Clear();
            foreach (string item in doneItems)
            {
                if (!string.IsNullOrEmpty(item))
                    _doneTutorials.Add(item, 0);
            }
        }

        public ITutorialSession StartTutorialJourney(string sessionName)
        {
            if (!_tutorialTable.ContainsKey(sessionName) && !_doneTutorials.ContainsKey(sessionName))
            {
                ITutorialSession sessionData = _resourceLoader.LoadResource(sessionName);
                StartTutorial(sessionData);
                return sessionData;
            }
            return null;
        }

        public IEnumerable<ITutorialSession> GetCurrentTutorials()
        {
            foreach (ITutorialSession item in _tutorialTable.Values)
            {
                yield return item;
            }
        }

        public void TutorialEnd(ITutorialSession tutorialSession)
        {
            ITutorialSession nextTutorial = null;
            foreach (string nextSession in tutorialSession.GetNextSessions())
            {
                _doneTutorials.Add(tutorialSession.TutorialKey, 0);
                nextTutorial = StartTutorialJourney(nextSession);
            }
            _tutorialTable.Remove(tutorialSession.TutorialKey);
            OnTutorialDone?.Invoke(tutorialSession);
            if (nextTutorial != null)
                OnNextTutorialSelect?.Invoke(nextTutorial);
            _resourceLoader.ReleaseTutorialResource(tutorialSession);
        }


        private void StartTutorial(ITutorialSession tutorialSession)
        {
            if (tutorialSession != null)
            {
                tutorialSession.Initialization(_serviceProvider);
                tutorialSession.OnTutorialEnd += TutorialEnd;
                OnTutorialSetup(tutorialSession);
            }
        }

    }
}