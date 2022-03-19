using System;
using System.Collections;
using System.Collections.Generic;
using GameWarriors.TutorialDomain.Data;
using UnityEngine;

namespace GameWarriors.TutorialDomain.Abstraction
{
    public interface ITutorialResourceLoader
    {
        void LoadResourceAsync(string assetName, Action<TutorialData> onLoadDone);
        ITutorialSession LoadResource(string assetName);
        void UnloadTutorialData(TutorialData tutorialData);
        void ReleaseTutorialResource(ITutorialSession item);
    }
}