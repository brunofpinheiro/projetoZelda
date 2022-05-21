using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCam : MonoBehaviour {
    public GameObject virtualCamera2;

    private void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag) {
            case "CamTrigger":
                 virtualCamera2.SetActive(true);
                 break;
        }
    }

    private void OnTriggerExit(Collider other) {
        switch (other.gameObject.tag) {
            case "CamTrigger":
                 virtualCamera2.SetActive(false);
                 break;
        }
    }
}
