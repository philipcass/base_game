using UnityEngine;
using System.Collections.Generic;
using System;

public class Platform : MonoBehaviour {
    FPNodeLink link;
    FSprite sprite;
    BoxCollider boxCollider;

    public static Platform Create() {
        GameObject platformGO = new GameObject("Platform");
        Platform platform = platformGO.AddComponent<Platform>();
        return platform;
    }
 
    public void Init(Vector2 startPos) {
     
        gameObject.transform.position = new Vector3(startPos.x * FPhysics.POINTS_TO_METERS, startPos.y * FPhysics.POINTS_TO_METERS, 0);
     
        sprite = new FSprite(Futile.whiteElement);
        sprite.width = 1000;
        sprite.SetPosition(startPos);
        Futile.stage.AddChild(sprite);
     
        InitPhysics();
     
        Futile.stage.ListenForUpdate(HandleUpdate);
    }
 
    public void Destroy() {
        UnityEngine.Object.Destroy(gameObject);
    }
 
    void InitPhysics() {
     
        boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(sprite.width, sprite.height, 100) * FPhysics.POINTS_TO_METERS;
     
        PhysicMaterial mat = new PhysicMaterial();
        mat.bounciness = 0.3f;
        mat.dynamicFriction = 0.5f;
        mat.staticFriction = 0.5f;
        mat.frictionCombine = PhysicMaterialCombine.Maximum;
        collider.material = mat;

        float speed = 30.0f;
        float angle = RXRandom.Range(0, RXMath.DOUBLE_PI);
        Vector2 startVector = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
    }

    void HandleUpdate() {
        if(Input.GetKey(KeyCode.Space))
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up*100);
        if(Input.GetKey(KeyCode.LeftArrow))
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left*10);
        if(Input.GetKey(KeyCode.RightArrow))
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right*10);
        
        sprite.SetPosition(GetPos());
    }
 
    void HandleFixedUpdate() {

    }
 
    public Vector2 GetPos() {
        return new Vector2(transform.position.x * FPhysics.METERS_TO_POINTS, transform.position.y * FPhysics.METERS_TO_POINTS);
    }
}

