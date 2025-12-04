using UnityEngine;

namespace EmptyGame.Misc
{
    [RequireComponent(typeof(RectTransform))]
    public class RectVolumeMesh : MonoBehaviour
    {
        [SerializeField] private float depth = 100f;
        [SerializeField] private Material volumeMaterial;

        private RectTransform _rectTransform;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            _meshFilter = GetComponent<MeshFilter>();
            if (_meshFilter == null)
                _meshFilter = gameObject.AddComponent<MeshFilter>();

            _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshRenderer == null)
                _meshRenderer = gameObject.AddComponent<MeshRenderer>();

            if (volumeMaterial != null)
                _meshRenderer.sharedMaterial = volumeMaterial;

            if (_mesh == null)
            {
                _mesh = new Mesh
                {
                    name = "RectVolumeMesh"
                };
                _meshFilter.sharedMesh = _mesh;
            }

            RebuildMesh();
        }

        protected void OnRectTransformDimensionsChange()
        {
            if (!isActiveAndEnabled) return;
            if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _mesh.name = "RectVolumeMesh";
                _meshFilter.sharedMesh = _mesh;
            }

            RebuildMesh();
        }

        private void RebuildMesh()
        {
            Rect rect = _rectTransform.rect;

            float xMin = rect.xMin;
            float xMax = rect.xMax;
            float yMin = rect.yMin;
            float yMax = rect.yMax;

            // 8 вершин коробки
            Vector3[] vertices = new Vector3[8];

            // Передняя грань (к камере)
            vertices[0] = new Vector3(xMin, yMin, 0); // bottom-left front
            vertices[1] = new Vector3(xMax, yMin, 0); // bottom-right front
            vertices[2] = new Vector3(xMax, yMax, 0); // top-right front
            vertices[3] = new Vector3(xMin, yMax, 0); // top-left front

            // Задняя грань (в глубину)
            vertices[4] = new Vector3(xMin, yMin, -depth); // bottom-left back
            vertices[5] = new Vector3(xMax, yMin, -depth); // bottom-right back
            vertices[6] = new Vector3(xMax, yMax, -depth); // top-right back
            vertices[7] = new Vector3(xMin, yMax, -depth); // top-left back

            int[] triangles = new int[36];

            int t = 0;

            // Front plane  (0,1,2,3)
            triangles[t++] = 0; triangles[t++] = 2; triangles[t++] = 1;
            triangles[t++] = 0; triangles[t++] = 3; triangles[t++] = 2;

            // back plane (4,5,6,7)
            triangles[t++] = 4; triangles[t++] = 5; triangles[t++] = 6;
            triangles[t++] = 4; triangles[t++] = 6; triangles[t++] = 7;

            // left plane (0,3,7,4)
            triangles[t++] = 0; triangles[t++] = 3; triangles[t++] = 7;
            triangles[t++] = 0; triangles[t++] = 7; triangles[t++] = 4;

            // right plane (1,2,6,5)
            triangles[t++] = 1; triangles[t++] = 6; triangles[t++] = 2;
            triangles[t++] = 1; triangles[t++] = 5; triangles[t++] = 6;

            // bottom plane (0,1,5,4)
            triangles[t++] = 0; triangles[t++] = 1; triangles[t++] = 5;
            triangles[t++] = 0; triangles[t++] = 5; triangles[t++] = 4;

            // top plane (3,2,6,7)
            triangles[t++] = 3; triangles[t++] = 2; triangles[t++] = 6;
            triangles[t++] = 3; triangles[t++] = 6; triangles[t++] = 7;

            _mesh.Clear();
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
        }
    }
}