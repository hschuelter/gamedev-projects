class_name HealthComponent
extends Node2D

signal die

var max_health: float : get = _get_max_health, set = _set_max_health
var current_health: float : get = _get_current_health, set = _set_current_health

@onready var health_bar = %HealthBar
@onready var label = $CanvasLayer/HealthBarContainer/Label

func damage(dmg: float) -> void:
	current_health -= dmg

func heal(regen: float) -> void:
	current_health += regen

func update_hp() -> void:
	var display_health = self.current_health
	if self.current_health < 1 and self.current_health > 0.01:
		display_health = 1
	health_bar.scale.x = clamp(self.current_health / floor(self.max_health), 0, 1)
	label.text = str(floor(display_health)) + " / " + str(floor(self.max_health))

# Setters and Getters =======================
func _get_max_health() -> float:
	return max_health

func _set_max_health(value: float) -> void:
	if (max_health < value):
		var increase = value - max_health
		max_health = value
		current_health += increase
	update_hp()

func _get_current_health() -> float:
	return current_health

func _set_current_health(value: float) -> void:
		current_health = clampf(value, 0.0, max_health)
		update_hp()
		if current_health <= 0:
			emit_signal("die")

