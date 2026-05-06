    using UnityEditor;
    using UnityEngine;
    using UnityEditor.SceneManagement;
    using System.Linq;
    using Theblueway.Core.Editor.SerializedInstanceMigrationTools;
using Theblueway.Core.Extensions;


namespace Theblueway.Core.Editor.Packages.com.blueutils.core.Editor.SerializedInstanceMigrationTools
{
    //todo: dry-run: only log what would be changed
    public static class MigrationRunner
    {
        [MenuItem("Tools/Ser Obj Migrations/Run All")]
        public static void RunAllMigrations()
        {
            MigrateScriptableObjects();
            MigratePrefabs();
            MigrateScenes();

            AssetDatabase.SaveAssets();
            Debug.Log("All migrations completed.");
        }

        private static void MigratePrefabs()
        {
            var guids = AssetDatabase.FindAssets("t:Prefab");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                var root = PrefabUtility.LoadPrefabContents(path);

                bool changed = MigrateGameObject(root);

                if (changed)
                {
                    PrefabUtility.SaveAsPrefabAsset(root, path);
                    Debug.Log($"Migrated prefab: {path}");
                }

                PrefabUtility.UnloadPrefabContents(root);
            }
        }

        private static void MigrateScenes()
        {
            var guids = AssetDatabase.FindAssets("t:Scene");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                var scene = EditorSceneManager.OpenScene(path);

                bool changed = false;

                foreach (var root in scene.GetRootGameObjects())
                {
                    if (MigrateGameObject(root))
                        changed = true;
                }

                if (changed)
                {
                    EditorSceneManager.MarkSceneDirty(scene);
                    EditorSceneManager.SaveScene(scene);
                    Debug.Log($"Migrated scene: {path}");
                }

                EditorSceneManager.CloseScene(scene, true);
            }
        }

        private static bool MigrateGameObject(GameObject root)
        {
            bool changed = false;

            var migratables = root.GetComponentsInChildren<MonoBehaviour>(true)
                                  .OfType<IMigratable>();

            foreach (var m in migratables)
            {
                if (m.Version < m.LatestVersion)
                {
                    Undo.RecordObject((Object)m, "Migration");

                    int oldVersion = m.Version;

                    m.Migrate(oldVersion);

                    m.Version = m.LatestVersion;

                    EditorUtility.SetDirty((Object)m);

                    var asMono = m as MonoBehaviour;

                    Debug.Log($"{m.GetType().Name} migrated {oldVersion} -> {m.LatestVersion} at {asMono.gameObject.HierarchyPath()}", (Object)m);

                    changed = true;
                }
            }

            return changed;
        }




        private static void MigrateScriptableObjects()
        {
            var guids = AssetDatabase.FindAssets("t:ScriptableObject");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

                if (asset is IMigratable migratable)
                {
                    if (migratable.Version < migratable.LatestVersion)
                    {
                        Undo.RecordObject(asset, "Migration");

                        int oldVersion = migratable.Version;

                        migratable.Migrate(oldVersion);
                        migratable.Version = migratable.LatestVersion;

                        EditorUtility.SetDirty(asset);

                        Debug.Log($"Migrated SO: {path} ({oldVersion} -> {migratable.LatestVersion})");
                    }
                }
            }
        }
    }
}
