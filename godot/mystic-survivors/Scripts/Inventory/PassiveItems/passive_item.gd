extends Node
class_name PassiveItem

var stats: StatsComponent
var item_data: PassiveItemData

var multiplier: float
var level: int
var title: String : get = get_title
var description: String : get = get_description
#var upgrade: Resource

# NextLevelPrefab
# Icon

func _init():
	_set_item_data()
	self.level = item_data.level
	self.multiplier = item_data.multiplier
	self.title = item_data.title
	self.description = item_data.description

func _set_item_data() -> void:
	pass

func apply_modifier() -> void:
	level += 1

func print():
	pass
	#print(title, " Lv. ", level)
	#print("multiplier: ", multiplier)
	#print("description: ", description)

func get_title() -> String:
	return title

func get_description() -> String:
	return description
