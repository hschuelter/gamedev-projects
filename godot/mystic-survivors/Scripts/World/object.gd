extends StaticBody2D

var health = 10
@onready var damage_number = $DamageNumber
@onready var drop_manager = $DropManager

func _ready():
	drop_manager.possible_drops = [
		Drop.new(0., 5, preload("res://Scenes/Pickups/pickup_health.tscn"))
	]

func take_damage(damage: float) -> void:
	health -= damage
	DamageNumbersManager.display_number(damage, damage_number.global_position, false)
	if health <= 0:
		die()

func die() -> void:
	drop_manager.drop_pickups()
	queue_free()
