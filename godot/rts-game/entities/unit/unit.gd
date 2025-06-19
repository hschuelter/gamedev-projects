extends CharacterBody2D
class_name Unit

@onready var outline: Sprite2D = $Outline
@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D

const SPEED = 100.0

var target = Vector2.ZERO
var is_selected = false
var is_hovering = false

func _input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_RIGHT and event.is_pressed():
			target = event.position
			
		if event.button_index == MOUSE_BUTTON_LEFT and event.is_pressed():
			is_selected = is_hovering and !is_selected

func _physics_process(delta: float) -> void:
	var direction := Vector2.ZERO
	
	
	if is_selected and target:
		direction = target - self.global_position
		handle_animation(direction.angle(), true)
		if direction.length() < 10:
			target = null
		else:
			velocity = direction.normalized() * SPEED
	else:
		velocity = Vector2.ZERO
		target = null
		handle_animation(0, false)

	move_and_slide()

func handle_animation(angle: float, moving: bool):
	angle = angle * 180 / PI
	if not moving: 
		animated_sprite_2d.play("idle")
		animated_sprite_2d.scale.x = 1
		return
	
	if (angle > -45 and angle < 0) or (angle >0 and angle < 45):
		animated_sprite_2d.play("walk_right")
		animated_sprite_2d.scale.x = 1
	elif angle < -45 and angle > -135:
		animated_sprite_2d.play("walk_up")
	elif (angle < -135 and angle > -180) or (angle > 135 and angle < 180):
		animated_sprite_2d.play("walk_right")
		animated_sprite_2d.scale.x = -1
	elif angle > 45 and angle < 135:
		animated_sprite_2d.play("walk_down")

func _on_mouse_entered() -> void:
	is_hovering = true
	#animated_sprite_2d.get_material().set_shader_parameter("outline_color", Vector4(1,1,1,1))
	animated_sprite_2d.get_material().set_shader_parameter("width", 1)

func _on_mouse_exited() -> void:
	if is_selected:
		#animated_sprite_2d.get_material().set_shader_parameter("outline_color", Vector4(1,1,1,1))
		animated_sprite_2d.get_material().set_shader_parameter("width", 1)
	else:
		#animated_sprite_2d.get_material().set_shader_parameter("outline_color", Vector4(1,1,1,1))
		animated_sprite_2d.get_material().set_shader_parameter("width", 0)
	
	is_hovering = false
