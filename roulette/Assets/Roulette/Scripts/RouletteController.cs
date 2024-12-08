using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RouletteSystem
{
    public class RouletteController
    {
        private RouletteData _data;
        private RouletteView _view;

        public void ShowRouletteView()
        {
            if (_data == null)
                _data = new RouletteData();

            if (_view == null)
            {
                LoadView();
                return;
            }

            _view?.Show(_data.SlotDatas);
        }

        private void LoadView()
        {
            var handle = Addressables.InstantiateAsync("Roulette_Canvas");

            handle.Completed +=(AsyncOperationHandle<GameObject> obj) =>
            {
                if(obj.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed Async Status");
                    return;
                }

                if(obj.Result == null)
                {
                    Debug.LogError($"Roulette_Canvas 오브젝트가 제대로 생성되지 않았습니다");
                }

                if(obj.Result.TryGetComponent(out RouletteView component))
                {
                    if(component == null)
                    {
                        Debug.LogError($"{obj.Result.name} Prefab에 : {component}가 제대로 적용되지 않았습니다");
                        return;
                    }
                }

                _view = component;

                _view.Init();
                _view.Show(_data.SlotDatas);

                _view.OnItemSelected += SelectedRouletteItem;
            };
        }

        private void SelectedRouletteItem(SlotData slot)
        {
            // TODO : 타입에 맞는 아이템을 ItemCount만큼 데이터로 저장하는 로직 추가

            ESlotItem   itemType    = slot.ItemType;
            int         itemCount   = slot.ItemCount;

            Debug.Log($"{itemType}의 아이템을 {itemCount}개 획득");
        }
    }
}