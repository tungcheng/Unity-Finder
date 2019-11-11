﻿using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace litefeel.Finder.Editor
{
    public class FindFontInPrefabWindow : EditorWindow
    {

        private Font m_Font;
        private readonly List<Text> m_Texts = new List<Text>();
        private string[] m_TextNames;
        private Vector2 m_ScrollPos = Vector2.zero;
        private int m_SelectedIdx = 0;

        private void OnGUI()
        {
            m_Font = EditorGUILayout.ObjectField("Font", m_Font, typeof(Font), false) as Font;
            //selection = AssetDatabase.LoadAssetAtPath(selectedPath, typeof(DefaultAsset)) as DefaultAsset;
            using (new EditorGUI.DisabledScope(m_Font == null))
            {
                if (GUILayout.Button("Find"))
                    FindTexts();
            }
            var count = m_TextNames != null ? m_TextNames.Length : 0;
            EditorGUILayout.LabelField(string.Format("Count:{0}", count));
            if (count > 0)
            {
                m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos);
                if (m_SelectedIdx >= m_TextNames.Length)
                    m_SelectedIdx = 0;
                m_SelectedIdx = GUILayout.SelectionGrid(m_SelectedIdx, m_TextNames, 1, EditorStyles.miniButton, GUILayout.ExpandHeight(false));
                EditorGUILayout.EndScrollView();

                SelectionUtil.Select(m_Texts[m_SelectedIdx]);
            }
        }

        private void FindTexts()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage == null) return;

            var texts = new List<Text>();
            prefabStage.prefabContentsRoot.GetComponentsInChildren(true, texts);

            m_Texts.Clear();
            foreach(var txt in texts)
            {
                if (txt.font == m_Font)
                    m_Texts.Add(txt);
            }
            
            FillMatNames(m_Texts, ref m_TextNames);
            Debug.Log($"XXX {m_Texts.Count}");
        }

        private void FillMatNames(List<Text> mats, ref string[] matNames)
        {
            if (matNames == null || matNames.Length != mats.Count)
                matNames = new string[mats.Count];

            for (var i = 0; i < mats.Count; i++)
            {
                matNames[i] = mats[i].name;
            }
        }
    }
}
