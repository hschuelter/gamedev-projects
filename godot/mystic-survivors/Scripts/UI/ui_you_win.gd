extends CanvasLayer
@onready var level_label = $VBoxContainer/LevelLabel

func update_screen(player: Player) -> void:
	var player_level = player.level_component.level
	
	level_label.text = "Level " + str(player_level)

func _on_quit_button_pressed():
	get_tree().paused = false
	reset_objects()
	get_tree().change_scene_to_file("res://Scenes/Menu/main_menu.tscn")

func _on_restart_pressed():
	get_tree().paused = false
	reset_objects()
	get_tree().reload_current_scene()

func reset_objects():
	for projectile in get_tree().get_nodes_in_group("projectiles"):
		projectile.queue_free()
		
	for pickup in get_tree().get_nodes_in_group("pickups"):
		pickup.queue_free()
