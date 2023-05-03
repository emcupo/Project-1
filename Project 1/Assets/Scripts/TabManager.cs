using UnityEngine;

public class TabManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;

    private void Start()
    {
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
        tabs[0].SetActive(true);
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
