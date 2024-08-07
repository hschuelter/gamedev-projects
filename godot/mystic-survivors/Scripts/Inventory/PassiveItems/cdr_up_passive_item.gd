extends PassiveItem
class_name CdrUpPassiveItem

func _set_item_data() -> void:
	item_data = preload("res://Resources/PassiveItems/cdr_up.tres")

func apply_modifier() -> void:
	super.apply_modifier()
	stats.cdr_modifier *= (1 + multiplier)

func get_description() -> String:
	return description.format({"multiplier": str(multiplier * 100)})
