extends EnemyAttack
class_name EnemySkullAttack

var projectile_scene = preload("res://Scenes/Enemies/EnemyAttacks/enemy_projectile_skull.tscn")

func Attack(player: Player, enemy: Enemy):
	var target = player.global_position - enemy.global_position
	var projectile: Projectile = projectile_scene.instantiate()
	projectile.global_position = enemy.marker_2d.global_position
	#projectile.global_rotation = target.angle()
	projectile._set_variables(
		enemy.stats.damage,
		2.0, 
		100, 
		0, 
		1,
		1,
		target 
	)
	get_tree().root.add_child(projectile)

