extends PassiveItem
class_name PieUpPassiveItem

func _set_item_data() -> void:
	item_data = preload("res://Resources/PassiveItems/pie_up.tres")

func apply_modifier() -> void:
	super.apply_modifier()
	stats.pierce_modifier += multiplier

func get_description() -> String:
	return description.format({"multiplier": str(multiplier)})
