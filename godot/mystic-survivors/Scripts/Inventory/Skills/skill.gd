extends Node2D
class_name Skill

@onready var sprite_2d = $Sprite2D
@onready var marker_2d = $Marker2D
@onready var cooldown_timer: Timer = $CooldownTimer
@onready var bullet_scene

var stats_component: StatsComponent
@onready var aim_strategy: AimStrategy
@export var weapon_data: WeaponData
var weapon_scene: Resource
var title: String
var level: int

var last_aim_direction: Vector2 = Vector2.RIGHT
var can_shoot = false
@onready var amount_timer = $AmountTimer
var amount

func _init():
	_set_weapon_data()
	_set_weapon_scene()
	self.title = weapon_data.title
	self.level = weapon_data.level
	self.aim_strategy = AimMouseStrategy.new()

func _ready():
	_set_weapon_data()
	self.title = weapon_data.title
	self.level = weapon_data.level
	
	self.aim_strategy.name = "AimStrategy"
	add_child(self.aim_strategy)
	
	add_child(amount_timer)
	start_cooldown()

func _physics_process(delta):
	var aim_direction = aim_strategy.aim()
	rotate_to_direction(aim_direction)
	if cooldown_timer.is_stopped() and not can_shoot:
		can_shoot = true
		amount = _get_amount()
	
	if can_shoot and amount_timer.is_stopped():
		shoot(aim_direction)
		amount_timer.wait_time = _get_cooldown() * 0.12
		amount_timer.start()
		amount -= 1
		
		if amount <= 0:
			start_cooldown()
			can_shoot = false

func shoot(aim_vector : Vector2) -> void:
	var target: Vector2 = aim_vector
	
	var projectile: Projectile = bullet_scene.instantiate()
	projectile.global_position = marker_2d.global_position
	projectile.global_rotation = self.rotation
	projectile._set_variables(
		_get_damage(), 
		_get_duration(), 
		_get_projectile_speed(), 
		_get_knockback(), 
		_get_pierce(),
		_get_size(),
		target
	)
	get_tree().root.add_child(projectile)

func level_up() -> void:
	if self.weapon_data.next:
		self.weapon_data = self.weapon_data.next
		self.level += 1

func rotate_to_direction(vector: Vector2) -> void:
	self.rotation = vector.normalized().angle()

func start_cooldown() -> void:
	cooldown_timer.wait_time = _get_cooldown()
	cooldown_timer.start()

func _set_weapon_data() -> void:
	self.weapon_data = preload("res://Resources/Weapons/Bow/bow_lv_1.tres")

func _set_weapon_scene() -> void:
	self.weapon_scene = preload("res://Scenes/Skills/skill.tscn")
	
func _get_damage() -> float:
	return weapon_data.damage * stats_component.get_damage()

func _get_duration() -> float:
	return weapon_data.duration * stats_component.get_duration()
	
func _get_projectile_speed() -> float:
	return weapon_data.speed * stats_component.get_projectile_speed()
	
func _get_knockback() -> float:
	return weapon_data.knockback * stats_component.get_knockback()
	
func _get_pierce() -> int:
	return weapon_data.pierce + stats_component.get_pierce()
	
func _get_cooldown() -> float:
	return weapon_data.cooldown / stats_component.get_cdr()

func _get_size() -> float:
	return weapon_data.size * stats_component.get_size()

func _get_amount() -> int:
	return weapon_data.amount + stats_component.get_amount()
