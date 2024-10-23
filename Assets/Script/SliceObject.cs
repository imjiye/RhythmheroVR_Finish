using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask slicealbeLayer;

    public Material crossSectionMaterial;

    public GameObject TutoController;

    public float cutForce = 2000;
    public float sliceAngleThreshold = 120f;
    public int score = 10;
    public int combo = 1;

    private Vector3 oldPos;

    private Quaternion _rotationPrevious = Quaternion.identity;
    private Vector3 rotDelta_Prev = Vector3.one;
    Mesh SSB_Mesh;
    [Range(1, 128)]
    [Tooltip("Motion Blur Amount")]
    public int shutterSpeed = 4;

    [Range(1, 50)]
    [Tooltip("Motion Blur Samples")]
    public int Samples = 8;
    Queue<Quaternion> rotationQueue = new Queue<Quaternion>();
    public Material SSB_Material;
    [Range(-0.1f, 0.1f)]
    [Tooltip("Motion Blur Opacity")]
    public float alphaOffset;
    public AdvancedSettings advancedSettings;

    // Start is called before the first frame update
    void Start()
    {
        SSB_Mesh = GetComponent<MeshFilter>().mesh;
        SSB_Material.enableInstancing = advancedSettings.enableGPUInstancing;
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �����ӿ� ���� �� ó��
        ApplySpinBlur();

        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, slicealbeLayer);

        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;

            // ������ �Ӱ谪 �̻��� ���� �ڸ���
            if (Vector3.Angle(endSlicePoint.position - oldPos, hit.transform.up) > sliceAngleThreshold)
            {
                Slice(target);
            }
            oldPos = endSlicePoint.position; // ���� ��ġ ������Ʈ
        }
        else
        {
            Debug.Log("No hit detected between the slice points.");
        }
    }

    // �� ȿ���� ó���ϴ� �޼���
    private void ApplySpinBlur()
    {
        if (rotationQueue.Count >= shutterSpeed)
        {
            rotationQueue.Dequeue();
            // Second Dequeue to reduce queue size
            if (rotationQueue.Count >= shutterSpeed)
            {
                rotationQueue.Dequeue();
            }
        }

        rotationQueue.Enqueue(transform.rotation);

        if (Quaternion.Angle(transform.rotation, rotationQueue.Peek()) / shutterSpeed >= advancedSettings.AngularVelocityCutoff)
        {
            if (advancedSettings.unitLocalScale)
            {
                for (int i = 0; i <= Samples; i++)
                {
                    Graphics.DrawMesh(SSB_Mesh, transform.position, Quaternion.Lerp(rotationQueue.Peek(), transform.rotation, (float)i / (float)Samples), SSB_Material, 0, null, advancedSettings.subMaterialIndex);
                }
            }
            else
            {
                for (int i = 0; i <= Samples; i++)
                {
                    Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.Lerp(rotationQueue.Peek(), transform.rotation, (float)i / (float)Samples), transform.localScale);
                    Graphics.DrawMesh(SSB_Mesh, matrix, SSB_Material, 0, null, advancedSettings.subMaterialIndex);
                }
            }

            Color tempColor = new Color(SSB_Material.color.r, SSB_Material.color.g, SSB_Material.color.b, Mathf.Abs((2 / (float)Samples) + alphaOffset));
            SSB_Material.color = tempColor;
        }
        else
        {
            if (SSB_Material.color.a < 1)
            {
                Color tempColor = new Color(SSB_Material.color.r, SSB_Material.color.g, SSB_Material.color.b, 1);
                SSB_Material.color = tempColor;
            }
        }
    }

    public void Slice(GameObject target)
    {
        GameManager.instance.AddScore(score);
        GameManager.instance.AddCombo(combo);

        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        // �߸��� ���� ť�� ��ġ�� ����
        Vector3 originalPosition = target.transform.position;

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if (hull != null)
        {
            // �߸� ���� ����
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);

            // �߸� ������ ����
            SetupSlicedComponent(upperHull);
            SetupSlicedComponent(lowerHull);

            // ť�긦 �ı�
            Destroy(target);

            // �߸� ������ ��ġ�� ���� ť���� ��ġ�� ����
            upperHull.transform.position = originalPosition;
            lowerHull.transform.position = originalPosition;

            // �߸� ������ ���� �߰��Ͽ� �о��
            Vector3 pushDirection = planeNormal.normalized; // �߸� ���� ��� ����
            upperHull.GetComponent<Rigidbody>().AddForce(pushDirection * 100); // ���� ����
            lowerHull.GetComponent<Rigidbody>().AddForce(-pushDirection * 100); // �Ʒ��� ����

            // �߸� ������ 0.5�� �Ŀ� ����
            Destroy(upperHull, 0.3f);
            Destroy(lowerHull, 0.3f);

            Debug.Log("Slicing successful, hull is valid.");
        }
        else
        {
            Debug.Log("Slicing failed, hull is null");
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;

        // �߷� ��Ȱ��ȭ
        rb.useGravity = false;

        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }
}

[System.Serializable]
public class AdvancedSettings
{
    [Tooltip("[Optimization] Enables material's GPU Instancing property")]
    public bool enableGPUInstancing;
    [Tooltip("Index for objects with multiple materials")]
    public int subMaterialIndex = 0;
    [Tooltip("[Optimization] Check this box if the scale of your model is globalScale (1,1,1)")]
    public bool unitLocalScale = false;
    [Tooltip("[Optimization] Angular velocity threshold value before which the effects will not be rendered.")]
    public float AngularVelocityCutoff;
}
