using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Text;

namespace RouletteSystem
{
    public class RouletteView : MonoBehaviour
    {
        /// <summary>
        /// property
        /// </summary>        
        public int SlotSize
        {
            get
            {
                return _slots.Length;
            }
        }



        /// <summary>
        /// serializedField
        /// </summary>
        [SerializeField] private RouletteSlot[] _slots;
        [SerializeField] private RewardSlotView _rewardView;

        [SerializeField] private GameObject _parent;
        [SerializeField] private Button _spin_Button;
        [SerializeField] private Transform _rouletteSpiner;
        [SerializeField] private AnimationCurve _spinningCurve;




        /// <summary>
        /// private
        /// </summary>
        private float _angle;
        private float _halfAngle;
        private float _halfAnglePadding;

        private float _spinDuration = 5;
        private bool _isSpinning;
        private RouletteSlot _selectSlot;



        private int accumulatedWeight;

        public void Init()
        {
            _rewardView.Init(()=>
            {
                _isSpinning = false;
                _spin_Button.interactable = true;
            });

            _angle              = 360 / _slots.Length;
            _halfAngle          = _angle * 0.5f;
            _halfAnglePadding   = _halfAngle - (_halfAngle * 0.25f);

            _spin_Button.onClick.AddListener(Spin);
        }

        public void Show(SlotData[] slotDatas)
        {
            if(slotDatas.Length != SlotSize)
            {
                Debug.LogError($"룰렛의 슬롯 개수와 데이터의 개수가 다릅니다.");
                return;
            }

            for (int i = 0; i < _slots.Length; i++)
                _slots[i].Setup(slotDatas[i], i);

            _parent.SetActive(true);
        }

        public void Spin()
        {
            if(_isSpinning)
                return;

            _selectSlot = GetRandomSlot();

            float angle = _angle * _selectSlot.SlotIndex;

            float leftOffset  = (angle - _halfAnglePadding) % 360;
            float rightOffset = (angle + _halfAnglePadding) % 360;
            float randomAngle = Random.Range(leftOffset, rightOffset);

            int rotateSpeed     = 2;
            float targetAngle   = randomAngle + 360 * _spinDuration * rotateSpeed;

            _isSpinning = true;
            _spin_Button.interactable = false;

            StartCoroutine(Co_Spin(targetAngle));

            IEnumerator Co_Spin(float end)
            {
                float current = 0;
                float maximum = 0;

                while (maximum < 1)
                {
                    current += Time.deltaTime;
                    maximum = current / _spinDuration;

                    float z = Mathf.Lerp(0, end, _spinningCurve.Evaluate(maximum));
                    _rouletteSpiner.rotation = Quaternion.Euler(0, 0, z);

                    yield return null;
                }

                _rouletteSpiner.rotation = Quaternion.Euler(0, 0, end);
                SlotData selectedSlot = _selectSlot.SlotData;
                
                _rewardView.ShowRewardSlotView(selectedSlot.Sprite, selectedSlot.Count);
            }
        }


        private RouletteSlot GetRandomSlot()
        {
            float[] slotWeights = new float[_slots.Length];

            for (int i = 0; i < slotWeights.Length; i++)
                slotWeights[i] = _slots[i].SlotData.Chance;

            int randomWeight = Choose(slotWeights);

            Debug.Log($"슬롯 인덱스 : {randomWeight} \n 슬롯 아이템 : {_slots[randomWeight].SlotData.ItemName}");
            
            return _slots[randomWeight];
        }

        private int Choose(float[] probs)
        {
            float total = 0;

            foreach (float elem in probs)
                total += elem;

            float randomPoint = Random.value * total;

            for (int i = 0; i < probs.Length; i++)
            {
                if (randomPoint < probs[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= probs[i];
                }
            }

            return probs.Length - 1;
        }


        [System.Serializable]
        private class RewardSlotView
        {
            public GameObject _parent;
            public Image IMG_Reward_Item;
            public TextMeshProUGUI TMP_Reward_Count;
            public Button BTN_Close;

            public void Init(System.Action closeAction)
            {
                BTN_Close.onClick.AddListener(() => CloseRewardSlotView(closeAction));

                _parent.SetActive(false);
            }

            public void ShowRewardSlotView(Sprite sprite, int count)
            {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append("x");
                stringBuilder.Append(count);

                IMG_Reward_Item.sprite  = sprite;
                TMP_Reward_Count.text   = stringBuilder.ToString();

                _parent.SetActive(true);
            }

            public void CloseRewardSlotView(System.Action closeAction)
            {
                closeAction?.Invoke();

                _parent.SetActive(false);
            }
        }
    }
}