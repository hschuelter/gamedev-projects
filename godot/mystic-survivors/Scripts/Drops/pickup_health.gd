extends Pickup
class_name PickupHealth

func _on_body_entered(player):
	player.health_component.heal(value)
	queue_free()
