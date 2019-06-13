using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityEngine {

    public class MultiSpawner : MonoBehaviour {

        [Header("Spawn behavior")]
        public GameObject Prefab;
        public int TotalClones = 10;
        [Tooltip("This template string will be used for the names of all prefab clones. Use '{1}' as a placeholder for the group index, and '{0}' as a placeholder for the copy index within the group.")]
        public string CloneNameTemplate = "clone-{1}-{0}";
        public Transform CloneParent;
        public bool SpawnOnStart = true;

        [Header("Clone Arrangement")]
        [Tooltip("If true, then clones will be arranged in a circle of radius " + nameof(CircleRadius) + ". Otherwise, clones will be arranged in a line with an offset of " + nameof(OffsetBetween) + " between each.")]
        public bool EncircleClones = false;
        [ShowIf(nameof(dontEncircle))]
        public Vector3 OffsetBetween = Vector3.up;
        [ShowIf(nameof(EncircleClones))]
        public float CircleRadius = 1f;
        [ShowIf(nameof(EncircleClones))]
        public bool RotateEncircledClones = true;

        [Header("Group Clones")]
        [Tooltip("If true, then each group of " + nameof(NumToGroupBy) + " clones will be parented to a new 'group' GameObject. Otherwise, clones will appear in a flat hierarchy under the " + nameof(CloneParent) + ". Each group GameObject will receive the name, tag, and layer specified below.")]
        public bool GroupClones = false;
        [ShowIf(nameof(GroupClones))]
        public int NumToGroupBy = 1;
        [ShowIf(nameof(GroupClones))]
        public Vector3 OffsetBetweenGroups = Vector3.up;
        [ShowIf(nameof(GroupClones))]
        public string GroupBaseName = "group-{0}";
        [ShowIf(nameof(GroupClones))]
        public string GroupTag = "Untagged";
        [ShowIf(nameof(GroupClones))]
        public LayerMask GroupLayer;

        private bool dontEncircle() => !EncircleClones;

        private void Awake() {
            Assert.IsNotNull(Prefab, this.GetAssociationAssertion(nameof(Prefab)));
        }
        private void Start() {
            if (SpawnOnStart)
                Spawn();
        }

        public void Spawn() {
            if (GroupClones)
                spawnGrouped();
            else
                spawnUngrouped();
        }
        private void spawnGrouped() {
            int numGrps = Mathf.CeilToInt(TotalClones / (float)NumToGroupBy);
            int copyNum = 0;
            float baseGrpRot = 2f * Mathf.PI / NumToGroupBy;
            for (int g = 0; g < numGrps; ++g) {
                var grpObj = new GameObject(string.Format(GroupBaseName, g)) {
                    tag = GroupTag,
                    layer = GroupLayer.value
                };
                grpObj.transform.parent = CloneParent;
                grpObj.transform.localPosition = g * OffsetBetween;
                grpObj.transform.up = OffsetBetweenGroups;
                for (int c = 0; c < NumToGroupBy && copyNum < TotalClones; ++c) {
                    GameObject cloneObj = Instantiate(Prefab, grpObj.transform);
                    cloneObj.name = string.Format(CloneNameTemplate, g, copyNum);
                    spawnClone(cloneObj, baseGrpRot, c);
                    ++copyNum;
                }
            }
        }
        private void spawnUngrouped() {
            float baseRot = 2f * Mathf.PI / TotalClones;
            for (int c = 0; c < TotalClones; ++c) {
                GameObject cloneObj = Instantiate(Prefab, CloneParent);
                cloneObj.name = string.Format(CloneNameTemplate, c);
                spawnClone(cloneObj, baseRot, c);
            }
        }
        private void spawnClone(GameObject clone, float baseRotationRadians, int index) {
            if (EncircleClones) {
                float rads = index * baseRotationRadians;
                var localPos = new Vector3(Mathf.Cos(rads), 0f, Mathf.Sin(rads));
                clone.transform.localPosition = CircleRadius * localPos;
                if (RotateEncircledClones) {
                    float degs = Mathf.Rad2Deg * rads;
                    clone.transform.localRotation = Quaternion.Euler(-degs * Vector3.up);
                }
            }
            else
                clone.transform.localPosition = index * OffsetBetween;
        }

    }

}
