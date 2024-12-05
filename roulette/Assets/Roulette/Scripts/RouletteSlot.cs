using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RouletteSystem
{
    public class RouletteSlot : MonoBehaviour
    {
        public SlotData SlotData { get; private set; }
        public int SlotIndex { get; private set; }

        [SerializeField] private Image _img;
        [SerializeField] private TextMeshProUGUI _count_TMP;
        

        public void Setup(SlotData data, int index)
        {
            SlotData        = data;
            SlotIndex       = index;

            _img.sprite     = data.Sprite;
            _count_TMP.text = $"x{data.Count}";
        }
    } 
}