extends Area2D

@onready var player = $".."

func _on_area_entered(area):
	area.player = player
	area.picked_up = true
