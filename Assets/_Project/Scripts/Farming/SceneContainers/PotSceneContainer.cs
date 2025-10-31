using System;
using UnityEngine;

namespace Farming
{
    [Serializable]
    public struct PotSpriteSet
    {
        public Sprite EmptySprite;
        public Sprite DugSprite;
        public Sprite WateredSprite;
    }

    public class PotSceneContainer : MonoBehaviour
    {
        [field: SerializeField] public PotSpriteSet[] SpriteSets { get; private set; }
        [field: SerializeField] public CropBase CropBasePrefab { get; private set; }
    }
}
