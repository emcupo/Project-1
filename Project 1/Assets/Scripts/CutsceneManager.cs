using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _cameras;
    private Queue<GameObject> _camerasQueue = new Queue<GameObject>();

    private GameObject _previousCam;

    [SerializeField] private float _sceneTime = 3f;
    private WaitForSeconds _timer;

    [SerializeField] private UnityEvent _OnFinish;

    private bool _autoPlay = false;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("AutoPlay"))
            _autoPlay = PlayerPrefs.GetInt("AutoPlay") != 0 ? true : false;

        _timer = new WaitForSeconds(_sceneTime);

        queueCams();
        nextCamera();
    }

    private void Start()
    {
        if (_autoPlay)
            StartCoroutine(AutoSwitch());
    }

    private void queueCams()
    {
        foreach (GameObject cam in _cameras)
        {
            cam.SetActive(false);
            _camerasQueue.Enqueue(cam);
        }
    }

    private void nextCamera()
    {
        if (_camerasQueue.Count != 0)
        {
            GameObject cam = _camerasQueue.Dequeue();
            if (_previousCam != null)
                _previousCam.SetActive(false);
            cam.SetActive(true);
            _previousCam = cam;
        }
    }

    private IEnumerator AutoSwitch()
    {
        yield return _timer;
        nextCamera();
        if (_camerasQueue.Count != 0)
            StartCoroutine(AutoSwitch());
    }

    private void OnSubmit()
    {
        if (_camerasQueue.Count != 0)
        {
            nextCamera();
            if (_autoPlay)
            {
                StopAllCoroutines();
                StartCoroutine(AutoSwitch());
            }
        }
        else
        {
            _OnFinish?.Invoke();
        }
    }
}
