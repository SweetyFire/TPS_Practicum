using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject _cubeToCreate;
    private string _cubeTag;

    private void Awake()
    {
        _cubeTag = _cubeToCreate.tag;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(GetMouseRay(), out var hit, 50f))
            {
                GameObject cube = Instantiate(_cubeToCreate, hit.point, Quaternion.identity);
                cube.SetActive(true);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(GetMouseRay(), out var hit, 50f))
            {
                if (hit.transform.CompareTag(_cubeTag))
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    private Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}
