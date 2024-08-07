extends Pickup
class_name PickupExperience

func _on_body_entered(player):
	player.level_component.get_exp(value)
	queue_free()
