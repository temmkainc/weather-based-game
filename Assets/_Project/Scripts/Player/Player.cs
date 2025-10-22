using Farming;
using Farming.Tools;
using UnityEngine;

namespace PlayerSystem
{
    public class Player : MonoBehaviour
    {
        public ToolManager ToolManager { get; private set; }
        public PlayerMovement Movement { get; private set; }

        public CropBase CropPrefab;


        private void Awake()
        {
            ToolManager = GetComponent<ToolManager>();
            Movement = GetComponent<PlayerMovement>();
        }
    }
}
