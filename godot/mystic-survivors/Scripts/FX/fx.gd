extends Node2D
class_name FX

@export var move_direction: Vector2 = Vector2(1, 1)
@export var move_speed: float = 10
@export var duration: float = 10
@onready var timer = $Timer
@onready var animation_player = $AnimationPlayer

func _ready():
	animation_player.play("fade_in")
	move_direction = move_direction.normalized()
	
	var rng = RandomNumberGenerator.new()
	timer.wait_time = rng.randi_range(duration, duration + 1)
	
	timer.start()

func _process(delta):
	self.global_position += move_direction.normalized() * move_speed * delta

func _on_timer_timeout():
	animation_player.play("fade_out")
