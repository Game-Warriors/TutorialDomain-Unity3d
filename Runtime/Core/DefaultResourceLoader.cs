using System;
using GameWarriors.TutorialDomain.Abstraction;
using GameWarriors.TutorialDomain.Data;
using UnityEngine;

namespace GameWarriors.TutorialDomain.Core
{
    public class DefaultResourceLoader : ITutorialResourceLoader
    {
        public ITutorialSession LoadResource(string assetName)
        {
            return Resources.Load<ScriptableObject>(assetName) as ITutorialSession;
        }

        public void LoadResourceAsync(string assetName, Action<TutorialData> onLoadDone)
        {
            ResourceRequest operation = Resources.LoadAsync<TutorialData>(assetName);
            operation.completed += (asyncOperation) => onLoadDone((asyncOperation as ResourceRequest).asset as TutorialData);
        }

        public void UnloadTutorialData(TutorialData tutorialData)
        {
            Resources.UnloadAsset(tutorialData);
        }

        public void ReleaseTutorialResource(ITutorialSession item)
        {
            ScriptableObject obj = item as ScriptableObject;
            if (obj != null)
                Resources.UnloadAsset(obj);
        }
    }
}