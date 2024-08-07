extends PassiveItem
class_name AmoUpPassiveItem

func _set_item_data() -> void:
	item_data = preload("res://Resources/PassiveItems/amo_up.tres")

func apply_modifier() -> void:
	super.apply_modifier()
	stats.amount_modifier += multiplier

func get_description() -> String:
	return description.format({"multiplier": str(multiplier)})
