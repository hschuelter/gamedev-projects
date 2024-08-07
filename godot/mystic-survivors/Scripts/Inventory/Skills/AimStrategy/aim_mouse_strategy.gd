extends AimStrategy
class_name AimMouseStrategy

func aim() -> Vector2:
	var vector = get_global_mouse_position() - self.global_position
	return vector.normalized()
