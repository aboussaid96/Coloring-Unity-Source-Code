using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Dreamteck.Splines;

public class Controller : MonoBehaviour
{
    public List<Vector3> PointsToBeFollow;
    public List<GameObject> SequenceOfLines;
    public List<GameObject> DarwingPanels;
    [Space(10)]
    public LineRenderer lineRenderer;
    [Space(10)]
    public GameObject SampleLineRender;
    public GameObject colorselection;
    public GameObject PaintBrush;
    public GameObject Mesh;
    public GameObject EndImage;
    public GameObject StartImage;
    public GameObject EndimageDisplay;
    public GameObject StartImageDisplay;
    [Space(10)]
    public bool Coloring;
    public bool line;
    public bool Reached;
    [Space(10)]
    public int count;
    public int PaintLineCount;
    public int DrawingpaCount;
    [Space(10)]
    public float Speed;
    public static Controller Instance;
    public void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }
    void Start()
    {
        count = 0;
        PaintLineCount = 0;
        DrawingpaCount = 0;
        line = true;
        colorselection.SetActive(false);
        PutADrawLine();
    }
    void Update()
    {
        if (line)
        {
            if (Input.GetMouseButton(0))
            {
                if (count < PointsToBeFollow.Count)
                {
                    movement();
                }
                else
                {
                    Reached = true;
                }
            }
        }
        if (Coloring)
        {
            if (Input.GetMouseButton(0))
            {
                if (count < PointsToBeFollow.Count)
                {
                    movement();
                }
                else
                {
                    if (DrawingpaCount < DarwingPanels.Count-1)
                    {
                        DarwingPanels[DrawingpaCount].transform.GetChild(1).GetComponent<P3dPaintableTexture>().enabled = false;
                        DrawingpaCount++;
                        Coloring = false;
                        transform.DOMove(new Vector3(0.56f, 1.22f, -1.25f), 0.45f);
                        StartCoroutine(putAlineWithEffect());
                    }
                    else
                    {
                        Coloring = false;
                        Ui.Instance.LevelCompleted();
                        if (AudioManager.instance)
                        {
                            AudioManager.instance.Play("WinSound");
                        }
                        if(Particaleffect.instance)
                        {
                            Particaleffect.instance.playpop();
                        }
                        if(Controller.Instance)
                        {
                            Controller.Instance.transform.gameObject.SetActive(false);
                        }
                    }
                }
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (line && Reached)
            {
                UndoingPaintingAbility();
                if (PaintLineCount < SequenceOfLines.Count)
                {
                    StartCoroutine(MoveBackOnNextline());
                }
                else
                {
                    StartCoroutine(putAlineWithEffect());
                }
            }
            else
            {
                PointsToBeFollow.Insert(count, transform.position);
            }
            transform.DOMove(new Vector3(0.56f, 1.22f, -1.25f), 0.45f);
        }
    }

    public IEnumerator MoveBackOnNextline()
    {
        transform.DOMove(new Vector3(0.56f, 1.22f, -1.25f), 0.45f);
        yield return new WaitForSeconds(0.59f);
        PaintLineCount++;
        PutADrawLine();
    }
    public void movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, PointsToBeFollow[count], Speed * Time.deltaTime);
        if (transform.position == PointsToBeFollow[count])
        {
            if (count < PointsToBeFollow.Count)
            {
                count++;
            }
        }
    }
    public void UndoingPaintingAbility()
    {
        line = false;
        count = 0;
        Destroy(EndimageDisplay);
        Destroy(StartImageDisplay);
        GameObject Temp = SequenceOfLines[PaintLineCount].gameObject;
        Temp.transform.GetChild(0).gameObject.SetActive(false);
        Temp.transform.GetChild(1).GetComponent<P3dPaintableTexture>().enabled = false;
    }
    public void PutADrawLine()
    {
        if (PaintLineCount < SequenceOfLines.Count)
        {
            GameObject Temp = SequenceOfLines[PaintLineCount].gameObject;
            Temp.SetActive(true);
            Sprite A = Temp.GetComponent<SpriteRenderer>().sprite;
            Texture Present = Temp.GetComponent<SpriteRenderer>().sprite.texture;
            Temp.GetComponent<SpriteRenderer>().enabled = false;
            Temp.transform.GetChild(0).gameObject.SetActive(true);
            PointsToBeFollow = Temp.transform.GetChild(0).GetComponent<linemakeing>().points;
            P3dPaintSphere PS = PaintBrush.GetComponent<P3dPaintSphere>();
            PS.blendMode.Texture = Present;
            PS.blendMode.Color = Color.black;
            GameObject temp2 = Temp.transform.GetChild(1).gameObject;
            temp2.transform.localScale = A.bounds.size;
            temp2.transform.localPosition = new Vector3(0, 0, 0);
            temp2.gameObject.SetActive(true);
            temp2.GetComponent<MeshRenderer>().material.SetTexture("Albedo (RGB) Alpha (A)", Present);
            line = true;
            Reached = false;
            EndimageDisplay = Instantiate(EndImage, transform.position, Quaternion.identity);
            EndimageDisplay.transform.position = PointsToBeFollow[PointsToBeFollow.Count - 1];
            StartImageDisplay = Instantiate(StartImage, transform.position, Quaternion.identity);
            StartImageDisplay.transform.position = PointsToBeFollow[0];
        }
        else
        {
            StartCoroutine(putAlineWithEffect());
        }
    }
    IEnumerator putAlineWithEffect()
    {
        transform.DOMove(new Vector3(0.56f, 1.22f, -1.25f), 0.45f);
        Mesh.GetComponent<MeshRenderer>().materials[1].color = Color.black;
        yield return new WaitForSeconds(1.5f);
        DarwingPanels[DrawingpaCount].GetComponent<SpriteRenderer>().enabled=true;
        StartCoroutine(blink());
        colorselection.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }
    public void SelectColor()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("pop");
        }
        count = 0;
        GameObject imagegameobject = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).transform.gameObject;
        PaintBrush.SetActive(true);
        P3dPaintSphere PS = PaintBrush.GetComponent<P3dPaintSphere>();
        PS.Scale = new Vector3(0.35f, 0.35f, 0.35f);
        PS.blendMode.Color = imagegameobject.GetComponent<Image>().color;
        Mesh.GetComponent<MeshRenderer>().materials[1].color = imagegameobject.GetComponent<Image>().color;
        PS.blendMode.Texture = DarwingPanels[DrawingpaCount].GetComponent<SpriteRenderer>().sprite.texture;
        GameObject temp = DarwingPanels[DrawingpaCount].transform.GetChild(0).gameObject;
        temp.gameObject.SetActive(true);
        DarwingPanels[DrawingpaCount].GetComponent<SpriteRenderer>().enabled = false;
        DarwingPanels[DrawingpaCount].transform.GetChild(1).gameObject.SetActive(true);
        DarwingPanels[DrawingpaCount].transform.GetChild(1).transform.localScale = DarwingPanels[DrawingpaCount].GetComponent<SpriteRenderer>().sprite.bounds.size;
        DarwingPanels[DrawingpaCount].transform.GetChild(1).transform.localPosition = new Vector3(0, 0, 0);                                    
        temp.GetComponent<LineRenderer>().enabled = false;
        colorselection.SetActive(false);
        PointsToBeFollow = temp.GetComponent<linemakeing>().points;
        Coloring = true;
                                                         
    }
    IEnumerator blink()
    {
        Debug.Log("opop");
        DarwingPanels[DrawingpaCount].GetComponent<SpriteRenderer>().DOColor(new Color(0.8f, 0.8f, 0.8f, 1), 0.5f);
        yield return new WaitForSeconds(0.55f);
        DarwingPanels[DrawingpaCount].GetComponent<SpriteRenderer>().DOColor(new Color(0.9f, 0.9f, 0.9f, 1), 0.5f);
        yield return new WaitForSeconds(0.55f);
        StartCoroutine(blink());
    }

    

}
