using UnityEngine;

namespace CameraSystem
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Configs/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Camera Rotation")]
        [field: SerializeField] public float RotationSpeed { get; private set; } = 180f;
        [field: SerializeField] public float RotationStep { get; private set; } = 45f;
    }
}
