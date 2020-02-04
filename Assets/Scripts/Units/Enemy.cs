using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public enum Type
    {
        Demon, None
    }

    #region Vars
    public Type enemyType;
    public bool isInFightWithTower;
    private int lifepoints;
    private int actHP;
    private int attack;

    private float speed;
    private float movementstep;
    private float attackspeed;

    private int moneydropped;
    public bool isInFight = false;

    private List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDist = 1.0f;
    private Color standard_color;
    #endregion

    // Use this for initialization
    void Start ()
    {
        #region Setup
        standard_color = this.gameObject.GetComponent<Renderer>().material.color;
        enemyType = SetType(this.gameObject.name.ToLower());
        ReadType();
        actHP = lifepoints;
        GameObject waypoint = GameObject.Find("Path_Object");
        foreach (Transform item in waypoint.GetComponentsInChildren<Transform>())
        {
            waypoints.Add(item);
        }
        waypoints.RemoveAt(0);
        targetWaypoint = waypoints[targetWaypointIndex];
        #endregion
    }

    // Update is called once per frame
    void Update ()
    {
        // All das hier muss im Update stehen
        #region Set Color
        if (isInFightWithTower || isInFight)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material.color = standard_color;
        }
        #endregion

        #region Set Stop in Fight
        if (isInFight)
        {
            movementstep = 0;
        }
        else
        {
            movementstep = speed * Time.deltaTime * 2;
        }
        #endregion

        if (GameManager.isDead)
        {
            Destroy(this.gameObject);
        }
        #region Check for Next Waypoint and Move // MoneyDrop and die
        if (targetWaypoint != null || this != null)
        {
            float dist = Vector3.Distance(this.transform.position, targetWaypoint.position);
            CheckDistance(dist);
        }


        this.transform.position = Vector3.MoveTowards(this.transform.position, targetWaypoint.position, movementstep);
        if (actHP <= 0)
        {
            MoneyManagement.DropMoney(moneydropped);
            Destroy(this.gameObject);
        }
        #endregion

    }

    private Type SetType(string enemy_name)
    {
        Type type;
        if (enemy_name.Contains("demon"))
        {
            type = Type.Demon;
        }
        else
        {
            type = Type.None;
        }
        return type;
    }
    
    #region Waypoints
    private void CheckDistance(float dist)
    {
        if (dist <= minDist)
        {
            targetWaypointIndex++;
            UpdateDistace();
        }
    }

    private void UpdateDistace()
    {
        if (targetWaypointIndex >= waypoints.Count && !GameManager.isDead && targetWaypoint != null)
        {
            GameManager.GetDamage(attack);
            Destroy(this.gameObject);
        }
        else
        {
            targetWaypoint = waypoints[targetWaypointIndex];
            this.transform.LookAt(targetWaypoint);
        }
    }
    #endregion

    private void ReadType()
    {
        switch (enemyType)
        {
            case Type.Demon:
                SetValues(DatabaseController.ReadAttributesFromTable("Enemy","Demon"));
                break;
            case Type.None:
                break;
            default:
                break;
        }
    }

    public void GetDamage(int damage)
    {
        actHP -= damage;
    }

    public void SetValues(int lifepoints, int attack, float speed, int moneydropped, float attackspeed)
    {
        this.lifepoints = lifepoints;
        this.attack = attack;
        this.speed = speed;
        this.moneydropped = moneydropped;
        this.attackspeed = attackspeed;
    }

    private void SetValues(List<string> content)
    {
        lifepoints = int.Parse(content[1]);
        attack = int.Parse(content[2]);
        speed = float.Parse(content[3]);
        moneydropped = int.Parse(content[4]);
        attackspeed = float.Parse(content[5]);
    }

    #region Fight
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.name.Contains("Hero"))
            {
                other.gameObject.GetComponent<Hero>().isInFight = true;
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
            if (other.name.Contains("Hero"))
            {
                isInFight = true;
                if (actHP <= 0)
                {
                    other.gameObject.GetComponent<Hero>().isInFight = false;
                    MoneyManagement.DropMoney(moneydropped);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private IEnumerator AttackTarget(Collider other)
    {
        while (other != null)
        {
            other.gameObject.GetComponent<Hero>().GetDamage(attack);
            yield return new WaitForSeconds(attackspeed);
        }
    }
    #endregion
}
