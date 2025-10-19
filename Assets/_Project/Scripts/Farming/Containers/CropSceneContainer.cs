using UnityEngine;
using UnityEngine.UI;

namespace Farming
{
    public class CropSceneContainer : MonoBehaviour
    {
        [field: SerializeField] public GameObject CropUI { get; private set; }
        [field: SerializeField] public Image WaterBar { get; private set; }
        [field: SerializeField] public Image GrowthBar { get; private set; }
    }
}
