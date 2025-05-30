using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace KatAudio.Editor
{
    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : UnityEditor.Editor
    {
        private Dictionary<string, List<AudioClip>> soundGroup;
        private List<GroupEditor> groupEditors = new();
        private ReorderableList groupList;
        private Dictionary<string, ReorderableList> audioClipLists = new(); // Lưu danh sách AudioClip theo tên nhóm
        private Dictionary<string, bool> groupExpandedStates = new(); // Lưu trạng thái mở rộng theo tên nhóm

        private void OnEnable()
        {
            SoundManager soundManager = (SoundManager)target;
            FieldInfo fieldInfo = typeof(SoundManager).GetField("_soundGroup", BindingFlags.NonPublic | BindingFlags.Instance);
            soundGroup = (Dictionary<string, List<AudioClip>>)fieldInfo.GetValue(soundManager);

            UpdateGroupEditorList(); // Cập nhật danh sách từ Dictionary
            SetupReorderableLists(); // Tạo danh sách hiển thị
        }

        private void UpdateGroupEditorList()
        {
            groupEditors.Clear();
            if (soundGroup == null) return;

            foreach (KeyValuePair<string, List<AudioClip>> entry in soundGroup)
            {
                var groupEditor = new GroupEditor()
                {
                    name = entry.Key,
                    clips = entry.Value
                };
                groupEditors.Add(groupEditor);
            }
        }

        private void SetupReorderableLists()
        {
            // ReorderableList chính cho danh sách GroupEditor
            groupList = new ReorderableList(groupEditors, typeof(GroupEditor), true, true, false, false)
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "Sound Groups");
                },

                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    if (index < 0 || index >= groupEditors.Count) return;

                    var group = groupEditors[index];

                    // Kiểm tra trạng thái mở rộng của nhóm dựa trên `name`
                    if (!groupExpandedStates.ContainsKey(group.name))
                    {
                        groupExpandedStates[group.name] = false; // Mặc định là đóng
                    }

                    Rect foldoutRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                    Rect labelRect = new Rect(rect.x + 15, rect.y, rect.width - 15, EditorGUIUtility.singleLineHeight);

                    // Vẽ Foldout để mở/đóng danh sách
                    groupExpandedStates[group.name] = EditorGUI.Foldout(foldoutRect, groupExpandedStates[group.name], "", true);

                    // Hiển thị tên nhóm cha
                    EditorGUI.LabelField(labelRect, $"Group: {group.name}", EditorStyles.boldLabel);

                    if (groupExpandedStates[group.name]) // Nếu mở, hiển thị danh sách AudioClip
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.Space();

                        if (!audioClipLists.ContainsKey(group.name))
                        {
                            GUI.enabled = false;
                            audioClipLists[group.name] = CreateAudioClipList(group.clips, group.name);
                            GUI.enabled = true;
                        }

                        audioClipLists[group.name].DoLayoutList();
                        EditorGUI.indentLevel--;
                    }
                }
            };
        }

        private ReorderableList CreateAudioClipList(List<AudioClip> clips, string groupName)
        {
            return new ReorderableList(clips, typeof(AudioClip), false, true, false, false) // ⚠ false để tắt thêm/xóa
            {
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, $"Audio Clips ({groupName})");
                },

                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    if (index < 0 || index >= clips.Count) return;

                    GUI.enabled = false; // ⚠ Tắt khả năng chỉnh sửa
                    EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                        clips[index], typeof(AudioClip), false);
                    GUI.enabled = true; // Bật lại để tránh ảnh hưởng các UI khác
                }
            };
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (soundGroup == null)
            {
                EditorGUILayout.HelpBox("SoundGroup is null!", MessageType.Warning);
                return;
            }

            EditorGUILayout.Space();
            groupList.DoLayoutList();
        }
    }

    [System.Serializable]
    public class GroupEditor
    {
        public string name;
        public List<AudioClip> clips;
    }
}
