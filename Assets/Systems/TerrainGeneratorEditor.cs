using UnityEditor;
using UnityEngine;

namespace Procedural
{
    public partial class TerrainGenerator
    {
        [CustomEditor(typeof(TerrainGenerator))]
        private class TerrainGeneratorEditor : Editor
        {
            #region Variables

            private TerrainGenerator terrainGenerator = null;
            
            private GUIContent buttonTitleGC      = null;
            private GUIContent autoUpdateToogleGC = null;
            
            private SerializedProperty terrainPreviewDefinitionSP = null;

            private Editor terrainPreviewDefinitionEditor = null;

            private bool autoUpdate = true;

            #endregion Variables

            #region Properties

            public Editor TerrainPreviewDefinitionEditor
            {
                get
                {
                    if (terrainGenerator != null && terrainPreviewDefinitionEditor == null)
                    {
                        terrainPreviewDefinitionEditor = CreateEditor(terrainGenerator.definition);
                    }
                    
                    return terrainPreviewDefinitionEditor;
                }
            }

            #endregion Properties

            #region Unity Editor Methods

            private void OnEnable()
            {
                terrainGenerator = (target as TerrainGenerator);
                buttonTitleGC = new GUIContent("Regenerate");
                autoUpdateToogleGC = new GUIContent("Auto update");
                terrainPreviewDefinitionSP = serializedObject.FindProperty("definition");
            }

            public override void OnInspectorGUI()
            {
                EditorGUI.BeginChangeCheck();
                
                DrawDefaultInspector();

                if (terrainPreviewDefinitionSP.objectReferenceValue != null)
                {
                    GUILayout.BeginVertical();
                
                    TerrainPreviewDefinitionEditor.DrawDefaultInspector();
                    
                    GUILayout.EndVertical();
                }
                
                autoUpdate = GUILayout.Toggle(autoUpdate, autoUpdateToogleGC);
                
                if (EditorGUI.EndChangeCheck())
                {
                    if (autoUpdate)
                    {
                        terrainGenerator.GenerateAndDisplayTerrain();
                    }
                }

                if (!autoUpdate && GUILayout.Button(buttonTitleGC))
                {
                    terrainGenerator.GenerateAndDisplayTerrain();
                }
            }

            #endregion Unity Editor Methods
        }
    }
}