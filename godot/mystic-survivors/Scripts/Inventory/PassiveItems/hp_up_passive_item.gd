extends PassiveItem
class_name HpUpPassiveItem

func _set_item_data() -> void:
	item_data = preload("res://Resources/PassiveItems/hp_up.tres")

func apply_modifier() -> void:
	super.apply_modifier()
	stats.health_modifier += (stats.health_modifier * multiplier)

func get_description() -> String:
	return description.format({"multiplier": str(multiplier * 100)})
