using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWarriors.TutorialDomain.Data
{
    [CreateAssetMenu(fileName = "TutorialData", menuName = "Tutorial/Create Tutorial Data")]
    public class TutorialData : ScriptableObject
    {
        public const string ASSET_PATH = "Assets/Scripts/TutorialSystem/Resources/TutorialData.asset";
        public const string RESOURCE_PATH = "TutorialData";

        [SerializeField]
        private TutorialSessionData[] _initialTutorials;

        public TutorialSessionData[] InitialTutorials => _initialTutorials;
        public int InitialTutorialsCount => _initialTutorials?.Length ?? 0;
    }
}