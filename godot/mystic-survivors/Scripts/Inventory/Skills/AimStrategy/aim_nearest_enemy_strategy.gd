extends AimStrategy
class_name AimNearestEnemyStrategy

func aim() -> Vector2:
	var distance = 1000
	var target: Vector2
	var enemie_group = get_tree().get_nodes_in_group("Enemies")
	for e in enemie_group:
		var enemy_distance = self.global_position.distance_to(e.marker_2d.global_position)
		if enemy_distance < distance:
			distance = enemy_distance
			target = e.hurtbox.global_position
	var vector = target - self.global_position
	return vector.normalized()
