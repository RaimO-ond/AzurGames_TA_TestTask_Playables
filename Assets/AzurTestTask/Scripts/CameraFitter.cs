using UnityEngine;

[ExecuteInEditMode]
public class CameraFitter : MonoBehaviour
{
    [Header("Настройки обзора")]
    public float targetWidth = 20f;
    public float bottomLimitY = -15f;
    public float topLimitY = 15f;

    [Header("Управление (только в Play Mode)")]
    public float scrollSensitivity = 15f;
    public float dragSensitivity = 1.0f;

    private Camera _cam;
    private Vector3 _lastMousePos;

    void Awake() => _cam = GetComponent<Camera>();

    void LateUpdate()
    {
        if (_cam == null) return;

        float distZ = Mathf.Abs(transform.position.z);
        
        float hFovRad = 2 * Mathf.Atan(targetWidth / (2 * distZ));
        float vFovRad = 2 * Mathf.Atan(Mathf.Tan(hFovRad / 2) / _cam.aspect);
        _cam.fieldOfView = vFovRad * Mathf.Rad2Deg;

        if (Application.isPlaying) HandleInput();

        ApplyRaycastClamping();
    }

    private void HandleInput()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            transform.Translate(Vector3.up * scroll * scrollSensitivity * Time.deltaTime, Space.World);
        }

        if (Input.GetMouseButtonDown(0)) _lastMousePos = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - _lastMousePos;
            float sensitivity = (_cam.fieldOfView / Screen.height) * dragSensitivity * 50f;
            transform.Translate(Vector3.down * delta.y * sensitivity * Time.deltaTime, Space.World);
            _lastMousePos = Input.mousePosition;
        }
    }

    private void ApplyRaycastClamping()
    {
        Ray bottomRay = _cam.ViewportPointToRay(new Vector3(0.5f, 0, 0));
        Ray topRay = _cam.ViewportPointToRay(new Vector3(0.5f, 1, 0));

        float tBottom = -bottomRay.origin.z / bottomRay.direction.z;
        float currentBottomY = bottomRay.origin.y + bottomRay.direction.y * tBottom;

        float tTop = -topRay.origin.z / topRay.direction.z;
        float currentTopY = topRay.origin.y + topRay.direction.y * tTop;

        float frustumHeightAtZ0 = currentTopY - currentBottomY;
        float limitsHeight = topLimitY - bottomLimitY;

        if (frustumHeightAtZ0 > limitsHeight)
        {
            transform.position += Vector3.up * (bottomLimitY - currentBottomY);
        }
        else
        {
            if (currentTopY > topLimitY)
            {
                transform.position -= Vector3.up * (currentTopY - topLimitY);
            }

            bottomRay = _cam.ViewportPointToRay(new Vector3(0.5f, 0, 0));
            tBottom = -bottomRay.origin.z / bottomRay.direction.z;
            currentBottomY = bottomRay.origin.y + bottomRay.direction.y * tBottom;

            if (currentBottomY < bottomLimitY)
            {
                transform.position += Vector3.up * (bottomLimitY - currentBottomY);
            }
        }
    }
}