
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SA
{
    public class GridManager : MonoBehaviour
    {
        public Vector3 extents;
        
        [SerializeField] private float xzScale, yScale;
        private Node[,] grid;
        private int maxX, maxZ, maxY;
        private Vector3 minPos;
        private List<Vector3> nodeViz;


        // We initialize some variables.
        private void Awake ()
        {
            nodeViz = new List<Vector3> ();
            extents = new Vector3 (0.8f, 0.8f, 0.8f);
            xzScale = 1.5f;
            yScale = 2;

            ReadLevel ();
        }


        // Update is called once per frame.
        private void Update ()
        {

        }


        //
        private void OnDrawGizmos ()
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < nodeViz.Count; i += 1)
            {
                Gizmos.DrawWireCube (nodeViz[i], extents);
            }
        }


        //
        private void ReadLevel () 
        {
            GridPosition[] gridPos = GameObject.FindObjectsOfType<GridPosition> ();
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minZ = minX;
            float maxZ = maxX;

            for (int i = 0; i < gridPos.Length; i += 1) 
            {
                Transform t = gridPos[i].transform;

                if (t.position.x < minX) 
                {
                    minX = t.position.x;
                }
                if (t.position.x > maxX) 
                {
                    maxX = t.position.x;
                }
                if (t.position.z < minZ) 
                {
                    minX = t.position.z;
                }
                if (t.position.z > maxZ) 
                {
                    maxZ = t.position.z;
                }
            }

            int posX = Mathf.FloorToInt ((maxX - minX) / xzScale);
            int posZ = Mathf.FloorToInt ((maxZ - minZ) / xzScale);

            minPos = Vector3.zero;
            minPos.x = minX;
            minPos.z = minZ;

            CreateGrid (posX, posZ);
        }


        //
        private void CreateGrid (int posX, int posZ) 
        {
            grid = new Node[posX, posZ];

            for (int x = 0; x < posX; x += 1) 
            {
                for (int z = 0; z < posZ; z += 1) 
                {
                    Node n = new Node ();

                    n.x = x;
                    n.z = z;

                    Vector3 transformPos = minPos;

                    transformPos.x += x * xzScale;
                    transformPos.z += z * xzScale;
                    n.worldPos = transformPos;
                    grid[x, z] = n;

                    nodeViz.Add (n.worldPos);
                }
            }
        }
    }
}*/