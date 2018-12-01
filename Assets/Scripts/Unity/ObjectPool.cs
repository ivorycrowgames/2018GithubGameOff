using System;
using System.Text;
using System.Collections.Generic;

using UnityEngine;

using IvoryCrow.Utilities;

namespace IvoryCrow.Unity
{
    public class GameObjectFactory
    {
        public enum CreateDestroyStrategy { NoAction, EnableDisableObject };
        public enum GrowthStrategy { AllowGrowth, Fixed };

        private GrowthStrategy _growthStrategy;
        private CreateDestroyStrategy _createDestroyStrategy;
        private GameObject _prefab;
        private Stack<GameObject> _pool = new Stack<GameObject>();

        public GameObjectFactory(GameObject prefab, int initialCount) :
            this(prefab, initialCount, GrowthStrategy.AllowGrowth, CreateDestroyStrategy.EnableDisableObject)
        {
        }

        public GameObjectFactory(GameObject prefab, int initialCount, GrowthStrategy growthStrategy) :
            this(prefab, initialCount, growthStrategy, CreateDestroyStrategy.EnableDisableObject)
        {
        }

        public GameObjectFactory(GameObject prefab, int initialCount, CreateDestroyStrategy createDestroyStrategy) :
            this(prefab, initialCount, GrowthStrategy.AllowGrowth, createDestroyStrategy)
        {
        }

        public GameObjectFactory(GameObject prefab, int initialCount, GrowthStrategy growthStrategy, CreateDestroyStrategy createDestroyStrategy)
        {
            _prefab = prefab;
            _growthStrategy = growthStrategy;
            _createDestroyStrategy = createDestroyStrategy;
            AddObjectsToPool(initialCount);
        }

        public GameObject Create()
        {
            if (_pool.Count == 0)
            {
                if (_growthStrategy == GrowthStrategy.AllowGrowth)
                {
                    AddObjectsToPool(1);
                }
                else
                {
                    Throw.Error("Pool depleted and cannot grow");
                    return null;
                }
            }

            GameObject returnObject = _pool.Pop();
            if (_createDestroyStrategy == CreateDestroyStrategy.EnableDisableObject)
            {
                returnObject.SetActive(true);
            }

            return returnObject;
        }

        public void Destroy(GameObject obj)
        {
            if (obj)
            {
                if (_createDestroyStrategy == CreateDestroyStrategy.EnableDisableObject)
                {
                    obj.SetActive(false);
                }

                _pool.Push(obj);
            }
        }

        private void AddObjectsToPool(int count)
        {
            for(int i = 0; i < count; ++i)
            {
                GameObject instantiatedObject = GameObject.Instantiate(_prefab);
                if (_createDestroyStrategy == CreateDestroyStrategy.EnableDisableObject)
                {
                    instantiatedObject.SetActive(false);
                }

                _pool.Push(instantiatedObject);
            }
        }
    }
}
