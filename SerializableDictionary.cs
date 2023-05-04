using System;
using System.Collections.Generic;
using UnityEngine;

namespace SerializableDictionary
{
    
    [Serializable]
    public class SerializableKVP<K, V>
    {
        [SerializeField]
        private K _key;
        public K Key => _key;
        
        
        [SerializeField]
        private V _value;
        public V Value => _value;
    }
    
    
    [Serializable]
    public class SerializableDictionary<K, V> : Dictionary<K, SerializableKVP<K, V>>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializableKVP<K, V>> _keys = new List<SerializableKVP<K, V>>();

        public static implicit operator SerializableDictionary<K, V>(Dictionary<K, V> dictionary)
        {
            SerializableDictionary<K, V> serializableDict = dictionary;
            return serializableDict;
        }

        public void OnBeforeSerialize()
        {
            if (!(Count > _keys.Count)) return;
            foreach (var kvp in this)
            {
                _keys.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var kvp in _keys)
            {
                if (kvp == null) continue;
                if (kvp.Key == null) continue;
                if (kvp.Value == null) continue;
                if (ContainsKey(kvp.Key)) continue;

                Add(kvp.Key, kvp);
            }
        }
    }
    
    
    [Serializable]
    public class SerializableKvpBoxed<K, V> : List<V>
    {
        [SerializeField] private K _key;
        public K Key => _key;
        
        
        [SerializeField] private List<V> _values;
        public List<V> Values => _values;
        public V Value => _values[0];
    }

    [Serializable]
    public class SerializableDictionaryBoxed<K, V> : Dictionary<K, SerializableKvpBoxed<K, V>>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<SerializableKvpBoxed<K, V>> _keys = new List<SerializableKvpBoxed<K, V>>();

        public static implicit operator SerializableDictionaryBoxed<K, V>(Dictionary<K, V> dictionary)
        {
            SerializableDictionaryBoxed<K, V> serializableDict = dictionary;
            return serializableDict;
        }

        public void OnBeforeSerialize()
        {
            if (!(Count > _keys.Count)) return;
            foreach (var kvp in this)
            {
                _keys.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var kvp in _keys)
            {
                if (kvp == null) continue;
                if (kvp.Key == null) continue;
                if (kvp.Values == null) continue;
                if (ContainsKey(kvp.Key)) continue;
                Add(kvp.Key, kvp);
                kvp.AddRange(kvp.Values);
                kvp.Values.CopyTo(kvp.ToArray());
            }
        }
    }
}
