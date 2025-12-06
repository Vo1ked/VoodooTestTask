using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Gameplay.Data;
using UnityEngine;

namespace Editor.Utils
{

    [CustomEditor(typeof(BoosterLevelsConfig))]
    public class BoosterLevelsConfigEditor : UnityEditor.Editor
    {
        private const string BoosterLevelIdFieldPath = "m_Id";
        private const string BoosterLevelPowerUpsFieldPath = "m_PowerUps";
        
        private SerializedProperty m_BasePowerUpsProperty;
        private SerializedProperty m_BoosterLevelsProperty;

        private int m_RangeFrom;
        private int m_RangeTo;
        private bool m_OverrideBoosterLevels;
        private int m_LastCount;

        private void OnEnable()
        {
            m_BasePowerUpsProperty = serializedObject.FindProperty("m_BasePowerUps");
            m_BoosterLevelsProperty = serializedObject.FindProperty("m_boosterLevels");
            m_LastCount = m_BoosterLevelsProperty.arraySize;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(m_BasePowerUpsProperty, true);
            EditorGUILayout.Space();
            DrawBoosterLevelsList();
            EditorGUILayout.Space();
            DrawTools();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawBoosterLevelsList()
        {
            EditorGUILayout.PropertyField(m_BoosterLevelsProperty, true);

            if (m_BoosterLevelsProperty.arraySize <= m_LastCount) 
                return;
            
            int newIndex = m_BoosterLevelsProperty.arraySize - 1;
            var element = m_BoosterLevelsProperty.GetArrayElementAtIndex(newIndex);

            int maxId = 0;
            for (int i = 0; i < m_BoosterLevelsProperty.arraySize - 1; i++)
            {
                var boosterLevelProperty = m_BoosterLevelsProperty.GetArrayElementAtIndex(i);
                maxId = Mathf.Max(maxId, boosterLevelProperty.FindPropertyRelative(BoosterLevelIdFieldPath).intValue);
            }

            element.FindPropertyRelative(BoosterLevelIdFieldPath).intValue = maxId + 1;

            var levelPowerUps = element.FindPropertyRelative(BoosterLevelPowerUpsFieldPath);
            levelPowerUps.arraySize = m_BasePowerUpsProperty.arraySize;
            for (int i = 0; i < m_BasePowerUpsProperty.arraySize; i++)
            {
                levelPowerUps.GetArrayElementAtIndex(i).objectReferenceValue =
                    m_BasePowerUpsProperty.GetArrayElementAtIndex(i).objectReferenceValue;
            }

            m_LastCount = m_BoosterLevelsProperty.arraySize;
        }

        private void DrawTools()
        {
            EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("From", GUILayout.Width(30));
            m_RangeFrom = EditorGUILayout.IntField(m_RangeFrom, GUILayout.Width(30));
            EditorGUILayout.LabelField("To", GUILayout.Width(20));
            m_RangeTo = EditorGUILayout.IntField(m_RangeTo, GUILayout.Width(30));
            GUILayout.Space(20);
            m_OverrideBoosterLevels = EditorGUILayout.ToggleLeft("Override if exist", m_OverrideBoosterLevels,GUILayout.Width(150));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Range"))
                AddRange(m_RangeFrom, m_RangeTo);
            
            if (GUILayout.Button("Remove Range"))
                RemoveRange(m_RangeFrom, m_RangeTo);
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (GUILayout.Button("Remove Duplicates"))
                RemoveDuplicates();

            if (GUILayout.Button("Sort by Id"))
                SortById();
            
            if (GUILayout.Button("Organize Sequentially"))
                OrganizeSequential();
        }

        private void AddRange(int from, int to)
        {
            if (to < from)
            {
                ((BoosterLevelsConfig)target).name = ((BoosterLevelsConfig)target).name; // for set a dirty
                Debug.LogWarning("Invalid range");
                return;
            }

            for (int id = from; id <= to; id++)
            {
                int index = FindIndexById(id);
                SerializedProperty elementAtIndex = null;
                if (index < 0)
                {
                    int newIndex = m_BoosterLevelsProperty.arraySize;
                    m_BoosterLevelsProperty.InsertArrayElementAtIndex(newIndex);
                    elementAtIndex = m_BoosterLevelsProperty.GetArrayElementAtIndex(newIndex);
                    elementAtIndex.FindPropertyRelative(BoosterLevelIdFieldPath).intValue = id;
                }

                if (index > -1 && m_OverrideBoosterLevels)
                {
                    elementAtIndex = m_BoosterLevelsProperty.GetArrayElementAtIndex(index);
                }

                if (elementAtIndex == null) 
                    continue;
                
                var levelPowerUps = elementAtIndex.FindPropertyRelative(BoosterLevelPowerUpsFieldPath);
                levelPowerUps.arraySize = m_BasePowerUpsProperty.arraySize;
                for (int i = 0; i < m_BasePowerUpsProperty.arraySize; i++)
                {
                    levelPowerUps.GetArrayElementAtIndex(i).objectReferenceValue =
                        m_BasePowerUpsProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                }
            }
        }

        private void RemoveRange(int from, int to)
        {
            if (to < from)
            {
                ((BoosterLevelsConfig)target).name = ((BoosterLevelsConfig)target).name; // for set a dirty
                Debug.LogWarning("Invalid range");
                return;
            }

            for (int id = from; id <= to; id++)
            {
                int index = FindIndexById(id);
                if (index < 0)
                    continue;
                m_BoosterLevelsProperty.DeleteArrayElementAtIndex(index);
            }
            
            m_LastCount = m_BoosterLevelsProperty.arraySize;
            serializedObject.ApplyModifiedProperties();
        }

        private int FindIndexById(int id)
        {
            for (int i = 0; i < m_BoosterLevelsProperty.arraySize; i++)
            {
                var elementAtIndex = m_BoosterLevelsProperty.GetArrayElementAtIndex(i);
                if (elementAtIndex.FindPropertyRelative(BoosterLevelIdFieldPath).intValue == id)
                    return i;
            }

            return -1;
        }

        private void RemoveDuplicates()
        {
            HashSet<int> seen = new HashSet<int>();

            for (int i = m_BoosterLevelsProperty.arraySize - 1; i >= 0; i--) // Start From end for keep latest
            {
                var el = m_BoosterLevelsProperty.GetArrayElementAtIndex(i);
                int id = el.FindPropertyRelative(BoosterLevelIdFieldPath).intValue;

                if (!seen.Add(id))
                    m_BoosterLevelsProperty.DeleteArrayElementAtIndex(i);
            }
        }

        void SortById()
        {
            var levels = new List<BoosterLevel>();
            SerializedProperty boosterLevel;
            SerializedProperty powerUpsProperty;
            
            for (int i = 0; i < m_BoosterLevelsProperty.arraySize; i++)
            {
                boosterLevel = m_BoosterLevelsProperty.GetArrayElementAtIndex(i);

                int id = boosterLevel.FindPropertyRelative(BoosterLevelIdFieldPath).intValue;

                powerUpsProperty = boosterLevel.FindPropertyRelative(BoosterLevelPowerUpsFieldPath);
                PowerUpData[] powerUps = new PowerUpData[powerUpsProperty.arraySize];
                for (int j = 0; j < powerUps.Length; j++)
                {
                    powerUps[j] = (PowerUpData)powerUpsProperty.GetArrayElementAtIndex(j).objectReferenceValue;
                }

                levels.Add(new BoosterLevel(id,powerUps));
            }

            levels = levels.OrderBy(l => l.Id).ToList();

            m_BoosterLevelsProperty.arraySize = levels.Count;

            for (int i = 0; i < levels.Count; i++)
            {
                boosterLevel = m_BoosterLevelsProperty.GetArrayElementAtIndex(i);
                boosterLevel.FindPropertyRelative(BoosterLevelIdFieldPath).intValue = levels[i].Id;

                powerUpsProperty = boosterLevel.FindPropertyRelative(BoosterLevelPowerUpsFieldPath);
                powerUpsProperty.arraySize = levels[i].PowerUps.Count;

                for (int j = 0; j < levels[i].PowerUps.Count; j++)
                {
                    powerUpsProperty.GetArrayElementAtIndex(j).objectReferenceValue = levels[i].PowerUps[j];
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
        private void OrganizeSequential()
        {
            SortById();
            for (int i = 0; i < m_BoosterLevelsProperty.arraySize; i++)
            {
                var boosterLevel = m_BoosterLevelsProperty.GetArrayElementAtIndex(i);
                boosterLevel.FindPropertyRelative(BoosterLevelIdFieldPath).intValue = i;
            }

            serializedObject.ApplyModifiedProperties();
            Debug.Log("Levels reorganized sequentially.");
        }

    }
}