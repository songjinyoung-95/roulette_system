using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Text;

namespace RouletteSystem
{
    public class RouletteView : MonoBehaviour
    {
        public delegate void SelectedRouletteItemEventHandler(SlotData slot);
        public delegate RouletteSlot SelectedRulleteSlotEventHandler();


        public event SelectedRouletteItemEventHandler OnItemSelected;
        public event SelectedRulleteSlotEventHandler OnSelectedSlot;

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

        public RouletteSlot[] Slots
        {
            get
            {
                return _slots;
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


        private void Spin()
        {
            if(_isSpinning)
                return;

            _selectSlot = OnSelectedSlot();
            OnItemSelected?.Invoke(_selectSlot.SlotData);

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
                
                _rewardView.ShowRewardSlotView(selectedSlot.Sprite, selectedSlot.ItemCount);
            }
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