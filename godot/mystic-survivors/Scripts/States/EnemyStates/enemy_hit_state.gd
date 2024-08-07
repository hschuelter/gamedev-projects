extends EnemyState
class_name EnemyHitState

func Enter() -> void:
	pass

func Exit() -> void:
	pass

func Update(delta: float) -> void:
	pass

func Physics_Update(delta: float) -> void:
	enemy.velocity = enemy.knockback * enemy.KNOCKBACK_FORCE * delta
