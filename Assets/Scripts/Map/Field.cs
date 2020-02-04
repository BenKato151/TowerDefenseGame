    // Changed solution from my lecturer
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public int fieldid;
    public bool isOccupied = false;
    public bool hover;
    public Vector2Int position;
    public bool isSelected;

    // da fields monobehaviours sind, könnte man hier auch gut ein MB als Typ nehmen.
    public static List<Field> fields = new List<Field>();

    //SN: habe es static gemacht, denn es soll nur eines geben. Ausgangszustand "null", weil wir kein zusätzliches Objekt in der Scene brauchen.
    public static GameObject selectedField; 
    //SN:
    private Collider collisionBox;
    private Rigidbody rigidbod;

    private Color st_color;
    private Vector3 st_fieldpos;
    private Vector3 st_fieldsize;

    // Use this for initialization
    private void Start()
    {
        st_fieldpos = this.gameObject.transform.position;
        st_fieldsize = this.gameObject.transform.localScale;

        //SN: Ja, man braucht beides um Clicks zu bekommen!
        collisionBox = this.gameObject.AddComponent<BoxCollider>();
        collisionBox.GetComponent<BoxCollider>();
        rigidbod = this.gameObject.AddComponent<Rigidbody>();
        rigidbod.isKinematic = true;
        rigidbod.useGravity = false;

        // wichtig, wenn man nachher die Übersicht behalten will
        fields.Add(this); 
        st_color = this.gameObject.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    private void Update()
    {
        if (hover && !PhaseManager.isInFight)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        if (PhaseManager.isInFight)
        {
            this.gameObject.GetComponent<Renderer>().material.color = st_color;
            this.gameObject.transform.position = st_fieldpos;
            this.transform.localScale = st_fieldsize;
        }
    }

    // die Implementation der Selektion und des Hoverns ist Ihre Sache, aber das Design sollte ein Grafiker machen.
    // Wir können dafür Objekte anzeigen, Lichteffekte nehmen oder weiß der Geier.

    // public, weil ich es bei dem anderen, das jetzt nicht mehr selektiert ist, ausschalten will
    public void ShowSelection(bool isShowed) 
    {
        if (isShowed)
        {
            //this.gameObject.transform.localScale = 2.0f * new Vector3(this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }
        else
        {
            this.gameObject.transform.localScale = st_fieldsize;
        }

        // NUR DES BEISPIELS WEGEN: Die Liste der Felder kann ja was machen:
        foreach (Field item in fields)
        {
            if (item.isOccupied)
            {
                item.gameObject.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    private void ShowHover(bool isHovering)
    {
        if (isSelected)
        {
            return;
        }
        if (isHovering)
        {
            this.gameObject.transform.localScale = 1.2f * new Vector3(this.gameObject.transform.localScale.x, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }
        else
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnMouseEnter()
    {
        ShowHover(true);
    }
    private void OnMouseExit()
    {
        ShowHover(false);
    }
    //Selection
    private void OnMouseDown()
    {
        // altes deselektieren, falls es existiert
        if (selectedField != null)
        {
            selectedField.GetComponent<Field>().ShowSelection(false);
            selectedField.GetComponent<Field>().isSelected = false;
        }
        selectedField = this.gameObject;
        isSelected = true;
        ShowSelection(true);

        if (BuyItems.isBuying)
        {
            List<BuyItems> buyItems_tmp = GameObject.Find("Shop").GetComponentsInChildren<BuyItems>().ToList();
            
            foreach (BuyItems item in buyItems_tmp)
            {
                if (item.whichItem)
                {
                    item.SetItem(this);
                    item.whichItem = false;
                }
            }

            BuyItems.isBuying = false;
            buyItems_tmp.Clear();
        }
    }
}
