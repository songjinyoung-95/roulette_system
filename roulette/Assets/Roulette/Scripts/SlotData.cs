using UnityEngine;
using UnityEngine.UI;

namespace RouletteSystem
{
    [CreateAssetMenu(fileName = "SlotData", menuName = "Roulette/SlotData")]
    public class SlotData : ScriptableObject
    {
        public ESlotItem ItemType        => _itemType;
        public string    ItemName        => _itemName;
        public int       ItemCount       => _itemCount;

        public int       Chance          => _chance;
        public Sprite    Sprite          => _itemSprite;

        [SerializeField] private ESlotItem _itemType;
        [SerializeField] private string _itemName;
        [SerializeField] int _chance;
        [SerializeField] int _itemCount;
        [SerializeField] Sprite _itemSprite;
    }
}