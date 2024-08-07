extends PassiveItem
class_name KnoUpPassiveItem

func _set_item_data() -> void:
	item_data = preload("res://Resources/PassiveItems/kno_up.tres")

func apply_modifier() -> void:
	super.apply_modifier()
	stats.knockback_modifier *= (1 + multiplier)

func get_description() -> String:
	return description.format({"multiplier": str(multiplier * 100)})
