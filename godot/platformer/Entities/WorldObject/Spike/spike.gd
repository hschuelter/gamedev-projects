extends WorldObject
class_name Spike

func _on_body_entered(player: Player) -> void:
	#player.check_point = floor(player.check_point / 8) * 8
	print(player.check_point)
	player.respawn()
