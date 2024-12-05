using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RouletteSystem
{
    public class RouletteData
    {
        public SlotData[] SlotDatas
        {
            get
            {                
                return _datas;
            }
            private set { }
        }

        private Dictionary<ESlotItem, SlotData> _slotDatas;

        private SlotData[] _datas;


        public RouletteData()
        {
            Init();
        }


        private void Init()
        {
            _slotDatas = new Dictionary<ESlotItem, SlotData>()
            {
                {ESlotItem.Item_1, Resources.Load<SlotData>("Item_1")},
                {ESlotItem.Item_2, Resources.Load<SlotData>("Item_2")},
                {ESlotItem.Item_3, Resources.Load<SlotData>("Item_3")},
                {ESlotItem.Item_4, Resources.Load<SlotData>("Item_4")},
                {ESlotItem.Item_5, Resources.Load<SlotData>("Item_5")},
                {ESlotItem.Item_6, Resources.Load<SlotData>("Item_6")},
                {ESlotItem.Item_7, Resources.Load<SlotData>("Item_7")},
                {ESlotItem.Item_8, Resources.Load<SlotData>("Item_8")},
            };

            _datas = new SlotData[8];
            var keys = _slotDatas.Keys.ToArray();
            
            for (int i = 0; i < _datas.Length; i++)
            {
                var key = keys[i % keys.Length];
                _datas[i] = _slotDatas[key];
            }            
        }
    }
}