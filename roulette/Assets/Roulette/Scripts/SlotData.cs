using UnityEngine;
using UnityEngine.UI;

namespace RouletteSystem
{
    [CreateAssetMenu(fileName = "SlotData", menuName = "Roulette/SlotData")]
    public class SlotData : ScriptableObject
    {
        public string   ItemName        => _itemName;
        public int      Chance          => _chance;
        public int      Count           => _count;
        public Sprite   Sprite          => _itemSprite;

        [SerializeField] private string _itemName;
        [SerializeField] int _chance;
        [SerializeField] int _count;
        [SerializeField] Sprite _itemSprite;
    }
}