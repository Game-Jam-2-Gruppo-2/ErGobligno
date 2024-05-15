using System.Threading;
using UnityEngine;

public abstract class MovementStates
{
    public abstract MovementController Controller { get; set; }
    public abstract void Enter(MovementController controller);
    public abstract void Tick();
    public abstract void FixedTick();
    public abstract void Exit();
    public abstract void Collision(Collision other);
    public abstract void CollisionExit(Collision other);
}

public class MoveState : MovementStates
{
    public override MovementController Controller { get; set; }
    protected AnimationCurve usedCurve;
    protected float speed;
    protected float minSpeed, maxSpeed;
    protected float timer = 0, lastTimer = 0;
    protected float timerIdle = 0, lastInputDuration;
    protected float duration;
    protected Vector3 vel, lastDir;
    protected bool HasToFinish;

    public MoveState(float minSpeed, float maxSpeed, float duration, bool HasToFinish, AnimationCurve usedCurve)
    {
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
        this.duration = duration;
        this.usedCurve = usedCurve;
        this.HasToFinish = HasToFinish;
    }


    #region shortcuts
    /// <summary>
    /// usedCurve.Evaluate(timer)
    /// </summary>
    protected float CurveValue => usedCurve.Evaluate(timer);

    /// <summary>
    /// Mathf.Lerp(minSpeed, maxSpeed, CurveValue)
    /// </summary>
    protected float GetSpeed => Mathf.Lerp(minSpeed, maxSpeed, CurveValue);

    /// <summary>
    /// get => Controller.MoveDir; 
    /// set => Controller.MoveDir = value;
    /// </summary>
    protected Vector3 GetMoveDir { get => Controller.MoveDir; set => Controller.MoveDir = value; }

    /// <summary>
    /// InputManager.MovementDir
    /// </summary>
    protected Vector3 GetInputDir => InputManager.MovementDir;

    /// <summary>
    ///  get => Controller.LastSpeed; set => Controller.LastSpeed = value;
    /// </summary>
    protected float GetLastSpeed { get => Controller.LastSpeed; set => Controller.LastSpeed = value; }

    /// <summary>
    /// Controller.MaxSpeed
    /// </summary>
    protected float GetMaxSpeed => Controller.MaxSpeed;

    /// <summary>
    /// get => Controller.LastDot;
    /// set => Controller.LastDot = value;
    /// </summary>
    protected float GetLastDot { get => Controller.LastDot; set => Controller.LastDot = value; }

    /// <summary>
    /// Controller.maxDotProduct
    /// </summary>
    protected float GetMaxDot => Controller.maxDotProduct;

    /// <summary>
    /// get => Controller.CollisionNormal; 
    /// set => Controller.CollisionNormal = value;
    /// </summary>
    protected Vector3 Normal { get => Controller.CollisionNormal; set => Controller.CollisionNormal = value; }
    #endregion

    public override void Enter(MovementController controller)
    {

    }

    public override void FixedTick()
    {
        if (GetLastDot < GetMaxDot) //Turn check
        {
            Controller.ChangeState
            (
                new MoveState
                (
                    GetLastSpeed / Controller.ChangeDirSpeedDivident,
                    GetLastSpeed,
                    Controller.DecelerationTime,
                    true,
                    Controller.DecelerationCurve
                 )
            );
        }
    }
    public override void Tick()
    {
        GetLastDot = Vector3.Dot(InputManager.MovementDir, GetMoveDir);
    }


    public override void Exit()
    {
        GetLastSpeed = speed;
    }

    public override void Collision(Collision other)
    {

    }

    public override void CollisionExit(Collision other)
    {

    }

}

public class WalkState : MoveState
{

    public WalkState(float minSpeed, float maxSpeed, float duration, bool HasToFinish, AnimationCurve usedCurve) : base(minSpeed, maxSpeed, duration, HasToFinish, usedCurve)
    {
        
    }
    public override MovementController Controller { get => base.Controller; set => base.Controller = value; }
    public override void Enter(MovementController controller)
    {
        if (GetLastSpeed > maxSpeed)
        {
            usedCurve = controller.DecelerationCurve;
            duration = controller.DecelerationTime;
            minSpeed = maxSpeed;
            maxSpeed = GetLastSpeed;
        }
    }

    public override void FixedTick()
    {
        base.FixedTick();
        GetMoveDir = GetInputDir;
    }

    public override void Tick()
    {
       

        base.Tick();
    }

    public override void Collision(Collision other)
    {
        base.Collision(other);
    }

    public override void CollisionExit(Collision other)
    {
        base.CollisionExit(other);
    }

    public override void Exit()
    {
        base.Exit();
    }

}

public class JumpState : MovementStates
{
    public override MovementController Controller { get; set; }

    /// <summary>
    /// get => Controller.CollisionNormal; 
    /// set => Controller.CollisionNormal = value;
    /// </summary>
    Vector3 Normal { get => Controller.CollisionNormal; set => Controller.CollisionNormal = value; }
    public override void Enter(MovementController controller)
    {
        Controller = controller;
        controller.Rb.AddForce(controller.transform.up * controller.JumpForce, ForceMode.Impulse);
        controller.IsAirborne = true;
    }

    public override void FixedTick()
    {

    }

    public override void Tick()
    {
        if (Normal.y > 0)
        {
            Controller.ChangeState(new MovingState());
        }
    }

    public override void Collision(Collision other)
    {
        Normal = other.contacts[0].normal;
        if (Normal.y > 0)
            Controller.ChangeState(new MovingState());
    }

    public override void CollisionExit(Collision other)
    {

    }

    public override void Exit()
    {
        Controller.IsAirborne = false;
    }
}

public class FallingState : MovementStates
{
    public override MovementController Controller { get; set; }

    /// <summary>
    /// get => Controller.CollisionNormal; 
    /// set => Controller.CollisionNormal = value;
    /// </summary>
    Vector3 Normal { get => Controller.CollisionNormal; set => Controller.CollisionNormal = value; }

    /// <summary>
    /// InputManager.MovementDir
    /// </summary>
    Vector3 GetInputDir => InputManager.MovementDir;

    public override void Enter(MovementController controller)
    {
        Controller = controller;
        controller.IsAirborne = true;
    }

    public override void FixedTick()
    {

    }

    public override void Tick()
    {
        if (Normal.y > 0)
        {
            Controller.IsAirborne = false;
            Controller.ChangeState(new MovingState());
        }
        else if (GetInputDir != Vector3.zero)
        {
            Controller.MaxSpeed = Controller.AirborneSpeed;

            Controller.ChangeState(new MovingState());
        }
    }

    public override void Collision(Collision other)
    {
        Normal = other.contacts[0].normal;
    }

    public override void CollisionExit(Collision other)
    {
        Normal = new();
    }

    public override void Exit()
    {

    }
}

public class ClimbState : MovementStates
{
    public override MovementController Controller { get; set; }
    Vector3 startpos;
    Vector3 endPos = new();
    Vector3 lerpedPos;
    float timer;

    public override void Enter(MovementController controller)
    {
        Controller = controller;
        Controller.MyCollider.enabled = false;
        Controller.Rb.useGravity = false;
        Controller.isClimbing = true;
        Controller.IsAirborne = false;

        startpos = Controller.transform.position;
        endPos = startpos;
        endPos.y = Controller.ClimbableObject.bounds.max.y + Controller.MyCollider.bounds.extents.y + Controller.ClimbOffsetY; // + controller.ClimbOffset
        endPos.z = controller.transform.position.z + Controller.ClimbOffsetZ;

        Controller.Rb.velocity = Vector3.zero;
        MovementController.climb?.Invoke();
    }
    public override void FixedTick()
    {
        if (timer < Controller.ClimbDuration)
        {
            timer += Time.fixedDeltaTime;
            lerpedPos = Vector3.Lerp(startpos, endPos, timer / Controller.ClimbDuration);
            Controller.Rb.MovePosition(lerpedPos);
        }
        else
        {
            Controller.Rb.MovePosition(endPos);
            Controller.ChangeState(new MovingState());
        }
    }

    public override void Tick()
    {

    }
    public override void Collision(Collision other)
    {

    }

    public override void CollisionExit(Collision other)
    {

    }


    public override void Exit()
    {
        Controller.MyCollider.enabled = true;
        Controller.Rb.useGravity = true;
        Controller.StartCoroutine(Controller.CheckLedgeCooldown());
    }
}