using System;
using System.Collections.Generic;


namespace GameWarriors.TutorialDomain.Abstraction
{
    public interface ITutorial
    {
        event Action<ITutorialSession> OnTutorialSetup;
        event Action<ITutorialSession> OnTutorialDone;
        event Action<ITutorialSession> OnNextTutorialSelect;

        void AddTutorialAsset(string assetName, object asset);
        object RemoveTutorialAsset(string assetName);
        void ClearAllTutorialAssets();

        void SetDoneTutorials(IEnumerable<string> doneItems);
        ITutorialSession StartTutorialJourney(string sessionName);
        IEnumerable<ITutorialSession> GetCurrentTutorials();
        void TutorialEnd(ITutorialSession tutorialSession);

    }
}