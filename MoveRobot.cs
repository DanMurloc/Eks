using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRobot : MonoBehaviour
{
   private float moveSp= 5f; // Скорость движения
   private float rotationSp = 100f; // Скорость поворота 



    void Update()
    {
        HandleInput();
    }
    private void HandleInput()
    {

        RotateAndMoveWithMouse();
        // Проверяем, если нажата кнопка "R", то создаем новую сферу
        if (Input.GetKeyDown(KeyCode.R))
        {
            CastGroundBall();
        }
        if (Input.GetKey(KeyCode.F))
        {
            Trais();
        }
    }

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public Camera cameras;
    public float speedRotate = 100f; 
    [SerializeField] float moveSpeed = 0f;


    private void RotateAndMoveWithMouse()
    {



        if (Input.GetMouseButton(1))
        {
            if (moveSpeed < 5)
            {
                moveSpeed += 0.1F;
            }
            else
            {
               
            }
           
 
            Vector3 mousePos = Input.mousePosition;

            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));
 
            Vector3 direction = mousePos - transform.position;

            direction.y = 0f;

    
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            rb.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * moveSpeed);
        }
        else
        {


        }

        if (!Input.GetMouseButton(1) &&moveSpeed>0)
        {
            moveSpeed--;
        }
        else
        {
           
        }
    }

    public enum ObjectType { Prefab, Cube, Sphere }

    public ObjectType objectType = ObjectType.Sphere;
    public GameObject prefab;
    public Mesh objectMesh;
    public Material objectMaterial;
    
    public float rotationSpeed = 5f;
    public string myTag2 = "MyObject";
    private void Trais()
    {
        float lifeTime = 10f;
        Vector3 scale = new Vector3(1f, 0.2f, 0.2f);
        Vector3 currentPosition = transform.position;
        GameObject obj = null;
        obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.GetComponent<Renderer>().material = objectMaterial;
        //obj.transform.position = spawnPosition + Vector3.up * height;
        obj.transform.rotation = transform.rotation;
        obj.transform.position = transform.position-new Vector3(0, 0F,0);
        obj.transform.localScale = scale;
        // Удаляем компонент BoxCollider
        Destroy(obj.GetComponent<BoxCollider>());

        Destroy(obj, lifeTime);

    }

    private void CastGroundBall()
    {
        //Vector3 scale = Vector3.one; 
        Vector3 scale = new Vector3(0.4f, 0.4f, 0.4f); 
        Vector3 currentPosition = transform.position;
        float speedBall = 10f; 
        float lifeTime = 10f; 
        float height = 1f; 
        //string objectTag = "grounBall"; 
        Vector3 customPosition = currentPosition;
        Vector3 spawnPosition = customPosition + new Vector3(0, 0, 0.5F);
 
        GameObject obj = null;

        Vector3 mousePos = Input.mousePosition;

        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));
    
        Vector3 direction = mousePos - transform.position;

        direction.y = 0f;



        switch (objectType)
        {
            case ObjectType.Prefab:
                obj = new GameObject("MyObject");
                obj.AddComponent<MeshFilter>().mesh = objectMesh;
                obj.AddComponent<MeshRenderer>().material = objectMaterial;
                //obj.transform.position = spawnPosition + Vector3.up * height;
                obj.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
                obj.transform.position = transform.position + transform.forward * Time.fixedDeltaTime * moveSpeed * 2 + Vector3.up * height;
                break;
            case ObjectType.Cube:
                obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.GetComponent<Renderer>().material = objectMaterial;
                //obj.transform.position = spawnPosition + Vector3.up * height;
                obj.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
                obj.transform.position = transform.position + transform.forward * Time.fixedDeltaTime * moveSpeed * 2 + Vector3.up * height;

                break;
            case ObjectType.Sphere:
                obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                obj.GetComponent<Renderer>().material = objectMaterial;
                //obj.transform.position = spawnPosition + Vector3.up * height;
                obj.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
                obj.transform.position = transform.position + transform.forward * Time.fixedDeltaTime * moveSpeed * 2 + Vector3.up * height;
                break;
              
        }

        if (obj != null)
        {
            //obj.tag = objectTag;

            obj.transform.localScale = scale;

            
            Rigidbody objRb = obj.GetComponent<Rigidbody>();
            if (objRb == null)
            {
                objRb = obj.AddComponent<Rigidbody>();
            }
            objRb.useGravity = false;
            objRb.mass = 0;
           
            objRb.velocity = transform.forward * speedBall;
      
            obj.tag = myTag2;
        
            Destroy(obj, lifeTime);
      
            obj.AddComponent<ObjectDestroyer>();
        }
    }
}

public class ObjectDestroyer : MonoBehaviour
{
    public string otherObjectTag = "Wall";
    public Material objectMaterial;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(otherObjectTag))
        {
            // Вычисляем расстояние по осям X и Z между объектами
            float distanceX = Mathf.Abs(transform.position.x - collision.transform.position.x);
            float distanceZ = Mathf.Abs(transform.position.z - collision.transform.position.z);

            // Вычисляем общее расстояние между объектами
            float totalDistance = Vector3.Distance(transform.position, collision.transform.position);
            //gameObject.transform.position = collision.transform.position;

            // Выводим результаты
            Debug.Log("Total Distance to wall: " + totalDistance);
            Debug.Log("Distance to wall (X): " + distanceX);
            Debug.Log("Distance to wall (Z): " + distanceZ);

            Vector3 scale = new Vector3(0.2f, 0.2f, 0.2f);
            Vector3 currentPosition = transform.position;
            GameObject obj = null;
            obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.GetComponent<Renderer>().material = objectMaterial;
            //obj.transform.position = spawnPosition + Vector3.up * height;
            obj.transform.rotation = transform.rotation;
            obj.transform.position = transform.position;
            obj.transform.localScale = scale;
            Destroy(gameObject);
        }
    }
}
