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

        public void StartTutorialJourney(string sessionName)
        {
            if (!_tutorialTable.ContainsKey(sessionName))
            {
                ITutorialSession sessionData = _resourceLoader.LoadResource(sessionName);
                StartTutorial(sessionData);
            }
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

            foreach (string nextSession in tutorialSession.GetNextSessions())
            {
                _doneTutorials.Add(tutorialSession.TutorialKey, 0);

                if (!_doneTutorials.ContainsKey(nextSession))
                {
                    StartTutorialJourney(nextSession);
                }
            }
            _tutorialTable.Remove(tutorialSession.TutorialKey);
            OnTutorialDone?.Invoke(tutorialSession);
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