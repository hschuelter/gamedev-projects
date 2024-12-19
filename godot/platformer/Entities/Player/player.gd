extends CharacterBody2D
class_name Player

@export var jump_height : float = 75.0
@export var jump_time_ascend : float = 0.5
@export var jump_time_descend : float = 0.35
@export var coyote_time : float = 0.5
@export var jump_buffer_time : float = 0.1

#region Constants
@onready var jump_velocity : float = (-2.0 * jump_height) / jump_time_ascend
@onready var jump_gravity : float = (2.0 * jump_height) / pow(jump_time_ascend, 2)
@onready var fall_gravity : float = (2.0 * jump_height) / pow(jump_time_descend, 2)
const SPEED = 100.0
const TERMINAL_VELOCITY = 250;
#endregion

@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D
@onready var target: Marker2D = $Marker2D
@onready var jump_available = false
@onready var jump_buffer = false

var check_point: Vector2
var coins_collected: Array

func _ready() -> void:
	coins_collected = []

func _physics_process(delta: float) -> void:
	if is_on_floor():
		jump_available = true
	
	var direction := Vector2(
			Input.get_axis("input_left", "input_right"),
			Input.get_axis("input_up", "input_down")).normalized()
	
	if not is_on_floor():
		velocity.y += handle_gravity() * delta
		velocity.y = clampf(velocity.y, -INF, TERMINAL_VELOCITY);
		
		if jump_available:
			get_tree().create_timer(coyote_time).timeout.connect(_coyote_timeout)
	else:
		jump_available = true
		if jump_buffer:
			jump()
			jump_buffer = false
	
	if Input.is_action_just_pressed("input_jump"):
		if jump_available: 
			jump()
			jump_available = false
		else:
			jump_buffer = true
			get_tree().create_timer(jump_buffer_time).timeout.connect(_jump_buffer_timeout)
	
	handle_movement(direction.x)
	handle_animation(direction.x)
	move_and_slide()

func handle_gravity() -> float:
	if Input.is_action_pressed("ui_accept") and not is_on_floor() and velocity.y < 0:
		return jump_gravity
	return fall_gravity

func jump() -> void:
	velocity.y = jump_velocity

func handle_movement(direction: float) -> void:
	if direction:
		velocity.x = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)

func handle_animation(direction: float) -> void:
	if is_on_floor():
		if velocity.x != 0:
			animated_sprite_2d.play('run')
		else:
			animated_sprite_2d.play('idle')
	else:
		if velocity.y < 0: 
			animated_sprite_2d.play('rising')
		else:
			animated_sprite_2d.play('falling')
	
	if direction > 0:
		animated_sprite_2d.scale.x = 1
	elif direction < 0:
		animated_sprite_2d.scale.x = -1
		

func set_checkpoint(position: Vector2) -> void:
	self.check_point = position

func respawn() -> void:
	self.global_position = check_point
	for coin in coins_collected:
		coin.target = null
	coins_collected = []

func _coyote_timeout() -> void:
	jump_available = false
	
func _jump_buffer_timeout() -> void:
	jump_buffer = false
