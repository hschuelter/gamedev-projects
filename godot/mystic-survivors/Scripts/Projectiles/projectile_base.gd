extends Area2D
class_name Projectile

@onready var hitbox = $Hitbox
@onready var sprite_2d = $Sprite2D
var particles_hit_scene = preload("res://VFX/particles_hit.tscn")

var damage: float : set = _set_damage, get = _get_damage
var duration: float : set = _set_duration, get = _get_duration
var speed: float : set = _set_speed, get = _get_speed
var knockback: float : set = _set_knockback, get = _get_knockback
var target: Vector2 : set = _set_target, get = _get_target
var pierce: int : set = _set_pierce, get = _get_pierce
var size: float : set = _set_size, get = _get_size

var projectile_timer: float

@onready var hit_effect_audio = $ProjectileAudio/HitEffectAudio

func _ready():
	projectile_timer = 0

func _physics_process(delta: float) -> void:
	projectile_timer += delta
	if projectile_timer >= self.duration:
		queue_free()
	
	move(delta)

func _set_variables(_damage: float, _duration: float, _speed: float, _knockback: float, _pierce: int, _size: float, _target: Vector2):
	self.damage = _damage
	self.duration = _duration
	self.speed = _speed
	self.knockback = _knockback
	self.pierce = _pierce
	self.size = _size
	self.target = _target
	
	await self.ready
	hitbox.knockback = self.knockback
	global_scale *= self.size

func move(delta: float) -> void:
	self.global_position += target.normalized() * self.speed * delta

func play_hit_effect() -> void:
	hit_effect_audio.pitch_scale = randf_range(.8, 1.2)
	hit_effect_audio.play()

func _on_hitbox_area_entered(area):
	area.emit_signal("take_damage", self.damage)
	play_hit_effect()
	pierce -= 1

func spawn_particles_hit():
	var particles_hit = particles_hit_scene.instantiate()
	particles_hit.set("emitting", true)
	particles_hit.global_position = self.global_position
	var gravity_vector = Vector2.RIGHT.rotated(global_rotation) * 250
	particles_hit.process_material.gravity = Vector3(gravity_vector.x, gravity_vector.y, 0)
	get_tree().root.add_child(particles_hit)

func despawn():
	sprite_2d.visible = false
	hitbox.disable_hitbox()
	set_deferred("monitorable", false) 
	set_deferred("monitoring", false) 
	await get_tree().create_timer(0.25).timeout
	queue_free()

#region Setters and Getters =================
func _set_damage(value: float) -> void:
	damage = value

func _get_damage() -> float:
	return damage

func _set_duration(value: float) -> void:
	duration = value

func _get_duration() -> float:
	return duration

func _set_speed(value: float) -> void:
	speed = value

func _get_speed() -> float:
	return speed

func _set_knockback(value: float) -> void:
	knockback = value

func _get_knockback() -> float:
	return knockback

func _set_target(value: Vector2) -> void:
	target = value

func _get_target() -> Vector2:
	return target

func _set_pierce(value: int) -> void:
	pierce = value
	if pierce <= 0:
		#spawn_particles_hit()
		despawn()

func _get_pierce() -> int:
	return pierce


func _set_size(value: float) -> void:
	size = value

func _get_size() -> float:
	return size
#endregion
