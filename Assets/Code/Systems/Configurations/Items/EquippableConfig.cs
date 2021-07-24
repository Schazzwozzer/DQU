using UnityEngine;

namespace DQU.Configurations
{
    public enum EquipmentSlot
    {
        Hand,
        Body,
        Bow
    }


    public class EquippableConfig : ItemConfig
    {
        [SerializeField]
        private EquipmentSlot _slot;
        public EquipmentSlot Slot { get { return _slot; } }
    }
}
