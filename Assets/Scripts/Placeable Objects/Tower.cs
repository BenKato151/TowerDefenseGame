using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: perma Upgrade for all NEW Towers too
public class Tower : MonoBehaviour {
    public enum Type
    {
        Priest, Kaserne, None
    }

    #region Vars
    public static List<Tower> towers = new List<Tower>();
    public static int allowedFieldID = 9;
    public Vector2Int posInGrid;

    public int placedFieldID;
    private Type tower_type;
    private int attack;
    private Vector3 reach;
    private float attackspeed;
    #endregion

    // Use this for initialization
    void Start () {
        tower_type = SetTowerType(this.gameObject.name.ToLower());
        ReadTowerType();
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.GetComponent<BoxCollider>().size += reach;
        this.gameObject.GetComponent<BoxCollider>().bounds.size.Scale(reach);
        this.gameObject.transform.rotation.eulerAngles.Set(-90, 0,0);
        this.gameObject.transform.Rotate(new Vector3(-90, 0, 0), Space.World);
        towers.Add(this);
    }

    // Update is called once per frame
    void Update () {

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.name.Contains("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>().isInFightWithTower = true;
                StartCoroutine(AttackTarget(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            other.gameObject.GetComponent<Enemy>().isInFightWithTower = false;
            StopCoroutine(AttackTarget(other));
        }
    }

    #region SetTowerType()
    private Type SetTowerType(string tower_name)
    {
        Type type;
        if (tower_name.ToString().Contains("priest"))
        {
            type = Type.Priest;
        }
        else if (tower_name.ToString().Contains("kaserne"))
        {
            type = Type.Kaserne;
        }
        else
        {
            type = Type.None;
        }
        return type;
    }
    #endregion

    #region ReadTowerType()
    private void ReadTowerType()
    {
        switch (tower_type)
        {
            case Type.Priest:
                SetValues(DatabaseController.ReadAttributesFromTable("Tower", "Priest"));
                break;
            case Type.Kaserne:
                SetValues(DatabaseController.ReadAttributesFromTable("Tower", "Kaserne"));
                break;
            case Type.None:
                break;
            default:
                break;
        }
    }
    #endregion

    private IEnumerator AttackTarget(Collider other)
    {
        while (other != null)
        {
            other.gameObject.GetComponent<Enemy>().GetDamage(attack);
            yield return new WaitForSeconds(attackspeed);
        }
    }

    public void SetValues(int attack, Vector3 reach, float attackspeed)
    {
        this.attack = attack;
        this.reach = reach;
        this.attackspeed = attackspeed;
    }

    private void SetValues(List<string> content)
    {
        attack = int.Parse(content[1]);
        reach = new Vector3(float.Parse(content[2]), float.Parse(content[2]), float.Parse(content[2]));
        attackspeed = float.Parse(content[3]);
    }
}
