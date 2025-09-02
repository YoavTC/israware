using UnityEngine;

public class RuntimeEditor : MonoBehaviour
{
    [SerializeField] private GameObject[] runtimeEditorObjects;

    private bool state;

    void Start()
    {
        state = false;
        ChangeState(state);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            state = !state;
            ChangeState(state);
        }
    }

    private void ChangeState(bool newState)
    {
        foreach (GameObject obj in runtimeEditorObjects)
        {
            obj.SetActive(newState);
        }
        state = newState;
    }
}
