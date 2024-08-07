extends Node2D
class_name Inventory

var starting_weapon = preload("res://Scenes/Skills/skill_bow.tscn")
var title = "Bow"

func instantiate_weapon() -> Skill:
	var weapon_node = starting_weapon.instantiate()
	weapon_node.name = title
	return weapon_node
