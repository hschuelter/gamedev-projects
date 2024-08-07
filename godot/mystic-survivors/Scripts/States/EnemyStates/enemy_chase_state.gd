extends EnemyState
class_name EnemyIdleState

@export var DISTANCE_TO_ATTACK: float = 75

func Enter() -> void:
	pass

func Exit() -> void:
	pass

func Update(delta: float) -> void:
	pass

func Physics_Update(delta: float) -> void:
	distance_to_player = enemy.global_position.distance_to(player.global_position)
	enemy.velocity = Vector2.ZERO
	enemy.velocity += self.global_position.direction_to(player.global_position).normalized()
	if enemy.soft_collision.is_colliding():
		enemy.velocity += enemy.soft_collision.get_push_vector()
		enemy.velocity = enemy.velocity.normalized()
	
	if distance_to_player <= DISTANCE_TO_ATTACK:
		transition.emit(self, "attack")
