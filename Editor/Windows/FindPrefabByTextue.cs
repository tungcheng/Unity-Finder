﻿using UnityEditor;
using UnityEngine;

namespace litefeel.Finder.Editor
{
    class FindPrefabByTextue : FindAssetWindowBase<Texture, UnityEngine.Object>
    {
        protected override void ConfigValues()
        {
            m_DisableFind = m_Asset == null;
            m_EnabledFindInScene = m_Asset != null;
        }

        protected override bool InGameObjectAndChildren(GameObject prefab)
        {
            return UnityUtil.AnyOneComponentAndChildren<Component>((comp) =>
            {
                return UnityUtil.AnyOneProperty((prop) =>
                {
                    return prop.propertyType == SerializedPropertyType.ObjectReference &&
                    prop.objectReferenceValue == m_Asset;

                }, comp);
            }, prefab.transform);
        }

        protected override void OnItemDoubleClick(int index)
        {
            AssetDatabase.OpenAsset(m_Items[index]);
        }
    }
}

