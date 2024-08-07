extends AimStrategy
class_name AimPlayerInputStrategy

func aim() -> Vector2:
	var aim_direction = Vector2.ZERO
	aim_direction.x = Input.get_axis("ui_left", "ui_right")
	aim_direction.y = Input.get_axis("ui_up", "ui_down")
	
	if aim_direction == Vector2.ZERO:
		aim_direction = last_aim_direction
	
	last_aim_direction = aim_direction
	return aim_direction.normalized()
