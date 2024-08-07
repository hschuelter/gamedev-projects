extends AimStrategy
class_name AimRandomStrategy

func aim() -> Vector2:
	var angle = randf() * 2 * PI
	var vector = Vector2(cos(angle),sin(angle))
	
	return vector.normalized()
