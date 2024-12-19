extends WorldObject
class_name Checkpoint

func _on_body_entered(player: Player) -> void:
	player.set_checkpoint(self.global_position)
