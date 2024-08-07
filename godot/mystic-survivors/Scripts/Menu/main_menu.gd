extends Control

@onready var start_button = %StartButton
@onready var animation_player = $AnimationPlayer

func _ready():
	Engine.max_fps = 60
	start_button.grab_focus()

func _on_start_button_pressed():
	animation_player.play("start_game")

func _on_quit_button_pressed():
	get_tree().quit()

func _on_start_game():
	get_tree().change_scene_to_file("res://Scenes/world.tscn")
	
