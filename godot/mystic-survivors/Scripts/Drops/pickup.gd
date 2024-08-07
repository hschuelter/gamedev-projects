extends Area2D
class_name Pickup

const SPEED = 120

@export var value: float = 0.0
var player: Player
var picked_up = false

func _physics_process(delta):
	if picked_up:
		move(delta)

func move(delta: float):
	var target = player.global_position - self.global_position
	self.global_position += target.normalized() * SPEED * delta

func _on_body_entered(body):
	pass # Replace with function body.
