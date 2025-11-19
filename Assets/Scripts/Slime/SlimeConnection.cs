using UnityEngine;

public class SlimeConnection : MonoBehaviour
{
    private Slime slime;

    private GrabCollision GrabCollision;

    private PunchCollision PunchCollision;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slime = GetComponentInParent<Slime>();
        PunchCollision = GetComponentInChildren<PunchCollision>();
        GrabCollision = GetComponentInChildren<GrabCollision>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SlimeDie()
    {
        slime?.Death();
    }

    public void GrabOn()
    {
        GrabCollision.ActivateGrab();
    }

    public void GrabOff() 
    { 
        GrabCollision.DeactivateGrab(); 
    }

    public void SuccesfulGrab(PlayerController player)
    {
        slime?.Grab(player);
    }

    public void RPunchOn()
    {
        GrabCollision.DeactivateGrab();
        PunchCollision?.ActivateRPunch();
    }

    public void RPunchOff()
    {
        PunchCollision?.DeactivateRPunch();
    }

    public void LPunchOn()
    {
        GrabCollision.DeactivateGrab();
        PunchCollision?.ActivateLPunch();
    }

    public void LPunchOff()
    { 
        PunchCollision?.DeactivateLPunch();
    }

    public void PunchSound()
    {
        slime?.PlayPunchSound();
    }

    public void GrabSound()
    {
        slime?.PlayGrabSound();
    }
}
