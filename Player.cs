using Godot;
using System;

public class Player : KinematicBody2D
{
    Vector2 direction;
    float movementSpeed = 250;
    float gravity = 30;
    float maxFallSpeed = 500;
    float minFallSpeed = 5;
    float jumpForce = 650;
    Sprite sprite;
    AnimationPlayer animationPlayer;
    AnimationTree animationTree;
    AnimationNodeStateMachinePlayback stateMachine;
    public override void _Ready()
    {
        sprite = (Sprite)GetNode("Sprite");
        animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        animationTree = (AnimationTree)GetNode("AnimationTree");
        stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
    }

    public override void _PhysicsProcess(float delta)
    {
        PlayerMovement();
        PlayerAttack();
    }

    public void PlayerMovement()
    {
        // Player gravity.
        direction.y += gravity;
        if(direction.y > maxFallSpeed)
        {
            direction.y = maxFallSpeed;
        }
        if(IsOnFloor())
        {
            direction.y = minFallSpeed;
        }
        
        // Player horizontal movement.
        direction.x = Input.GetActionStrength("move_right")-Input.GetActionStrength("move_left");
        direction.x *= movementSpeed;

        // Player jump.
        if(IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            direction.y = -jumpForce;
        }

        direction = MoveAndSlide(direction, Vector2.Up);

        // Fliping sprite.
        if(direction.x > 0)
        {
            sprite.FlipH = false;
        }else if(direction.x < 0)
        {
            sprite.FlipH = true;
        }

        

        // Playing movement animations.
        if(IsOnFloor() && direction.x == 0)
        {
            stateMachine.Travel("idle");
        }else if(IsOnFloor() && direction.x != 0)
        {
            stateMachine.Travel("run");
        }else if(!IsOnFloor())
        {
            stateMachine.Travel("jump");
        }
        
    }

    public void PlayerAttack()
    {
        // Player attack animation.
        if(Input.IsActionJustPressed("attack"))
        {
            stateMachine.Travel("attack");
        }
    }

}
