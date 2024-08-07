class_name LevelComponent
extends Node2D

signal player_level_up

var level: int : get = _get_level, set = _set_level
var experience: float : get = _get_experience, set = _set_experience
var experience_cap: float : get = _get_experience_cap, set = _set_experience_cap

var experience_ranges = [50, 100, 150, 200, 300]

@onready var experience_bar = %ExperienceBar
@onready var label_level = %Label_Level

func _ready() -> void:
	self.level = 1
	self.experience_cap = 100.0
	self.experience = 0.0

func level_up() -> void:
	level += 1
	experience = fmod(experience, experience_cap)
	var index = clamp(floor(level / 10), 0, experience_ranges.size() - 1)
	experience_cap += experience_ranges[index]
	#print(experience_ranges[index], ' -> ', experience_cap)
	emit_signal("player_level_up")

func get_exp(value: float) -> void:
	experience += value

func update_level(value: int) -> void:
	label_level.text = "Lv. " + str(value)

func update_xp(value: float) -> void:
	experience_bar.scale.x = value

# Setters and Getters =======================
func _get_level() -> int:
	return level

func _set_level(value: int) -> void:
	level = value
	update_level(level)

func _get_experience() -> float:
	return experience

func _set_experience(value: float) -> void:
	experience = value
	update_xp(experience / experience_cap)
	if (experience >= experience_cap):
		level_up()

func _get_experience_cap() -> float:
	return experience_cap

func _set_experience_cap(value: float) -> void:
	experience_cap = value
	update_xp(experience / experience_cap)
