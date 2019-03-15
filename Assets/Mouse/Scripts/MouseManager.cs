using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridSystem;

namespace MouseSystem
{
    public class MouseManager : MonoBehaviour
    {
        public static MouseManager instance;

        public GameObject mouseMarkerPrefab;

        private GameObject mouseMarker;


        void Start()
        {
            if (instance == null) {
                instance = this;
            }
            else if (instance != this) {
                Debug.LogWarning("Tried to instantiate additional Grid3D");
                Destroy(gameObject);
            }

            mouseMarker = Instantiate(mouseMarkerPrefab, transform);
        }

    }
}
