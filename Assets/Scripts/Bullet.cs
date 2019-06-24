using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public Weapon.WeaponStats.AmmoType ammo;
    public Vector2 direction;
    public float velocity;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health hitHealth = collision.gameObject.GetComponent<Health>();
        if (hitHealth == null)
            return;
        else if (hitHealth.physMat == Health.physicalMaterial.INDESTRUCTABLE)
        {
            Destroy(gameObject);
        }
        else if (hitHealth.physMat == Health.physicalMaterial.FLESH)
        {
            hitHealth.ChangeHP(-damage);
            if (ammo == Weapon.WeaponStats.AmmoType.ARMORPIERCING)
            {
                damage /= 2;
                PenetrateObject(collision);
            }
            else
                Destroy(gameObject);
        }
        else if (hitHealth.physMat == Health.physicalMaterial.STONE)
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            Vector3 normal = collision.GetContact(0).normal;
            Vector3 vel = direction;
            float impactAngle = Vector3.Angle(vel, -normal);
            //Debug.DrawRay(collision.GetContact(0).point, normal, Color.red, 10);
            Debug.DrawRay(collision.GetContact(0).point, direction, Color.red, 10);

            float hpPercent = hitHealth.ChangeHP(-damage);
            hitHealth.gameObject.GetComponent<RoofSupport>().UpdateStrength(hpPercent);
            hitHealth.gameObject.GetComponentInParent<BuildingParent>().OnStructureHit();

            if (impactAngle > 45)
            {
                direction = (rigidbody.velocity = Vector3.Reflect(vel, normal) * velocity).normalized;
            }
            else
            {
                float distance = PenetrateObject(collision);
                damage /= (1 + distance) * 4;
                if (damage < 1)
                    Destroy(gameObject);

            }
        }
        else if (hitHealth.physMat == Health.physicalMaterial.WOOD)
        {
            Debug.DrawRay(collision.GetContact(0).point, direction, Color.red, 10);

            float hpPercent = hitHealth.ChangeHP(-damage);
            hitHealth.gameObject.GetComponent<RoofSupport>().UpdateStrength(hpPercent);
            hitHealth.gameObject.GetComponentInParent<BuildingParent>().OnStructureHit();

            float distance = PenetrateObject(collision);
            damage /= (1 + distance);
            if (damage < 1)
                Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        transform.DetachChildren();
    }

    private Vector3 OppositeImpactPoint(Collision2D collision)
    {
        Bounds hitBounds = collision.collider.bounds;
        //float approxMaxDiameter = (hitBounds.max - hitBounds.min).magnitude;
        float approxMaxDiameter = (hitBounds.extents * 2 * direction).magnitude * 2;
        LayerMask oldMask = collision.collider.gameObject.layer;
        collision.collider.gameObject.layer = LayerMask.NameToLayer("TEMP_RAYCAST");
        RaycastHit2D hitInfo = Physics2D.Raycast(collision.GetContact(0).point + approxMaxDiameter * direction, -direction, approxMaxDiameter, ~LayerMask.NameToLayer("TEMP_RAYCAST"));
        collision.collider.gameObject.layer = oldMask;
        Debug.DrawRay(hitInfo.point, direction, Color.yellow, 10);
        DrawArrow(collision.GetContact(0).point + approxMaxDiameter * direction, -direction * approxMaxDiameter, 10, Color.cyan);
        print(hitInfo.collider.name);
        return hitInfo.point;
    }

    private float PenetrateObject(Collision2D collision)
    {
        Vector3 oppositePoint = OppositeImpactPoint(collision);
        Vector3 originalPoint = transform.position;
        transform.position = oppositePoint;

        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = direction * velocity;
        return (oppositePoint - originalPoint).magnitude;
    }

    void DrawArrow(Vector3 pos, Vector3 direction, float duration, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay(pos, direction, color, duration);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(270 + arrowHeadAngle, 0, 0) * new Vector3(0, 1, 0);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(270 - arrowHeadAngle, 0, 0) * new Vector3(0, 1, 0);
        Debug.DrawRay(pos + direction, right * arrowHeadLength, color, duration);
        Debug.DrawRay(pos + direction, left * arrowHeadLength, color, duration);
    }
}
