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
            /// TODO : 룰렛 보상 개발 방향성에 따라 룰렛데이터 초기화 방식 설정

            /// ex) 스크립터블 데이터를 사용하여 각각의 룰렛 보상을 만들어둔 후 필요한 아이템에 따라 로드 후 사용            
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