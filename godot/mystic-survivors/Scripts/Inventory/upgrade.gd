extends Node
class_name Upgrade

var passive_item: PassiveItem
var weapon_scene: Resource
var weapon_data: WeaponData

var title: String
var level: int
var icon: Sprite2D
var upgrade

func get_upgrade():
	pass

func get_title() -> String:
	return get_upgrade().title
	
func get_level() -> int:
	return get_upgrade().level

func get_description() -> String:
	return get_upgrade().description

func is_next() -> bool:
	return true
