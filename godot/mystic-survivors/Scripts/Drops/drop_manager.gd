extends Node2D

@export var possible_drops: Array = []

func drop_pickups():
	for drop in possible_drops:
		if randf() < drop.drop_rate:
			var pickup = drop.pickup_scene.instantiate()
			pickup.value = drop.value
			pickup.global_position = self.global_position
			pickup.global_position += Vector2( (randf()-0.5) * 5, (randf()-0.5) * 5)
			get_tree().root.add_child(pickup)
