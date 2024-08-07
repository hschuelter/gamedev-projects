extends AimStrategy
class_name AimRandomEnemyStrategy

func aim() -> Vector2:
	var target = Vector2(10000, 0)
	var enemie_group = get_tree().get_nodes_in_group("Enemies")
	if enemie_group:
		enemie_group.shuffle()
		target = enemie_group[0].hurtbox.global_position
	return target
