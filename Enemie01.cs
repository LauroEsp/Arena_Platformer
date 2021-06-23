using Godot;
using System;

public class Enemie01 : KinematicBody2D
{
    Vector2 velocity;
    int direction = 1;
    float movementSpeed = 50;
    float gravity = 30;
    float maxFallSpeed = 500;
    float minFallSpeed = 5;
    //float jumpForce = 650;
    Sprite sprite;
    AnimationPlayer animationPlayer;
    AnimationTree animationTree;
    AnimationNodeStateMachinePlayback stateMachine;
    RayCast2D rayCastDown;
    Vector2 castTo;
    public override void _Ready()
    {
        sprite = (Sprite)GetNode("Sprite");
        animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        animationTree = (AnimationTree)GetNode("AnimationTree");
        stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        rayCastDown = (RayCast2D)GetNode("RayCastDown");
    }

    public override void _PhysicsProcess(float delta)
    {
        EnemieMovement(delta);
    }

    public void EnemieMovement(float delta)
    {
        // Enemie gravity.
        velocity.y += gravity;
        if(velocity.y > maxFallSpeed)
        {
            velocity.y = maxFallSpeed;
        }
        if(IsOnFloor())
        {
            velocity.y = minFallSpeed;
        }

        // Enemie horizontal movement.
        velocity.x = movementSpeed * direction;
        velocity = MoveAndSlide(velocity, Vector2.Up);
        castTo = rayCastDown.GetPosition();
        if(IsOnWall())
        {
            direction *= -1;
            rayCastDown.SetPosition(castTo*-1);
        }

        if(!rayCastDown.IsColliding())
        {
            direction *= -1;
            rayCastDown.SetPosition(castTo*-1);
        }

        /* Enemie jump.
        if(!rayCastDown.IsColliding())
        {
            direction.y = -jumpForce;
        }*/
        
        // Enemie sprite.
        if(velocity.x > 0)
        {
            sprite.FlipH = false;
        }else if(velocity.x < 0)
        {
            sprite.FlipH = true;
        }

        // Enemie movement animations.
        if(IsOnFloor() && velocity.x == 0)
        {
            stateMachine.Travel("idle");
        }else if(IsOnFloor() && velocity.x != 0)
        {
            stateMachine.Travel("run");
        }

    }

}
