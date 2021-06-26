using UnityEngine;

namespace GameModule.Class
{
    public class InputManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private GameObject _hitItem;

        #region MonoBehaviour Callbacks

        

        #endregion
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var maxDistance = 100.0f;
                if (Physics.Raycast(ray, out hit, maxDistance))
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);
                    Debug.Log("You selected the " + hit.transform.name); // ensure you picked right object
                    _hitItem = hit.transform.gameObject;
                    _hitItem.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.white);
                    if (_hitItem != null)
                    {
                        _hitItem.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                        _hitItem = null;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space)) //focus
            {
                // Camera.main.orthographic = !Camera.main.orthographic;
            }
        }
    }
}
