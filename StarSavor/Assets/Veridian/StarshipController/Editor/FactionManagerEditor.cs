using UnityEditor; // This namespace is essential for editor scripting
using UnityEngine;

namespace Veridian.Starship.Core
{

    /// <summary>
    /// A data class to hold the settings for waypoint generation.
    /// This keeps the FactionManager component clean from editor-only variables.
    /// </summary>
    [System.Serializable]
    public class WaypointGenerationSettings
    {
        [Tooltip("How many waypoints to generate around each team's home base.")]
        public int WaypointsPerBase = 5;

        [Tooltip("The minimum and maximum height (Y-axis) for the generated waypoints, relative to the base's position.")]
        public float MinHeight = 400f;
        public float MaxHeight = 1000f;

        [Tooltip("The maximum horizontal distance (radius) a waypoint can be from its base.")]
        public float MaxDistance = 5000f;

        [Tooltip("Controls the distribution of waypoints based on distance. X-axis (0 to 1) maps to distance (0 to MaxDistance). Y-axis (0 to 1) is the evaluated multiplier. A linear curve provides uniform distribution.")]
        public AnimationCurve DistributionCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }

    /// <summary>
    /// Creates a custom inspector for the FactionManager to add a waypoint generation tool.
    /// </summary>
    [CustomEditor(typeof(FactionManager))]
    public class FactionManagerEditor : Editor
    {
        private FactionManager _factionManager;
        private WaypointGenerationSettings _settings = new WaypointGenerationSettings();
        private bool _showGenerator = true;

        private void OnEnable()
        {
            _factionManager = (FactionManager)target;
        }

        public override void OnInspectorGUI()
        {
            // Draw the default inspector fields (for home bases, etc.)
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Add a foldable section for our custom tool
            _showGenerator = EditorGUILayout.Foldout(_showGenerator, "Waypoint Generation Tool", true);

            if (_showGenerator)
            {
                EditorGUILayout.HelpBox("This tool will clear existing waypoint lists and generate new waypoints around each team's assigned Home Bases.", MessageType.Info);

                _settings.WaypointsPerBase = EditorGUILayout.IntField("Waypoints Per Base", _settings.WaypointsPerBase);
                _settings.MinHeight = EditorGUILayout.FloatField("Minimum Height", _settings.MinHeight);
                _settings.MaxHeight = EditorGUILayout.FloatField("Maximum Height", _settings.MaxHeight);
                _settings.MaxDistance = EditorGUILayout.FloatField("Maximum Distance", _settings.MaxDistance);
                _settings.DistributionCurve = EditorGUILayout.CurveField("Distance Distribution", _settings.DistributionCurve);

                EditorGUILayout.Space();

                if (GUILayout.Button("Generate Team Waypoints"))
                {
                    GenerateAllWaypoints();
                }
            }
        }

        /// <summary>
        /// The core logic for generating waypoints for both teams.
        /// </summary>
        private void GenerateAllWaypoints()
        {
            // Get the lists from the FactionManager via SerializedObject to ensure Undo/Redo works
            SerializedProperty teamAWaypointsProp = serializedObject.FindProperty("TeamAWaypoints");
            SerializedProperty teamBWaypointsProp = serializedObject.FindProperty("TeamBWaypoints");
            SerializedProperty teamABasesProp = serializedObject.FindProperty("TeamABases");
            SerializedProperty teamBBasesProp = serializedObject.FindProperty("TeamBBases");

            // Generate waypoints for each team
            GenerateForTeam(Faction.TeamA, teamAWaypointsProp, teamABasesProp);
            GenerateForTeam(Faction.TeamB, teamBWaypointsProp, teamBBasesProp);

            Debug.Log("Waypoint generation complete!");
        }

        /// <summary>
        /// Generates waypoints for a single team.
        /// </summary>
        private void GenerateForTeam(Faction faction, SerializedProperty waypointsProp, SerializedProperty basesProp)
        {
            string containerName = $"{faction} Waypoints [Generated]";

            // Find and destroy the old container to ensure a clean generation
            GameObject oldContainer = GameObject.Find(containerName);
            if (oldContainer != null)
            {
                Undo.DestroyObjectImmediate(oldContainer);
            }

            // Create a new empty GameObject to hold the waypoints
            GameObject waypointContainer = new GameObject(containerName);
            Undo.RegisterCreatedObjectUndo(waypointContainer, "Create Waypoint Container");

            // Clear the manager's list before populating it
            waypointsProp.ClearArray();
            serializedObject.ApplyModifiedProperties();

            int waypointIndex = 0;

            for (int i = 0; i < basesProp.arraySize; i++)
            {
                Transform baseTransform = basesProp.GetArrayElementAtIndex(i).objectReferenceValue as Transform;
                if (baseTransform == null) continue;

                for (int j = 0; j < _settings.WaypointsPerBase; j++)
                {
                    // 1. Get a random value from 0 to 1
                    float randomValue = Random.value;

                    // 2. Use the curve to map the linear random value to a biased one
                    float radiusMultiplier = _settings.DistributionCurve.Evaluate(randomValue);
                    float finalRadius = radiusMultiplier * _settings.MaxDistance;

                    // 3. Get a random direction and height
                    float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                    float yPos = Random.Range(_settings.MinHeight, _settings.MaxHeight);

                    // 4. Calculate the final waypoint position
                    Vector3 offset = new Vector3(Mathf.Cos(randomAngle) * finalRadius, yPos, Mathf.Sin(randomAngle) * finalRadius);
                    Vector3 waypointPosition = baseTransform.position + offset;

                    // 5. Create the waypoint GameObject
                    GameObject waypoint = new GameObject($"{faction}_Waypoint_{waypointIndex++}");
                    Undo.RegisterCreatedObjectUndo(waypoint, "Create Waypoint");
                    waypoint.transform.position = waypointPosition;
                    waypoint.transform.SetParent(waypointContainer.transform);

                    // 6. Add the new waypoint to the FactionManager's list
                    waypointsProp.InsertArrayElementAtIndex(waypointsProp.arraySize);
                    waypointsProp.GetArrayElementAtIndex(waypointsProp.arraySize - 1).objectReferenceValue = waypoint.transform;
                }
            }

            // IMPORTANT: Apply changes to the SerializedObject to save them.
            serializedObject.ApplyModifiedProperties();
        }
    }
}