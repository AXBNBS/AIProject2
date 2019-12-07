
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeed;

    private float movementSpeed;
    public float movementTime;

    public float rotationAmount;

    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    [SerializeField] private UnitMovement[] selectedUnt;
    private LayerMask terrainMsk;
    private float xLim, zLim;

    public GameObject buildingMenu;


    // Start is called before the first frame update.
    private void Start ()
    {
        Grid grid = GameObject.FindObjectOfType<Grid> ();

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
        terrainMsk = LayerMask.GetMask ("Terrain");
        xLim = grid.gridWth / 2 * 1.05f;
        zLim = grid.gridHgt / 2 * 0.8f;
        selectedUnt = null;
    }


    // Update is called once per frame.
    private void Update ()
    {
        HandleMouseInput ();
        HandleMovementInput ();
    }


    void HandleMouseInput ()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
            if (newZoom.y < 20)
            {
                newZoom.y = 20;
                newZoom.z = -20;
            } else if (newZoom.y > 1000)
            {
                newZoom.y = 1000;
                newZoom.z = -1000;
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton (2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }

        if (Input.GetMouseButtonDown (0) == true && IsMouseOverUI () == false) 
        {
            //Plane plane = new Plane (Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

            //float entry;
            RaycastHit hit;

            if (Physics.Raycast (ray, out hit, Mathf.Infinity, terrainMsk, QueryTriggerInteraction.Collide) == true) 
            {
                Hexagon hex = hit.transform.GetComponent<Hexagon> ();
                //print (hex.name);

                if (hex != null) 
                {
                    if (selectedUnt == null && hex.GetIsBuilded() && hex.GetCity().GetCitySide()=="Blue")
                    {
                        buildingMenu.GetComponent<BuildingMenu>().readHexagonBuilding(hex);
                    }

                    if (selectedUnt == null)
                    {
                        if (hex.UnitsPlaced().Length != 0)
                        {
                            selectedUnt = hex.UnitsPlaced();
                            //print(selectedUnt[0].name);
                            if (!hex.GetIsBuilded() && selectedUnt != null && selectedUnt[0].tag != "Enemy")
                            {
                                buildingMenu.GetComponent<UnityMenu>().readHexagonUnity(hex);
                            }
                            else if (selectedUnt != null && selectedUnt[0].tag == "Enemy")
                            {
                                selectedUnt = null;
                            }
                        }
                    }
                    else if(!IsMouseOverUI())
                    {
                        if (selectedUnt[0].currentHex != hex && hex.hexagonType >= 0 && hex.GetCapacity () >= selectedUnt.Length && ((hex.UnitsPlaced().Length == 0) || (hex.UnitsPlaced()[0].stats.race == selectedUnt[0].stats.race))) 
                        {
                            foreach (Hexagon neighbour in hex.neighbours)
                            {
                                if (neighbour == selectedUnt[0].currentHex && (hex.GetHexagonType()==-1 || hex.GetHexagonType()==-2))
                                {
                                    buildingMenu.GetComponent<UnityMenu>().CloseWindow();

                                    return;
                                }
                            }

                            selectedUnt[0].FindPathTo (hex);

                            if (selectedUnt[0].reachedTrg == false) 
                            {
                                if (hex.presentUnt != 0)
                                {
                                    foreach (UnitMovement u in selectedUnt)
                                    {
                                        u.regroup = true;
                                    }
                                }
                                foreach (UnitMovement u in selectedUnt)
                                {
                                    u.targetHex = hex;
                                }
                            }

                            buildingMenu.GetComponent<UnityMenu>().readHexagonUnity(hex);
                        }
                        else if(selectedUnt[0].currentHex != hex && hex.UnitsPlaced()[0].gameObject.tag == "Enemy")
                        {
                            selectedUnt[0].FindPathTo (hex);
                            if (selectedUnt[0].reachedTrg == false)
                            {
                                foreach (Hexagon neighbour in hex.neighbours)
                                {
                                    if (neighbour == selectedUnt[0].currentHex)
                                    {
                                        selectedUnt[0].SendMessage ("Fight", hex);
                                    }
                                }

                                if (hex.presentUnt != 0)
                                {
                                    foreach (UnitMovement u in selectedUnt)
                                    {
                                        u.regroup = true;
                                        u.targetHex = hex;
                                    }
                                }
                            }

                            buildingMenu.GetComponent<UnityMenu>().readHexagonUnity (hex);
                        }
                        buildingMenu.GetComponent<UnityMenu>().CloseWindow();
                        //selectedUnt = null;
                    }
                }
            }
        }
    }


    private bool IsMouseOverUI ()
    {
        return EventSystem.current.IsPointerOverGameObject ();
    }


    void HandleMovementInput () //Control mediante teclado
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        } else
        {
            movementSpeed = normalSpeed;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
            if (newZoom.y < 20)
            {
                newZoom.y = 20;
                newZoom.z = -20;
            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
            if (newZoom.y > 1000)
            {
                newZoom.y = 1000;
                newZoom.z = -1000;
            }
        }

        if (Input.GetKeyDown (KeyCode.Tab) == true) 
        {
            buildingMenu.GetComponent<UnityMenu>().CloseWindow ();

            selectedUnt = null;
        }
        this.transform.position = Vector3.Lerp (transform.position, newPosition, Time.deltaTime * movementTime);
        this.transform.position = new Vector3 (Mathf.Clamp (this.transform.position.x, -xLim, +xLim), this.transform.position.y, Mathf.Clamp (this.transform.position.z, -zLim, +zLim));
        this.transform.rotation = Quaternion.Lerp (transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp (cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }


    public void SetNullSelectedUnit ()
    {
        selectedUnt = null;
    }


    public UnitMovement[] GetSelectedUnits ()
    {
        if (selectedUnt == null)
        {
            return null;
        }
        else
        {
            return selectedUnt;
        }
    }
}