extends Control

@onready var title_label = %TitleLabel
@onready var level_label = %LevelLabel
@onready var description_label = %DescriptionLabel
@onready var sprite_2d = %Sprite2D
@onready var button = $Button

var selected: Upgrade

func update_info(upgrade: Upgrade) -> void:
	self.selected = upgrade
	title_label.text = upgrade.get_title()
	level_label.text = "Lv. " + str(upgrade.get_level() + 1)
	sprite_2d.texture = upgrade.icon.texture
	
	#var desc: String = upgrade.get_description()
	description_label.text = upgrade.get_description()
