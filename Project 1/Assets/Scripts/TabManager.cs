using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;

    private void Awake()
    {
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(true);
        }
    }

    private void Start()
    {
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
    }

    public void Select(GameObject target)
    {
        if (target.activeSelf)
            return;

        foreach (var tab in tabs)
        {
            tab.SetActive(false);
        }
        target.SetActive(true);
    }
}
