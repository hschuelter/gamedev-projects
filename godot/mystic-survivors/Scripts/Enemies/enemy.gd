extends Area2D
class_name Enemy

@export var stats: StatsEnemy
var current_health: float

@export var player: Player
@onready var soft_collision = $Soft_Collision
@onready var animation_player = $AnimationPlayer
@onready var hurtbox = $Hurtbox
@onready var hitbox = $Hitbox
@onready var state_machine = $StateMachine
@onready var marker_2d = $Marker2D
@onready var damage_number = $DamageNumber
@onready var drop_manager = $DropManager

var velocity: Vector2
var knockback: Vector2
var KNOCKBACK_FORCE: float = 0.4
var PUSH_FORCE: float = 3
var RESPAWN_DISTANCE: float = 512

signal mob_died
signal mob_respawn

func _ready():
	current_health = stats.max_health
	for child in state_machine.get_children():
		child.player = player
	
	drop_manager.possible_drops = [
		Drop.new(0.60, stats.exp_granted, preload("res://Scenes/Pickups/pickup_experience.tscn")),
		Drop.new(0.01, 5, preload("res://Scenes/Pickups/pickup_health.tscn"))
	]

func _physics_process(delta):
	move(velocity, delta)
	
	var distance = self.global_position.distance_to(player.global_position)
	if (distance >= RESPAWN_DISTANCE):
		respawn()

func move(velocity: Vector2, delta: float) -> void:
	self.global_position += velocity * stats.move_speed * delta

func take_damage(damage: float) -> void:
	current_health -= damage
	DamageNumbersManager.display_number(damage, damage_number.global_position, false)
	if current_health <= 0:
		die()

func respawn() -> void:
	emit_signal("mob_respawn", self)

func die() -> void:
	#player.gain_exp(stats.exp_granted)
	drop_manager.drop_pickups()
	emit_signal("mob_died")
	queue_free()

func change_state_to_hit() -> void:
	state_machine._on_state_transition(state_machine.current_state, "hit")
	hurtbox.disable_hurtbox()
	hitbox.disable_hitbox()
	#soft_collision.disable_soft_collision()
	animation_player.play("enemy_hit")

func change_state_to_chase() -> void:
	state_machine._on_state_transition(state_machine.current_state, "chase")
	hurtbox.enable_hurtbox()
	hitbox.enable_hitbox()
	soft_collision.enable_soft_collision()
	animation_player.play("enemy_default")

func _on_hurtbox_area_entered(area):
	var kno_vector = self.global_position - player.global_position
	knockback =  kno_vector.normalized() * area.knockback
	change_state_to_hit()

func _on_hitbox_area_entered(area):
	area.emit_signal("take_damage", stats.damage)
