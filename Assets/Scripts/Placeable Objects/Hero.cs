using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {

    public enum Type
    {
        knight, priest, NONE
    }

    #region Vars
    public Vector2Int posInGrid;
    private Type hero_type;
    private int lifepoints;
    private int actHP;
    public bool isInFight = false;
    private int attack;
    private Vector3 reach;
    private float attackspeed;
    private Rigidbody rigidbod;
    public static readonly int[] allowedFieldIDs = new int[6] {5,6,7,8, 15, 16};
    private Color standard_color;
    #endregion

    // Use this for initialization
    void Start () {
        hero_type = SetHeroType(this.gameObject.name.ToLower());
        ReadHeroType();
        actHP = lifepoints;
        standard_color = this.gameObject.GetComponent<Renderer>().material.color;
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.GetComponent<BoxCollider>().size += reach;
        this.gameObject.GetComponent<BoxCollider>().bounds.size.Scale(reach);
        rigidbod = this.gameObject.AddComponent<Rigidbody>();
        rigidbod.isKinematic = true;
        rigidbod.useGravity = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (isInFight)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material.color = standard_color;
        }
        if (actHP <= 0)
        {
            foreach (KeyValuePair<Vector2Int, GameObject> item in MapManager.GetGrid())
            {
                if (posInGrid == item.Key)
                {
                    item.Value.gameObject.GetComponent<Field>().isOccupied = false;
                }
            }
            Destroy(this.gameObject);
        }
    }

    #region SetHeroType
    private Type SetHeroType(string hero_name)
    {
        Type type;
        if (hero_name.Contains("knight"))
        {
            type = Type.knight;
        }
        
        else if(hero_name.Contains("priest"))
        {
            type = Type.priest;
        }
        else
        {
            type = Type.NONE;
        }
        return type;
    }
    #endregion

    #region ReadHeroType
    private void ReadHeroType()
    {
        switch (hero_type)
        {
            case Type.knight:
                SetValues(DatabaseController.ReadAttributesFromTable("Hero", "Knight"));
                break;
            case Type.priest:
                SetValues(DatabaseController.ReadAttributesFromTable("Hero", "Priest"));
                break;
            case Type.NONE:
                break;
            default:
                break;
        }
    }
    #endregion

    public void GetDamage(int damage)
    {
        actHP -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.name.Contains("Enemy"))
            {
                StartCoroutine(AttackTarget(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            StopCoroutine(AttackTarget(other));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            if (other.name.Contains("Enemy"))
            {
                if (actHP <= 0)
                {
                    other.gameObject.GetComponent<Enemy>().isInFight = false;
                }
            }
        }
    }

    private IEnumerator AttackTarget(Collider other)
    {
        while (other != null)
        {
            other.gameObject.GetComponent<Enemy>().GetDamage(attack);
            yield return new WaitForSeconds(attackspeed);
        }
    }

    private void SetValues(List<string> content)
    {
        lifepoints = int.Parse(content[1]);
        attack = int.Parse(content[2]);
        attackspeed = float.Parse(content[3]);
        reach = new Vector3(float.Parse(content[4]), float.Parse(content[4]), float.Parse(content[4]));

    }

    private void SetValues(int lifepoints, int attack, Vector3 reach, float attackspeed)
    {
        this.lifepoints = lifepoints;
        this.attack = attack;
        this.reach = reach;
        this.attackspeed = attackspeed;
    }
}
