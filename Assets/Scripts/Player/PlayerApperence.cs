using UnityEngine;

public class PlayerApperence : SceneSingletonMonoBehaviour<PlayerApperence>
{
    public SpriteRenderer playerSkin;
    void Start()
    { 
        
    }
    public void SetFlip(Vector2 dir)
    {
        if (dir.x < 0)
        {
            playerSkin.flipX = true;
        }
        else
        {
            playerSkin.flipX = false;
        }
    }

}
