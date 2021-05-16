using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Scritps

{
    public class GridCreation : MonoBehaviour
    {
        public GameObject mesh;
        public int mapZ, mapX;

        private void Start(){
            StartCoroutine(CreateGrid());
        }
        IEnumerator CreateGrid(){
            for(int z = 0; z < mapZ; z++){
                for(int x = 0; x < mapX; x++){
                    var meshPoint = Instantiate(mesh, new Vector3(x, 0, z), Quaternion.identity);
                    meshPoint.transform.SetParent(transform);
                    yield return new WaitForSeconds(.1f);
                }
            }
        }
    }
}
